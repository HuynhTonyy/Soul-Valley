using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI slotPrefab;

    protected override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        
    }
    public void RefreshDynamicInventory(InventorySystem invToDisplay)
    {
        ClearSlot();
        inventorySystem = invToDisplay;
        if(inventorySystem != null)inventorySystem.OnInventorySlotChanged += UpdateSLot;
        AssignSlot(invToDisplay);
    }


    public override void AssignSlot(InventorySystem invDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();
        if (invDisplay == null) return;
        
        for(int i = 0; i < invDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invDisplay.InventorySlots[i]);
            uiSlot.Init(invDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }
    private void ClearSlot()
    {
        foreach( var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        if (slotDictionary != null) slotDictionary.Clear();
    }
    private void OnDisable()
    {
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSLot;
    }
}

