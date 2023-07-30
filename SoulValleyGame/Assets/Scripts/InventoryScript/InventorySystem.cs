using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private int inventorySize;
    public InventorySlot_UI[] inventorySlot;

    public void setInventorySlots(List<InventorySlot> inventorySlots)
    {
        this.inventorySlots = inventorySlots;
    }
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)// Contructor set amount of slot
    {
        inventorySlots = new List<InventorySlot>(size);
        for(int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public void AssignItemBySlotIndex(ItemScript item, int amount ,int index)
    {
        inventorySlots[index] = new InventorySlot(item,amount);
    }
    public bool AddToInventory(ItemScript item,int amount)
    {
        if (ContainsItem(item, out List<InventorySlot> invSlot))// Check if Item exist in inventory
        {
            foreach(var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    GameEventManager.instance.inventoryEvent.AddItem(item,amount);
                    return true;
                }
            }  
        }
        if (HasFreeSlot(out InventorySlot freeSlot))// Get the first available slot
        {
            if (freeSlot.EnoughRoomLeftInStack(amount))
            {
                freeSlot.UpdateInventorySlot(item, amount);
                OnInventorySlotChanged?.Invoke(freeSlot);
                GameEventManager.instance.inventoryEvent.AddItem(item,amount);
                return true;
            }
        }
        return false;
    }


    public bool ContainsItem(ItemScript item, out List<InventorySlot> invSlot)// any slot have item to add 
    {
        invSlot = inventorySlots.Where(i => i.ItemData == item).ToList(); // get the list of all of them
        return invSlot == null ? false : true;
    }
    public bool ContainSpecificItem(ItemScript item, out List<InventorySlot> invSlot)// any slot have item to add 
    {
        invSlot = inventorySlots.Where(i => i.ItemData == item).ToList(); // get the list of all of them\
        return invSlot.Count == 0  ? false : true;
    }
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null); // get the first free slot
        return freeSlot == null ? false : true;
    }
    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < InventorySize)
        {
            return inventorySlots[index];
        }
        return null;
    }
}
