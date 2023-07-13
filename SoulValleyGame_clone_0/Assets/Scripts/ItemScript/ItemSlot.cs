using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : ISerializationCallbackReceiver
{
    [SerializeField] protected ItemScript itemData; // reference to the data
    [SerializeField] protected int stackSize; // Current stack size

    public ItemScript ItemData => itemData;
    public int StackSize => stackSize;

    public void ClearSlot() // clear a slot
    {
        itemData = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot) // assign Item to slot
    {
        if (itemData == invSlot.ItemData) AddToStack(invSlot.StackSize);// check the same item or not
        else // overide slot with Inventory slot pass in
        {
            itemData = invSlot.ItemData;
            stackSize = 0;
            AddToStack(invSlot.StackSize);
        }
    }

    public void AssignItem(ItemScript data, int amount)
    {
        if (itemData == data) AddToStack(amount);
        else
        {
            itemData = data;
            stackSize = 0;
            AddToStack(amount);
        }
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public void OnBeforeSerialize()
    {
         
    }

    public void OnAfterDeserialize()
    {
       
    }



    // Start is called before the first frame update

}
