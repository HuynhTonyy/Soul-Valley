using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemScript[] itemToPickup;

    public void pickUpItem(int id)
    {
        bool result = inventoryManager.addItem(itemToPickup[id]);
        if (result == true)
        {
            Debug.Log("Item Added");
        }else { 
            Debug.Log("Inventory Full"); 
        }

    }
    public void UseGetSelectedItem()
    {
        ItemScript recievedItem = inventoryManager.GetSelectedItem(true);
        if(recievedItem != null)
        {
            Debug.Log("Used Item: " + recievedItem);
        }
        else 
        { 
            Debug.Log("No Item used");
        }
    }
}
