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
    protected virtual void Start(){
        playerInventory = GetComponentInParent<PlayerInventoryHolder>();
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
                AutoCombineSlot(clickedUISlot);

        
            
                break;
        }  
    }

    private void AutoCombineSlot(InventorySlot_UI clickedUISlot){

        bool isAdded = false;
        List <InventorySlot> slots = playerInventory.PrimaryInventorySystem.InventorySlots;

        // for(int i = 9; i<slots.Count;i++)
        // {   
        //     // if both are the same && enought room - combine
        //     if (slots[i].ItemData == clickedUISlot.AssignInventorySlot.ItemData)
        //     {
        //         if(slots[i].EnoughRoomLeftInStack(clickedUISlot.AssignInventorySlot.StackSize))
        //         {
        //             slots[i] = (clickedUISlot.AssignInventorySlot);
        //             clickedUISlot.UpdateUISlot();
        //             slots[i].ClearSlot();
                  
        //         }
        //         else if (!slots[i].RoomLeftInStack(clickedUISlot.AssignInventorySlot.StackSize, out int leftInStack))
        //         {
        //             int remainOnOriginSlot = clickedUISlot.AssignInventorySlot.StackSize - leftInStack;
        //             slots[i].AddToStack(leftInStack);

        //             clickedUISlot.UpdateUISlot();
        //             slots[i].ClearSlot();
        //         }
        //         isAdded = true;
        //         break;
        //     }
        // }
        if(!isAdded)
        {
            for(int i = 9; i<slots.Count;i++)
            {
                Debug.Log(i);
                if(slots[i].ItemData == null)
                {
                    //slots[i] = clickedUISlot.AssignInventorySlot;
                    slots[i].UpdateInventorySlot(clickedUISlot.AssignInventorySlot.ItemData,clickedUISlot.AssignInventorySlot.StackSize);
                    playerInventory.PrimaryInventorySystem.OnInventorySlotChanged?.Invoke(slots[i]);
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
