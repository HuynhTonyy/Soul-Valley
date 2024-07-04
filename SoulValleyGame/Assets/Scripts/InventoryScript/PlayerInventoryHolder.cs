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
    [SerializeField] StaticInventoryDisplay staticInventoryDisplay;
    UIController uIController;
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDynamicPlayerInventoryDisplayRequested;
    PhotonView view;
    SaveGameManager saveGame;
    public Image backGround;
    public PetAI pet;
    public GameObject petControllerUI;

    public void setPrimarySystem(InventorySystem invSys){
        this.primaryInventorySystem = invSys;
        OnPlayerInventoryChanged?.Invoke();
    }
    private void Start(){
        petControllerUI.SetActive(false);
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
            if(Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
                if (saveGame.isEscape)
                {
                    saveGame.close();
                    staticInventoryDisplay.TurnOnHotBarDisplay();
                    staticInventoryDisplay.ToolTip.enabled = true;
                    staticInventoryDisplay.isInAction = false;
                    TurnOffUI();
                }
                else
                {
                    saveGame.open();
                    inventoryUIControler.close();
                    inventoryUIControler.isClosed = true;
                    uIController.close();
                    uIController.isShopClosed = true;
                    staticInventoryDisplay.TurnOffHotBarDisplay();
                    staticInventoryDisplay.isInAction = true;
                    staticInventoryDisplay.ToolTip.enabled = false;
                    if(pet != null && pet.isDisplayUI){
                        TurnOffPetControllerUI();
                    }
                    TurnOnUI();
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
                    staticInventoryDisplay.TurnOnHotBarDisplay();
                    staticInventoryDisplay.isInAction = false;
                    staticInventoryDisplay.ToolTip.enabled = true;
                    TurnOffUI();
                    photonView.RPC("UpdateShopState", RpcTarget.AllBufferedViaServer,
                        this.gameObject.GetComponent<Interactor>().shopKeeper.GetComponent<PhotonView>().ViewID);
                }
                else {
                    if (!inventoryUIControler.isClosed)
                    {   
                        inventoryUIControler.close();
                        inventoryUIControler.isClosed = true;
                        staticInventoryDisplay.TurnOnHotBarDisplay();
                        staticInventoryDisplay.isInAction = false;
                        staticInventoryDisplay.ToolTip.enabled = true;
                        TurnOffUI();
                        checkChest();
                    }else if(pet != null && pet.isDisplayUI){
                        TurnOffPetControllerUI();
                        staticInventoryDisplay.ToolTip.enabled = true;
                    }
                    else 
                    {
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.tabSound, this.transform.position);
                        OnDynamicPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
                        TurnOnUI();
                        staticInventoryDisplay.TurnOnHotBarDisplay();
                        inventoryUIControler.isClosed = false;
                        staticInventoryDisplay.isInAction = true;
                        staticInventoryDisplay.ToolTip.enabled = true;
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
    private void TurnOffUI(){
        backGround.enabled = false;
        gameObject.GetComponent<Interactor>().enabled = true;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        gameObject.GetComponent<PlayerCam>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void TurnOnUI(){
        backGround.enabled = true;
        Interactor interactor = gameObject.GetComponent<Interactor>();
        interactor.enabled = false;
        interactor.cropInfoDisplay.SetActive(false);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<PlayerCam>().enabled = false;
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
    public void TurnOnPetControllerUI(){
        pet.isDisplayUI = true;
        staticInventoryDisplay.TurnOffHotBarDisplay();
        staticInventoryDisplay.isInAction = false;
        TurnOnUI();
        petControllerUI.SetActive(true);
    }
    public void TurnOffPetControllerUI(){
        pet.isDisplayUI = false;
        staticInventoryDisplay.TurnOnHotBarDisplay();
        staticInventoryDisplay.isInAction = false;
        TurnOffUI();
        petControllerUI.SetActive(false);
    }
    public void TellPetToIdle(){
        pet.Idle();
        TurnOffPetControllerUI();
    }
    public void TellPetToFollow(){
        pet.Follow(transform);
        TurnOffPetControllerUI();
    }
    public void TellPetToCollect(){
        pet.Collect();
        TurnOffPetControllerUI();
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
