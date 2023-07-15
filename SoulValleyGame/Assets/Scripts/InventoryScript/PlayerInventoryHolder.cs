using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;
[RequireComponent(typeof(UniqueID))]
public class PlayerInventoryHolder : InventoryHolder
{
    private string playerId;
    [SerializeField] private Canvas canvas;
    InventoryUIControler inventoryUIControler;
    UIController uIController;
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDynamicPlayerInventoryDisplayRequested;

    PhotonView view;
    public void setPrimarySystem(InventorySystem invSys){
        this.primaryInventorySystem = invSys;
        OnPlayerInventoryChanged?.Invoke();
    }
    protected override void LoadInventory(SaveData data){}

    private void Start(){
        playerId = GetComponent<UniqueID>().ID;
        view = GetComponent<PhotonView>();
        if(!view.IsMine){
            Destroy(canvas.gameObject);
        }
        inventoryUIControler = GetComponentInChildren<InventoryUIControler>();
        uIController = GetComponentInChildren<UIController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(view.IsMine)
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
