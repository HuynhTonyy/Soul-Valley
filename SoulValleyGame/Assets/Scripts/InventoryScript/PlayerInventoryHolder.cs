using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDynamicPlayerInventoryDisplayRequested;

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
            
    }
    protected override void LoadInventory(SaveData data)
    {
        // check the save data for specific chest inventory - if exist load in
        if (data.playerInventory.InvSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryUIControler.isClosed && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OnDynamicPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
            InventoryUIControler.isClosed = false;
        }
        else if(!InventoryUIControler.isClosed && Keyboard.current.tabKey.wasPressedThisFrame)
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
