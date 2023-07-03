using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;
    public InventorySlot InventorySlot;
    private Transform playerTransform;
    int selectedSlot=-1;
    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
        ChangedSelectedSlot(0);
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }
    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }
    private void throwItem(int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        bool isShiftPress = Keyboard.current.leftShiftKey.isPressed;
        Vector3 positionToSpawn = playerTransform.position + playerTransform.forward * 1f;

        if (selectedSlotData != null && selectedSlotData.ItemData != null)
        {
            ItemScript itemData = selectedSlotData.ItemData;
            GameObject itemGameObject = itemData.ItemPreFab;
            if (isShiftPress){
                for(int i = 0; i < selectedSlotData.StackSize; i++)
                {
                    var position = new Vector3(Random.Range(-0.3f, -0.1f), 0, Random.Range(-0.3f, -0.1f));
                    Vector3 _dropOffset = position;
                    Instantiate(itemGameObject, positionToSpawn + _dropOffset, Quaternion.identity);  
                }
                selectedSlotData.ClearSlot();    
            }
            else
            {
                if (selectedSlotData.StackSize > 1)
                {
                    selectedSlotData.RemoveFromStack(1);
                    selectedUISlot.UpdateUISlot(selectedSlotData);
                    Instantiate(itemGameObject, positionToSpawn, Quaternion.identity);
                }
                else
                {
                    selectedSlotData.ClearSlot();
                    Instantiate(itemData.ItemPreFab, positionToSpawn, Quaternion.identity);
                }
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }
    private void UseItem(int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;

        if (selectedSlotData != null && selectedSlotData.ItemData != null)
        {
            ItemScript itemData = selectedSlotData.ItemData;
            if (itemData.MaxStackSize > 1)
            {
                selectedSlotData.RemoveFromStack(1);
            }
            else
            {
                selectedSlotData.ClearSlot();
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }
    private bool GetSelectedItem(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        InventorySlot slot = inventorySystem.GetSlot(selectedSlot);
       

        if (slot != null && slot.ItemData != null)
        {
            return true;
        }
        else
        {
            //Debug.Log("No item in the specified slot.");
            return false;
        }
    }
    private SeedData GetSeed(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        SeedData slot = inventorySystem.GetSlot(selectedSlot).ItemData as SeedData;
        return slot;
    }
    private PlaceableData GetPlaceableData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        PlaceableData data = inventorySystem.GetSlot(selectedSlot).ItemData as PlaceableData;
        return data;
    }
    private ToolData GetToolData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        ToolData data = inventorySystem.GetSlot(selectedSlot).ItemData as ToolData;
        return data;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && GetSelectedItem(selectedSlot))
        {
            throwItem(selectedSlot);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && GetSelectedItem(selectedSlot))
        {
            if (Interactor.inRange)
            {
                SeedData seed = GetSeed(selectedSlot);
                PlaceableData placeable = GetPlaceableData(selectedSlot);
                ToolData tool = GetToolData(selectedSlot);
                if (seed != null)
                {
                    if (Interactor.selectedLand != null && Interactor.selectedLand.Plant(seed))
                    {
                        UseItem(selectedSlot);
                    }
                }
                else if (placeable != null && Interactor.hit.transform.tag == "Placeable")
                {
                    Debug.Log("Place");
                    Instantiate(placeable.itemData.ItemPreFab, (playerTransform.position) + playerTransform.forward * 1f, Quaternion.identity);
                    UseItem(selectedSlot);
                }
                else if (tool != null)
                {
                    switch (tool.toolType)
                    {
                        case ToolData.ToolType.WateringCan:
                            if (Interactor.inRange && Interactor.hit.transform.tag == "FarmLand")
                            {
                                FarmLand farmLand = Interactor.hit.transform.GetComponent<FarmLand>();
                                if (farmLand.landStatus == FarmLand.LandStatus.Dry && farmLand.cropPlanted != null && farmLand.cropPlanted.cropState != CropBehaviour.CropState.Harvestable)
                                {
                                    farmLand.Water();
                                    Debug.Log("Watered");
                                }
                            }
                            break;
                    }
                }
            }          
        }

        float scrollValue = Input.mouseScrollDelta.y;

        if (selectedSlot > -1 && selectedSlot < 9)
        {
            if (scrollValue < 0 && selectedSlot < 8)
            {
                //Debug.Log("cuon len");
                ChangedSelectedSlot(selectedSlot + 1);
                GetSelectedItem(selectedSlot);
            }
            else if (scrollValue > 0 && selectedSlot > 0)
            {
                //Debug.Log("cuon xuong");
                ChangedSelectedSlot(selectedSlot - 1);
                GetSelectedItem(selectedSlot);
            }
        }
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number >0 && number< 10)
            {
                //Debug.Log("Phim "+number);
                ChangedSelectedSlot(number - 1);
                GetSelectedItem(selectedSlot);
            }
        }

    }
    private void ChangedSelectedSlot(int value)
    {
        if (selectedSlot >= 0)
        {
            slots[selectedSlot].Deselected();
        }
        slots[value].Selected();
        selectedSlot = value;
    }
  
    public void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSLot;
        }
        else
        {
           // Debug.Log("No inventory assign to: " + this.gameObject);
        }
        AssignSlot(inventorySystem,0);
    }
    public override void AssignSlot(InventorySystem invDisplay,int offset) 
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for(int i = 0; i < inventoryHolder.Offset; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
   

}
