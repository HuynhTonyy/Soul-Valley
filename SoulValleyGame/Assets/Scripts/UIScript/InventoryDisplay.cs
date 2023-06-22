using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;
    protected virtual void Start()
    {

    }
    public abstract void AssignSlot(InventorySystem invDisplay);
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
       

        // Clicked slot has an item - mouse doesn't have an item - pick up item
        if (clickedUISlot.AssignInventorySlot.ItemData != null && mouseInventoryItem.AssignInventorySlot.ItemData == null)
        {
            // if shift key hold - split stack
            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignInventorySlot);
            clickedUISlot.ClearSlot();
            return;
        }

        // Clicked slot doesn't has an item - mouse does have an item - place to empty slot
        if (clickedUISlot.AssignInventorySlot.ItemData == null && mouseInventoryItem.AssignInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignInventorySlot.AssignItem(mouseInventoryItem.AssignInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
        }

        //both slot have item - decide
        // if both are the same - combine
        // is the slot stack size + mouse stack > the slot max stack size ? take from mouse
        // if different - swap
    }
}
