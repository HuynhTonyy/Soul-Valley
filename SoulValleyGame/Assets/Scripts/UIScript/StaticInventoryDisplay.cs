using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;

    int selectedSlot=-1;
    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
        ChangedSelectedSlot(0);
        GetSelectedItem(selectedSlot);
    }
    private void GetSelectedItem(int selectedSlot)
    {
        InventorySlot slot = slots[selectedSlot];
        ItemScript itemInSlot = slots[selectedSlot].GetComponent<InventorySlot>();
        Debug.Log("use " + itemInSlot);
        if (slots[selectedSlot] != null)
        {
            var itemSelected = slots[selectedSlot].GetComponent<ItemScript>();
            Debug.Log("use "+itemSelected.DisplayName);
        }
        
    }
    private void Update()
    {
        float scrollValue = Input.mouseScrollDelta.y;

        if (selectedSlot > -1 && selectedSlot < 9)
        {
            if (scrollValue > 0 && selectedSlot < 8)
            {
                Debug.Log("cuon len");
                ChangedSelectedSlot(selectedSlot + 1);
            }
            else if (scrollValue < 0 && selectedSlot > 0)
            {
                Debug.Log("cuon xuong");
                ChangedSelectedSlot(selectedSlot - 1);
            }
        }
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number >0 && number< 10)
            {
                Debug.Log("Phim "+number);
                ChangedSelectedSlot(number - 1);
            }
        }
    }

    private void ChangedSelectedSlot(int value)
    {
        if (selectedSlot >= 0)
        {
            slots[selectedSlot].Deselected();
        }
        slots[value].Selected();
        selectedSlot = value;
    }
  
    public void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
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
