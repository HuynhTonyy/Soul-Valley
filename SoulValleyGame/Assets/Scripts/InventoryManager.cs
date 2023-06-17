using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public void addItem(ItemScript item)
    {
        // Find empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                spawnNewItem(item, slot);
                return;
            }
        }
    }
    void spawnNewItem(ItemScript item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem invenItem = newItemGameObject.GetComponent<InventoryItem>();
        invenItem.InitializeItem(item);
    }
}
