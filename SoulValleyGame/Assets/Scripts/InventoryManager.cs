using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
   
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public int maxStackItem = 4;

    int selectedSlot = -1;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }
    private void Update()
    {
        if (Input.inputString != null)
        {
            //Check inputString is a number
            //  -> if string to number success : true then attack to variable number
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number >0 && number < 10)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue){
        if(selectedSlot > -1)
        {
            inventorySlots[selectedSlot].NotSelect();
        }
        
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool addItem(ItemScript item)
    {
        // If slot had the same item with count lower maxCount
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null 
                && itemInSlot.item == item 
                && itemInSlot.itemCount < maxStackItem
                && itemInSlot.item.stackable == true)
            {
                itemInSlot.itemCount++;
                itemInSlot.Count();
                return true;
            }
        }

        // Find empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                spawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void spawnNewItem(ItemScript item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem invenItem = newItemGameObject.GetComponent<InventoryItem>();
        invenItem.InitializeItem(item);
    }

    public ItemScript GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            ItemScript item =  itemInSlot.item;
            if (use == true && itemInSlot.item.tool == false)
            {
                itemInSlot.itemCount--;
                if (itemInSlot.itemCount < 1)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.Count();
                }
            }
            return item;
        }
        return null;
    }
}
