using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;// pair UI_slot with System_slot
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;
    private PlayerInventoryHolder playerInventory;
    private ChestInventory chestInventory;
    private InventoryUIControler inventory;
    
    protected virtual void Start(){
        playerInventory = GetComponentInParent<PlayerInventoryHolder>();
        inventory = GetComponentInParent<InventoryUIControler>();
    }
   
    public abstract void AssignSlot(InventorySystem invDisplay,int offset);// implement in child class
    protected virtual void UpdateSLot(InventorySlot updatedSlot)
    {
        foreach(var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)// slot value
            {
                slot.Key.UpdateUISlot(updatedSlot);// slot key - UI representation of the value
            }
        }
    }
    public void SlotCliked(InventorySlot_UI clickedUISlot, int mouseNumPress)
    {
        
        bool isShiftPress = Keyboard.current.leftShiftKey.isPressed;
        switch (mouseNumPress){
            case 0: 
                // Clicked slot has an item - mouse doesn't have an item - pick up item
                if (clickedUISlot.AssignInventorySlot.ItemData != null && mouseInventoryItem.AssignInventorySlot.ItemData == null)
                {
                    // if shift key hold - split stack
                    if (isShiftPress && clickedUISlot.AssignInventorySlot.SplitStack( out InventorySlot halfStackSlot))
                    {
                        mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                        clickedUISlot.UpdateUISlot();
                        return;
                    }
                    else// pick item in the clicked slot
                    {
                        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignInventorySlot);
                        clickedUISlot.ClearSlot();
                        return;
                    } 
                }
                // Clicked slot doesn't has an item - mouse does have an item - place to empty slot
                if (clickedUISlot.AssignInventorySlot.ItemData == null && mouseInventoryItem.AssignInventorySlot.ItemData != null)
                {
                    clickedUISlot.AssignInventorySlot.AssignItem(mouseInventoryItem.AssignInventorySlot);
                    clickedUISlot.UpdateUISlot();

                    mouseInventoryItem.ClearSlot();
                    return;
                }
                //both slot have item - decide
                if (clickedUISlot.AssignInventorySlot.ItemData != null && mouseInventoryItem.AssignInventorySlot.ItemData != null)
                {
                    // if different - swap
                    if (clickedUISlot.AssignInventorySlot.ItemData != mouseInventoryItem.AssignInventorySlot.ItemData)
                    {
                        SwapSlot(clickedUISlot);
                        return;
                    }
                    // if both are the same && enought room - combine
                    if (clickedUISlot.AssignInventorySlot.ItemData == mouseInventoryItem.AssignInventorySlot.ItemData
                        && clickedUISlot.AssignInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignInventorySlot.StackSize))
                    {
                        clickedUISlot.AssignInventorySlot.AssignItem(mouseInventoryItem.AssignInventorySlot);
                        clickedUISlot.UpdateUISlot();

                        mouseInventoryItem.ClearSlot();
                        return;
                    }
                    // if both are the same && not enought room - take needed
                    else if (clickedUISlot.AssignInventorySlot.ItemData == mouseInventoryItem.AssignInventorySlot.ItemData
                        && !clickedUISlot.AssignInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignInventorySlot.StackSize, out int leftInStack))
                    {
                        if (leftInStack < 1) SwapSlot(clickedUISlot);// swap when stack full
                        else // take what need if stack NOT full
                        {
                            int remainOnMouse = mouseInventoryItem.AssignInventorySlot.StackSize - leftInStack;
                            clickedUISlot.AssignInventorySlot.AddToStack(leftInStack);
                            clickedUISlot.UpdateUISlot();

                            var newItem = new InventorySlot(mouseInventoryItem.AssignInventorySlot.ItemData, remainOnMouse);
                            mouseInventoryItem.ClearSlot();
                            mouseInventoryItem.UpdateMouseSlot(newItem);
                            return;
                        }
                    }
                    else if (clickedUISlot.AssignInventorySlot.ItemData != mouseInventoryItem.AssignInventorySlot.ItemData)
                    {
                        SwapSlot(clickedUISlot);
                        return;
                    }
                }   
                break;
            case 1:
                InventorySlot thisSlot = clickedUISlot.AssignInventorySlot;
                List <InventorySlot> slots = playerInventory.PrimaryInventorySystem.InventorySlots;
                if(!inventory.inventoryPanel.isActiveAndEnabled)
                {
                    if(slots.IndexOf(thisSlot) <= 8){ 
                    //slot in hotbar
                    moveItemInBackpackOnPlayer(clickedUISlot,thisSlot,slots,9,slots.Count); // move item from hot bar to backpack
                    }else{
                        // slot in backpack
                        moveItemInBackpackOnPlayer(clickedUISlot,thisSlot,slots,0,9);// move item from backpack to hot bar
                    }
                }
                else{
                    if(slots.Contains(thisSlot))
                    {
                        chestInventory = GetComponentInParent<Interactor>().chest.GetComponent<ChestInventory>();
                        List <InventorySlot> chestSlots = chestInventory.PrimaryInventorySystem.InventorySlots;
                        moveItemInBackpackOnChest(clickedUISlot,thisSlot,chestSlots,0,chestSlots.Count);
                    }
                    else{
                        moveItemInBackpackOnPlayer(clickedUISlot,thisSlot,slots,0,slots.Count);
                    }
                }
                break;
        }  
    }
    void moveItemInBackpackOnPlayer(InventorySlot_UI clickedUISlot,InventorySlot thisSlot,List<InventorySlot> slots,int start, int end){
        bool isAdded = false;
        for(int i = start; i < end; i++){
                if (slots[i].ItemData == thisSlot.ItemData){// check same item to merge
                    if(slots[i].EnoughRoomLeftInStack(thisSlot.StackSize)){
                        slots[i].AddToStack(thisSlot.StackSize);
                        playerInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(slots[i]);
                        clickedUISlot.ClearSlot();
                        isAdded = true;
                        break;
                    }else if (!slots[i].RoomLeftInStack(thisSlot.StackSize, out int amountToAdd) && amountToAdd != 0){
                        slots[i].AddToStack(amountToAdd);
                        thisSlot.RemoveFromStack(amountToAdd);
                        playerInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(slots[i]);
                        clickedUISlot.UpdateUISlot();
                        break;
                    }
                }
            }
            if(!isAdded){ // whether this had
                for(int i = start; i < end; i++){
                    if(slots[i].ItemData == null){
                        slots[i].UpdateInventorySlot(thisSlot.ItemData,thisSlot.StackSize);
                        playerInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(slots[i]);
                        clickedUISlot.ClearSlot();
                        break;
                    }
                }
            }
    }
    void moveItemInBackpackOnChest(InventorySlot_UI clickedUISlot,InventorySlot thisSlot,List<InventorySlot> chestSlots,int start, int end)
    {
        bool isAdded = false;
        for(int i = start; i < end; i++)
        {
            if (chestSlots[i].ItemData == thisSlot.ItemData){// check same item to merge
                if(chestSlots[i].EnoughRoomLeftInStack(thisSlot.StackSize)){
                    chestSlots[i].AddToStack(thisSlot.StackSize);
                    chestInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(chestSlots[i]);
                    clickedUISlot.ClearSlot();
                    isAdded = true;
                    break;
                }else if (!chestSlots[i].RoomLeftInStack(thisSlot.StackSize, out int amountToAdd) && amountToAdd != 0){
                    chestSlots[i].AddToStack(amountToAdd);
                    thisSlot.RemoveFromStack(amountToAdd);
                    chestInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(chestSlots[i]);
                    clickedUISlot.UpdateUISlot();
                    break;
                }
            }
        }
        if(!isAdded){ // whether this had
            for(int i = start; i < end; i++){
                if(chestSlots[i].ItemData == null){
                    chestSlots[i].UpdateInventorySlot(thisSlot.ItemData,thisSlot.StackSize);
                    chestInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(chestSlots[i]);
                    clickedUISlot.ClearSlot();
                    break;
                }
            }
        }
    }

    private void SwapSlot(InventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignInventorySlot.ItemData, mouseInventoryItem.AssignInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();
        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
