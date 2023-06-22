using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;
    protected override void Start()
    {
        base.Start();
        if(inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSLot;
        }
        else
        {
            Debug.Log("No inventory assign to: " + this.gameObject);
        }
        AssignSlot(inventorySystem);
    }
    public override void AssignSlot(InventorySystem invDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();
        if (slots.Length != inventorySystem.InventorySize) Debug.Log("Inventory slot out of on " + this.gameObject);
        for(int i =0; i< inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }

}
