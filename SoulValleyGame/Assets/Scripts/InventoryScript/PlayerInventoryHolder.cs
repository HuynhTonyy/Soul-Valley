using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    InventoryUIControler inventoryUIControler;
    UIController uIController;
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDynamicPlayerInventoryDisplayRequested;

    public void setPrimarySystem(InventorySystem invSys){
        this.primaryInventorySystem = invSys;
        OnPlayerInventoryChanged?.Invoke();
    }
    protected override void LoadInventory(SaveData data){}

    private void Start(){
        inventoryUIControler = GetComponentInChildren<InventoryUIControler>();
        uIController = GetComponentInChildren<UIController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if(!uIController.isShopClosed)
            {
                uIController.close();
                uIController.isShopClosed = true;
            }
            else {
                if (!inventoryUIControler.isClosed)
                {   
                    inventoryUIControler.isClosed = true;
                    inventoryUIControler.close();
                }
                else 
                {
                    OnDynamicPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
                    inventoryUIControler.isClosed = false;
                }  
            }
            
        }
       
    }

    public void InvokeInventory()
    {
        OnDynamicPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
