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
    protected virtual void Start()
    {

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
    public void SlotCliked(InventorySlot_UI clickedUISlot)
    {
        bool isShiftPress = Keyboard.current.leftShiftKey.isPressed;

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
