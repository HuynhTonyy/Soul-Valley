using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] LayerMask whatIsGround;
    FarmLand selectedLand;
    RaycastHit hit;
    public GameObject chest;
    PhotonView view;
    public StaticInventoryDisplay hotBar;
    new string tag;
    bool inRange;
    GameObject itemBP = null;
    public GameObject shopKeeper;
    Outline interactGO;
    public House house;
    public bool isInAction = false;

    // Farm land info
    [SerializeField] GameObject cropInfoDisplay;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI landState;
    [SerializeField] TextMeshProUGUI cropState;
    [SerializeField] TextMeshProUGUI time;

    private void Start() {
        view = GetComponent<PhotonView>();
        hotBar = GetComponentInChildren<StaticInventoryDisplay>();
        house = GameObject.FindObjectOfType<House>();
    }
    private void Update()
    {
        
        if(view.IsMine){
            if (isInAction && Keyboard.current.anyKey.isPressed) 
            {
                house.QuitQueue(gameObject);
            }
            // if (Keyboard.current.kKey.wasPressedThisFrame && PhotonNetwork.IsMasterClient){
            //     SaveLoad.Save(SaveGameManager.data);
            // }
            // if (Keyboard.current.lKey.wasPressedThisFrame && PhotonNetwork.IsMasterClient){
            //     SaveLoad.Load();
            // }
            // //Time skip
            // if (Keyboard.current.oKey.isPressed && PhotonNetwork.IsMasterClient){
            //     TimeManager.Instance.Tick();
            // }
            int selectedSlot = hotBar.selectedSlot;
            inRange = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f,whatIsGround,QueryTriggerInteraction.Ignore);
            //Throw item which is selected in hot bar
            if(hotBar.GetSelectedItem(selectedSlot)){
                UseableData useableData = hotBar.GetUseableData(selectedSlot);
                if(GetComponentInChildren<InventoryUIControler>().isClosed && Keyboard.current.qKey.wasPressedThisFrame){
                    hotBar.throwItem(transform,selectedSlot);
                }
                else if(useableData && Mouse.current.leftButton.isPressed){
                    hotBar.throwPokeBall(transform,selectedSlot);
                }
            }
            //In range of sight
            if (inRange){
                tag = hit.transform.tag;
                if(tag == "FarmLand"){
                    FarmLand farmLand = hit.collider.GetComponent<FarmLand>();
                    if (selectedLand){
                        selectedLand.Select(false);
                    }
                    selectedLand = farmLand;
                    farmLand.Select(true);
                    //Display time info
                    if(farmLand.cropPlanted){
                        name.SetText(farmLand.cropPlanted.seedData.CropToYield.DisplayName);
                        int timeRemain = farmLand.cropPlanted.maxGrowth - farmLand.cropPlanted.growth;
                        if(timeRemain <= 0)
                            time.SetText("Harvestable");
                        else
                            time.SetText((timeRemain)+" min");
                        landState.SetText(farmLand.landState.ToString());
                        cropInfoDisplay.SetActive(true);
                    }else
                        cropInfoDisplay.SetActive(false);
                    
                    
                    if(Keyboard.current.eKey.wasReleasedThisFrame && farmLand.cropPlanted && farmLand.cropPlanted.cropState == CropBehaviour.CropState.Harvestable){
                        farmLand.Harvest(this.gameObject);
                    }
                }else if(tag == "Interactable"){
                    if(interactGO){
                        interactGO.enabled = false;
                        interactGO = null;
                    }
                    if(hit.transform.gameObject.TryGetComponent<Outline>(out interactGO)){
                        interactGO.enabled = true;
                    }
                    if(Mouse.current.rightButton.wasPressedThisFrame && GetComponentInChildren<InventoryUIControler>().isClosed){
                        IIntractable interactable;
                        if (hit.collider.TryGetComponent<IIntractable>(out interactable)){
                            interactable.Interact(this);
                            // hotBar.enabled = false;
                            // this.enabled = false;
                        }
                        if(hit.transform.gameObject.GetComponent<ChestInventory>())
                        {
                            chest = hit.transform.gameObject;
                        }
                    }
                }else if(selectedLand){
                    selectedLand.Select(false);
                    cropInfoDisplay.SetActive(false);
                }
                if(interactGO && tag != "Interactable"){
                        interactGO.enabled = false;
                        interactGO = null;
                }
                if(hotBar.GetSelectedItem(selectedSlot)  && gameObject.GetComponentInChildren<InventoryUIControler>().isClosed){
                    SeedData seed = hotBar.GetSeed(selectedSlot);
                    PlaceableData placeable = hotBar.GetPlaceableData(selectedSlot);
                    ToolData tool = hotBar.GetToolData(selectedSlot);
                    if (placeable && tag == "Placeable"){
                        if(!itemBP){
                            itemBP = Instantiate(placeable.itemBP,hit.point,Quaternion.identity);
                        }else{
                            if(placeable.itemBP.name == itemBP.name.Substring(0,itemBP.name.Length-7)) 
                                itemBP.transform.position = hit.point;
                            else
                            {
                                Destroy(itemBP);
                                itemBP = Instantiate(placeable.itemBP,hit.point,Quaternion.identity);
                            }
                            if(Keyboard.current.rKey.wasPressedThisFrame){
                                itemBP.transform.Rotate(new Vector3(0,90,0));
                            }
                        }
                    }else{
                        Destroy(itemBP);
                        itemBP = null;
                    }
                    if(Mouse.current.leftButton.wasPressedThisFrame){
                        if (seed && selectedLand && selectedLand.Plant(seed)){
                            hotBar.UseItem(selectedSlot);
                        }   
                        else if (placeable && tag == "Placeable" && itemBP.GetComponent<Blueprint>().isPlaceable){
                            PhotonNetwork.Instantiate(placeable.itemData.ItemPreFab.name, hit.point,itemBP.transform.rotation);
                            hotBar.UseItem(selectedSlot);
                            Destroy(itemBP);
                            itemBP = null;
                        }
                        else if (tool){
                            switch (tool.toolType){
                                case ToolData.ToolType.WateringCan:
                                    if (tag == "FarmLand" && tool.currentDurability != 0)
                                    {
                                        FarmLand farmLand = hit.transform.GetComponent<FarmLand>();
                                        if (farmLand.Water()){
                                            GameEventManager.instance.inventoryEvent.UseItem(tool.Id,UseQuestStep.UseType.use);
                                            tool.currentDurability--;
                                        }
                                            
                                    }
                                    else if(tag == "FarmLand" && tool.currentDurability == 0)
                                    {
                                        AudioManager.instance.PlayOneShot(FMODEvents.instance.emptyBucketSound, this.transform.position);
                                    }
                                    break;
                                case ToolData.ToolType.Hoe:
                                    if (tag == "FarmLand"){
                                        FarmLand farmLand = hit.transform.GetComponent<FarmLand>();
                                        farmLand.Till();
                                        GameEventManager.instance.inventoryEvent.UseItem(tool.Id,UseQuestStep.UseType.use);
                                    }
                                    break;
                                case ToolData.ToolType.Hammer:
                                    if (tag == "Interactable"){
                                        ChestInventory chestGO = hit.transform.gameObject.GetComponent<ChestInventory>();
                                        if(chestGO)
                                            chestGO.DestroyChestByPlayer();
                                        GameEventManager.instance.inventoryEvent.UseItem(tool.Id,UseQuestStep.UseType.use);
                                    }
                                    break;
                            }
                        }         
                    }
                }else{
                    if(itemBP){
                        Destroy(itemBP);
                        itemBP = null;
                    }
                }
            }
            else{
                if(selectedLand){
                    selectedLand.Select(false);
                    cropInfoDisplay.SetActive(false);
                }
                if(interactGO){
                    interactGO.enabled = false;
                    interactGO = null;
                }
                if(itemBP){
                    Destroy(itemBP);
                    itemBP = null;
                }
            }
        }
    }
}


