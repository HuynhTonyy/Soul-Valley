using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class StaticInventoryDisplay : InventoryDisplay
{
    public TextMeshProUGUI ToolTip;
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;
    
    public InventorySlot InventorySlot;
    public int selectedSlot=0;
    public bool isInAction = false;
    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
        ChangedSelectedSlot(0);
    }
    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }
    public void throwItem(Transform transform,int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        bool isShiftPress = Keyboard.current.leftShiftKey.isPressed;
        Vector3 positionToSpawn = new Vector3(transform.position.x,transform.position.y+.25f,transform.position.z) + transform.forward * 1f;
        if (selectedSlotData != null && selectedSlotData.ItemData != null){
            GameObject itemGameObject = selectedSlotData.ItemData.ItemPreFab;
            if (isShiftPress){
                for(int i = 0; i < selectedSlotData.StackSize; i++)
                {
                    Vector3 _dropOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
                    GameObject newObject = PhotonNetwork.Instantiate(itemGameObject.name, positionToSpawn + _dropOffset, Quaternion.identity);
                    newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);
                }
                selectedSlotData.ClearSlot();    
            }
            else
            {
                if (selectedSlotData.GetCurrentStackSize() > 1)
                {
                    selectedSlotData.RemoveFromStack(1);
                    selectedUISlot.UpdateUISlot(selectedSlotData);
                    GameObject newObject = PhotonNetwork.Instantiate(itemGameObject.name, positionToSpawn, Quaternion.identity);
                    newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);
                    
                }
                else
                {
                    GameObject newObject = PhotonNetwork.Instantiate(itemGameObject.name, positionToSpawn, Quaternion.identity);
                    newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);
                    selectedSlotData.ClearSlot();
                }
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }
    public void throwPokeBall(Transform transform,int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        Vector3 positionToSpawn = new Vector3(transform.position.x,transform.position.y+.25f,transform.position.z) + transform.forward * 1f;
        if (selectedSlotData != null && selectedSlotData.ItemData != null){
            UseableData useableData = selectedSlotData.ItemData as UseableData;
            GameObject itemGameObject = useableData.pokeball;
            if (selectedSlotData.GetCurrentStackSize() > 1)
            {
                selectedSlotData.RemoveFromStack(1);
                GameObject newObject = PhotonNetwork.Instantiate(itemGameObject.name, positionToSpawn, Quaternion.identity);
                newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);
                
            }
            else
            {
                GameObject newObject = PhotonNetwork.Instantiate(itemGameObject.name, positionToSpawn, Quaternion.identity);
                newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);
                selectedSlotData.ClearSlot();
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
        else
        {
            Debug.Log("No item in the selected slot.");
        }
    }

    public static void mouseThrow(Transform transform, InventorySlot slot)
    {
        Vector3 positionToSpawn = new Vector3(transform.position.x,transform.position.y+.25f,transform.position.z) + transform.forward * 1f;
        for(int i = 0; i < slot.StackSize; i++)
        {
            Vector3 _dropOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            GameObject newObject = PhotonNetwork.Instantiate(slot.ItemData.ItemPreFab.name, positionToSpawn + _dropOffset, Quaternion.identity);
            newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5,ForceMode.Impulse);
        }
        slot.ClearSlot();    
    }

    public void UseItem(int selectedSlot)
    {
        InventorySlot_UI selectedUISlot = slots[selectedSlot];
        InventorySlot selectedSlotData = selectedUISlot.AssignInventorySlot;
        if (selectedSlotData != null && selectedSlotData.ItemData != null)
        {
            ItemScript itemData = selectedSlotData.ItemData;
            if (selectedSlotData.GetCurrentStackSize() > 1)
            {
                selectedSlotData.RemoveFromStack(1);
            }
            else
            {
                selectedSlotData.ClearSlot();
            }
            selectedUISlot.UpdateUISlot(selectedSlotData);
        }
    }
    public bool GetSelectedItem(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        InventorySlot slot = inventorySystem.GetSlot(selectedSlot);
        if (slot != null && slot.ItemData != null)
        {
            if (slot.ItemData.DisplayName == "WateringCan")
            {
                ToolData slotItem = slot.ItemData as ToolData;
                string durability = slotItem.currentDurability + "/" + slotItem.maxDurability;
                ToolTip.SetText(slot.ItemData.DisplayName + " ( " + durability + " )");
            }
            else
            {
                ToolTip.SetText(slot.ItemData.DisplayName);
            }
            return true;
        }
        else
        {
            ToolTip.SetText("");
            return false;
        }
    }
    public SeedData GetSeed(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        SeedData slot = inventorySystem.GetSlot(selectedSlot).ItemData as SeedData;
        return slot;
    }
    public PlaceableData GetPlaceableData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        PlaceableData data = inventorySystem.GetSlot(selectedSlot).ItemData as PlaceableData;
        return data;
    }
    public ToolData GetToolData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        ToolData data = inventorySystem.GetSlot(selectedSlot).ItemData as ToolData;
        return data;
    }
    public UseableData GetUseableData(int selectedSlot)
    {
        InventorySystem inventorySystem = inventoryHolder.PrimaryInventorySystem;
        UseableData data = inventorySystem.GetSlot(selectedSlot).ItemData as UseableData;
        return data;
    }
    public void ChangedSelectedSlot(int value)
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
    private void Update() {
        if(isInAction)return;
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
    public void TurnOffHotBarDisplay(){
        gameObject.GetComponent<Image>().enabled = false;
        foreach(InventorySlot_UI inventorySlot in slots){
            inventorySlot.gameObject.GetComponent<Image>().enabled = false;
            inventorySlot.itemSprite.enabled = false;
            inventorySlot.itemCount.enabled = false;
        }
    }
    public void TurnOnHotBarDisplay(){
        gameObject.GetComponent<Image>().enabled = true;
        foreach(InventorySlot_UI inventorySlot in slots){
            inventorySlot.gameObject.GetComponent<Image>().enabled = true;
            inventorySlot.itemSprite.enabled = true;
            inventorySlot.itemCount.enabled = true;
        }
        RefreshStaticDisplay();
    }
}
