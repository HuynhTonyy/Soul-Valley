using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemScript itemData; // reference to the data
    [SerializeField] private int stackSize; // Current stack size

    public void setItemData(ItemScript item)
    {
        itemData = item;
    }
    public void setItemStack(int amount)
    {
        stackSize = amount;
    }
    public ItemScript ItemData => itemData;
    public int StackSize => stackSize;
    public int GetCurrentStackSize()
    {
        return stackSize;
    }
    public InventorySlot(ItemScript source, int amount)// contructor to make a occupied inventoryslot
    {
        itemData = source;
        stackSize = amount;
    }
    public InventorySlot() // contructor to make an empty inventoryslot
    {
        ClearSlot();
    }
    public void ClearSlot() // clear a slot
    {
        itemData = null;
        stackSize = -1;
    }
    public void AssignItem(InventorySlot invSlot) // assign Item to slot
    {
        if (itemData == invSlot.ItemData) AddToStack(invSlot.stackSize);// check the same item or not
        else // overide slot with Inventory slot pass in
        {
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }
    public void UpdateInventorySlot(ItemScript item, int amount)// update slot directly
    {
        itemData = item;
        stackSize = amount;
    }
    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)// check room in stack to add
    {
        amountRemaining = ItemData.MaxStackSize - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }
    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (itemData == null || itemData != null && stackSize + amountToAdd <= itemData.MaxStackSize) return true;
        else return false;
    }
    
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }


    public bool SplitStack(out InventorySlot splitStack)
    {
        if (StackSize <= 1)// enought to split ?
        {
            splitStack = null;
            return false;
        }

        int haftStack = Mathf.RoundToInt(StackSize / 2); // get half the stack
        RemoveFromStack(haftStack);

        splitStack = new InventorySlot(itemData, haftStack);// Create copy the slot with half the stack
        return true;
    }
}
