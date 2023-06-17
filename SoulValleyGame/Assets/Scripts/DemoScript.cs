using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemScript[] itemToPickup;

    public void pickUpItem(int id)
    {
        inventoryManager.addItem(itemToPickup[id]);
    }
}
