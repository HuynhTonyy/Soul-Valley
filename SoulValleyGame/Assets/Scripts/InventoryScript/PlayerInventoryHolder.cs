using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(UniqueID))]
public class PlayerInventoryHolder : InventoryHolder
{
    private string playerId;
    [SerializeField] private Canvas canvas;
    InventoryUIControler inventoryUIControler;
    MouseItemData mouse;
    // [SerializeField] StaticInventoryDisplay staticInventoryDisplay;
    UIController uIController;
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDynamicPlayerInventoryDisplayRequested;
    PhotonView view;
    SaveGameManager saveGame;
    public Image backGround;

    public void setPrimarySystem(InventorySystem invSys){
        this.primaryInventorySystem = invSys;
        OnPlayerInventoryChanged?.Invoke();
    }
    private void Start(){
        playerId = GetComponent<UniqueID>().ID;
        view = GetComponent<PhotonView>();
        mouse = GetComponentInChildren<MouseItemData>();
        if (!view.IsMine)
        {
            Destroy(canvas.gameObject);
        }
        inventoryUIControler = GetComponentInChildren<InventoryUIControler>();
        uIController = GetComponentInChildren<UIController>();
        saveGame = GetComponentInChildren<SaveGameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(view.IsMine)
        {
            if(Keyboard.current.capsLockKey.wasPressedThisFrame)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
                if (saveGame.isEscape)
                {
                    saveGame.close();
                    enable();
                    backGround.enabled=false;
                }
                else
                {
                    saveGame.open();
                    inventoryUIControler.close();
                    inventoryUIControler.isClosed = true;
                    uIController.close();
                    uIController.isShopClosed = true;
                    disable();
                    backGround.enabled=true;
                    checkChest();
                    if(gameObject.GetComponent<Interactor>().shopKeeper){
                        gameObject.GetComponent<Interactor>().shopKeeper.GetComponent<ShopKeeper>().isInAction = false;
                    }
                }
            }
            if(!saveGame.isEscape && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                if(!uIController.isShopClosed)
                {
                    uIController.close();
                    uIController.isShopClosed = true;
                    // staticInventoryDisplay.gameObject.SetActive(true);
                    enable();
                    photonView.RPC("UpdateShopState", RpcTarget.AllBufferedViaServer,
                        this.gameObject.GetComponent<Interactor>().shopKeeper.GetComponent<PhotonView>().ViewID);
                }
                else {
                    if (!inventoryUIControler.isClosed)
                    {   
                        inventoryUIControler.close();
                        inventoryUIControler.isClosed = true;
                        enable();
                        checkChest();
                    }
                    else 
                    {
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.tabSound, this.transform.position);
                        OnDynamicPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
                        disable();
                        inventoryUIControler.isClosed = false;
                    }  
                }
                if (mouse.AssignInventorySlot.ItemData != null)
                {
                    StaticInventoryDisplay.mouseThrow(this.transform, mouse.AssignInventorySlot);
                    mouse.ClearSlot();
                }
            }
        }
    }
    private void checkChest(){
        GameObject chest = this.gameObject.GetComponent<Interactor>().chest;
        if(chest)
        {
            this.gameObject.GetComponent<Interactor>().chest = null;
            photonView.RPC("UpdateChest", RpcTarget.AllBufferedViaServer,chest.GetComponent<PhotonView>().ViewID);
            chest.GetComponent<ChestInventory>().syncChest();
            chest.GetComponent<ChestInventory>().animator.SetTrigger("Close");
        }  
    }
    private void enable(){
        // staticInventoryDisplay.gameObject.SetActive(true);
        gameObject.GetComponent<Interactor>().enabled = true;
    }
    private void disable(){
        // staticInventoryDisplay.gameObject.SetActive(false);
        gameObject.GetComponent<Interactor>().enabled = false;
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
    void RemoveFromInventory(ItemScript item, int amount)
    {
        foreach (InventorySlot inventorySlot in primaryInventorySystem.InventorySlots)
        {
            if(inventorySlot.ItemData == item){
                if(inventorySlot.StackSize == amount){
                    inventorySlot.ClearSlot();
                    primaryInventorySystem.OnInventorySlotChanged?.Invoke(inventorySlot);
                    break;
                }else if(inventorySlot.StackSize > amount){
                    inventorySlot.RemoveFromStack(amount);
                    break;
                }
            }
        }
    }

    private void OnEnable() {
        GameEventManager.instance.inventoryEvent.onRemoveItem += RemoveFromInventory;
    }
    private void OnDisable() {
        GameEventManager.instance.inventoryEvent.onRemoveItem -= RemoveFromInventory;
    }

    [PunRPC]
    public void UpdateShopState(int npcID)
    {
        PhotonView.Find(npcID).GetComponent<ShopKeeper>().isInAction = false;
    }

    [PunRPC]
    public void UpdateChest(int chestId)
    {
        PhotonView.Find(chestId).GetComponent<ChestInventory>().isUsed = false;
    }


}
