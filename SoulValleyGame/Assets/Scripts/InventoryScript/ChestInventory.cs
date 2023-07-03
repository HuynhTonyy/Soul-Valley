using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    [SerializeField] private PlaceableData itemData;
    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadChest;
    }

    private void Start()
    {
        var chestSaveData = new ChestSaveData(primaryInventorySystem,transform.position,transform.rotation, itemData);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }
    void LoadChest(SaveData data)
    {
        if(!data.chestDictionary.ContainsKey(GetComponent<UniqueID>().ID)) Destroy(gameObject);
        else
        {
            LoadInventory(data, GetComponent<UniqueID>().ID);
        }
    }
    protected override void LoadInventory(SaveData data)
    {

        // check the save data for specific chest inventory - if exist load in
        /*if(data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            primaryInventorySystem = chestData.InvSystem;
        }*/
    }
    public void LoadInventory(SaveData data, string ID)
    {

        // check the save data for specific chest inventory - if exist load in
        if (data.chestDictionary.TryGetValue(ID, out ChestSaveData chestData))
        {
            primaryInventorySystem = chestData.InvSystem;
        }
    }
    public void LoadInventory(ChestSaveData chestData)
    {
        primaryInventorySystem = chestData.InvSystem;
    }
    public void Interact(Interactor interactor)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0);
        InventoryUIControler.isClosed = false;
    }

    public void Destroy(){
        foreach (InventorySlot slot in primaryInventorySystem.InventorySlots)
        {
            if(slot != null){
                for(int i = 0; i < slot.StackSize; i++)
                {
                    var position = new Vector3(Random.Range(-0.3f, -0.1f), 0, Random.Range(-0.3f, -0.1f));
                    Vector3 _dropOffset = position;
                    Instantiate(slot.ItemData.ItemPreFab, transform.position + _dropOffset, Quaternion.identity);
                }
            }
        }
        Instantiate(itemData.ItemPreFab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }

}
[System.Serializable]
public struct ChestSaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position;
    public Quaternion Rotation;
    public ItemScript ItemData;

    public ChestSaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation, ItemScript _itemData)
    {
        InvSystem = _invSystem;
        Position = _position;
        Rotation = _rotation;
        ItemData = _itemData;
    }


}


