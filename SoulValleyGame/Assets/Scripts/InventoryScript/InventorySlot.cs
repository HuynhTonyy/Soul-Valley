using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemScript itemData;
    [SerializeField] private int stackSize;

    public ItemScript ItemData => itemData;
    public int StackSize => stackSize;
    public InventorySlot(ItemScript source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }
    public InventorySlot()
    {
        ClearSlot();
    }
    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }
    public void UpdateInventorySlot(ItemScript item, int amount)
    {
        itemData = item;
        stackSize = amount;
    }
    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.MaxStackSize - stackSize;
        return RoomLeftInStack(amountToAdd);
    }
    public bool RoomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData.MaxStackSize) return true;
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
}
