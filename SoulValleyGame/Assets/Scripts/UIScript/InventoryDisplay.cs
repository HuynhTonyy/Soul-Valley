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
    public void SlotCliked(InventorySlot_UI clickedSlot)
    {
        Debug.Log("Slot clicked");
    }
}
