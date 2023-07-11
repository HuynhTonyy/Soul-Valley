using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform cam;
    FarmLand selectedLand;
    RaycastHit hit;
    StaticInventoryDisplay hotBar;
    new string tag;
    bool inRange;
    private void Start() {
        hotBar = GameObject.FindAnyObjectByType<StaticInventoryDisplay>();
    }
    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame){
            SaveLoad.Save(SaveGameManager.data);
        }
        if (Keyboard.current.lKey.wasPressedThisFrame){
            SaveLoad.Load();
        }
        //Time skip
        if (Keyboard.current.oKey.wasPressedThisFrame){
            TimeManager.Instance.Tick();
        }
        int selectedSlot = hotBar.selectedSlot;
        //Throw item which is selected in hot bar
        if(hotBar.GetSelectedItem(selectedSlot)  && gameObject.GetComponentInChildren<InventoryUIControler>().isClosed && Keyboard.current.qKey.wasPressedThisFrame){
            hotBar.throwItem(transform,selectedSlot);
        }
        //In range of sight
        inRange = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f);
        if (inRange)
        {
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
            }else if(tag == "Interactable" && Mouse.current.rightButton.wasPressedThisFrame &&gameObject.GetComponentInChildren<InventoryUIControler>().isClosed){
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null) interactable.Interact(this);
            }else if(selectedLand){
                selectedLand.Select(false);
            }
            if(hotBar.GetSelectedItem(selectedSlot)  && gameObject.GetComponentInChildren<InventoryUIControler>().isClosed && Mouse.current.leftButton.wasPressedThisFrame){
                SeedData seed = hotBar.GetSeed(selectedSlot);
                PlaceableData placeable = hotBar.GetPlaceableData(selectedSlot);
                ToolData tool = hotBar.GetToolData(selectedSlot);
                if (seed && selectedLand && selectedLand.Plant(seed)){
                    hotBar.UseItem(selectedSlot);
                }   
                else if (placeable && tag == "Placeable" ){
                    GameObject spawn = Instantiate(placeable.itemData.ItemPreFab, hit.point,Quaternion.identity);
                    hotBar.UseItem(selectedSlot);
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
                                chestGO.Destroy();
                            }
                            break;
                    }
                }         
            }
        }else if(selectedLand){
            selectedLand.Select(false);
        }
        
    }
}


