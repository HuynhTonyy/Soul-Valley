using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] LayerMask whatIsGround;
    FarmLand selectedLand;
    RaycastHit hit;
    public GameObject chest;
    PhotonView view;
    StaticInventoryDisplay hotBar;
    new string tag;
    bool inRange;
    GameObject itemBP = null;
    public GameObject shopKeeper;
    private void Start() {
        view = GetComponent<PhotonView>();
        hotBar = GetComponentInChildren<StaticInventoryDisplay>();
    }
    private void Update()
    {
        if(view.IsMine){
            if (Keyboard.current.kKey.wasPressedThisFrame && PhotonNetwork.IsMasterClient){
                SaveLoad.Save(SaveGameManager.data);
            }
            if (Keyboard.current.lKey.wasPressedThisFrame && PhotonNetwork.IsMasterClient){
                SaveLoad.Load();
            }
            //Time skip
            if (Keyboard.current.oKey.isPressed && PhotonNetwork.IsMasterClient){
                TimeManager.Instance.Tick();
            }
            int selectedSlot = hotBar.selectedSlot;
            inRange = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f,whatIsGround,QueryTriggerInteraction.Ignore);
            //Throw item which is selected in hot bar
            if(hotBar.GetSelectedItem(selectedSlot)  && GetComponentInChildren<InventoryUIControler>().isClosed && Keyboard.current.qKey.wasPressedThisFrame){
                hotBar.throwItem(transform,selectedSlot);
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
                    if(Keyboard.current.eKey.wasReleasedThisFrame && farmLand.cropPlanted && farmLand.cropPlanted.cropState == CropBehaviour.CropState.Harvestable){
                        farmLand.Harvest(this.gameObject);
                    }
                }else if(tag == "Interactable" && Mouse.current.rightButton.wasPressedThisFrame && GetComponentInChildren<InventoryUIControler>().isClosed){
                    IIntractable interactable = hit.collider.GetComponent<IIntractable>();
                    if (interactable != null) interactable.Interact(this);
                    chest = hit.transform.gameObject;
                }else if(selectedLand){
                    selectedLand.Select(false);
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
                                    if (tag == "FarmLand"){
                                        FarmLand farmLand = hit.transform.GetComponent<FarmLand>();
                                        farmLand.Water();
                                    }
                                    break;
                                case ToolData.ToolType.Hoe:
                                    if (tag == "FarmLand"){
                                        FarmLand farmLand = hit.transform.GetComponent<FarmLand>();
                                        farmLand.Till();
                                    }
                                    break;
                                case ToolData.ToolType.Hammer:
                                    if (tag == "Interactable"){
                                        ChestInventory chestGO = hit.transform.gameObject.GetComponent<ChestInventory>();
                                        if(chestGO)
                                            chestGO.DestroyChest();
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
                }
                if(itemBP){
                    Destroy(itemBP);
                    itemBP = null;
                }
            }
        }
    }
}


