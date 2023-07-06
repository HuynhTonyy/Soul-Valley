using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDynamicPlayerInventoryDisplayRequested;

    public void setPrimarySystem(InventorySystem invSys){
        this.primaryInventorySystem = invSys;
        OnPlayerInventoryChanged?.Invoke();
    }
    protected override void LoadInventory(SaveData data)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.isShopClosed && InventoryUIControler.isClosed && Keyboard.current.tabKey.wasPressedThisFrame)
        {

        }
        else if (UIController.isShopClosed && InventoryUIControler.isClosed && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OnDynamicPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
            InventoryUIControler.isClosed = false;    
        }
        else if(UIController.isShopClosed && !InventoryUIControler.isClosed && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            
            InventoryUIControler.isClosed = true;
        }
        
            
    }
    public bool AddToInventory(ItemScript item, int amount)
    {
        if (primaryInventorySystem.AddToInventory(item, amount))
        {
            return true;
        }
        return false;
    }
}
