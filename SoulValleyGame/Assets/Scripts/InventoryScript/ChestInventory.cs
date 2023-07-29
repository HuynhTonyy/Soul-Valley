using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IIntractable
{
    [SerializeField] private PlaceableData itemData;

    String id;
    PhotonView view;
    public bool isUsed = false;

    List<ItemScript> listOfItem = new List<ItemScript>();

    public InventorySystem getChestInventory(){ return primaryInventorySystem;}
    protected override void Awake()
    {
        base.Awake();
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnSaveData += SaveChest;
        SaveLoad.OnLoadGame += LoadChest;
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        listOfItem = getAllItem();
        var chestSaveData = new ChestSaveData(primaryInventorySystem,transform.position,transform.rotation, itemData);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }
    private void SaveChest()
    {
        SaveGameManager.data.chestDictionary[id] = new ChestSaveData(primaryInventorySystem,transform.position,transform.rotation, itemData);
    }
    void LoadChest(SaveData data)
    {
        if(SaveGameManager.data.chestDictionary.ContainsKey(id)) 
        {
        //     LoadInventory(data.chestDictionary[id]);
        // }
        // else
        // {
            SaveGameManager.data.chestDictionary.Remove(id);
        }
        SaveLoad.OnSaveData -= SaveChest;
        SaveLoad.OnLoadGame -= LoadChest;
        Destroy(gameObject);
}
    protected override void LoadInventory(SaveData data)
    {
    }
    
    public void LoadInventory(ChestSaveData chestData)
    {
        primaryInventorySystem = chestData.InvSystem;
    }
    public void Interact(Interactor interactor)
    {
        if(isUsed)
        {
            return;
        }
        isUsed = true;

        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0,interactor.gameObject.GetComponent<PlayerInventoryHolder>().PrimaryInventorySystem,9);
        interactor.gameObject.GetComponentInChildren<InventoryUIControler>().isClosed = false;
 
    }

    public void syncChest()
    {
        List<string> listItemID = new List<string>();
        foreach (InventorySlot item in primaryInventorySystem.InventorySlots)
        {
            if(item.ItemData)
            {
                listItemID.Add(item.ItemData.Id);
            }
            else
            {
                listItemID.Add(null);
            }
        }
        List<int> listItemAmount = new List<int>();
        foreach (InventorySlot item in primaryInventorySystem.InventorySlots)
        {
            if(item.ItemData)
            {
                listItemAmount.Add(item.StackSize);
            }
            else
            {
                listItemAmount.Add(-1);
            }
        }
        
        for (int i = 0 ; i < listItemID.Count;i++)
        {
            InventorySlot inventorySlot = new InventorySlot();
            bool check = true;
            
            foreach(ItemScript item in listOfItem)
            {
                if(item.Id == listItemID[i])
                {
                    inventorySlot.setItemData(item);
                    inventorySlot.setItemStack(listItemAmount[i]);
                    check = false;
                    break;
                }
            }
            if(check)
            {
                inventorySlot.setItemData(null);
                inventorySlot.setItemStack(-1);
            }
            photonView.RPC("updateChest",RpcTarget.AllBufferedViaServer,inventorySlot.ItemData.Id,inventorySlot.StackSize,i);
        }
    }
    public void DestroyChest(){
        Vector3 _dropOffset = new Vector3(Random.Range(-0.3f, -0.1f), .5f, Random.Range(-0.3f, -0.1f));
        foreach (InventorySlot slot in primaryInventorySystem.InventorySlots)
        {
            if(slot != null){
                for(int i = 0; i < slot.StackSize; i++)
                {
                    _dropOffset = new Vector3(Random.Range(-0.3f, -0.1f), .5f, Random.Range(-0.3f, -0.1f));
                    PhotonNetwork.Instantiate(slot.ItemData.ItemPreFab.name, transform.position + _dropOffset, Quaternion.identity);
                }
            }
        }
        PhotonNetwork.Instantiate(itemData.ItemPreFab.name, transform.position + _dropOffset, Quaternion.identity);
        SaveGameManager.data.chestDictionary.Remove(id);
        SaveLoad.OnSaveData -= SaveChest;
        SaveLoad.OnLoadGame -= LoadChest;
        view.RPC("DestroyItem", RpcTarget.AllBufferedViaServer);
    }
    [PunRPC]
    private void DestroyItem()
    {
        Destroy(gameObject);
    }

    List<ItemScript> getAllItem()
    {
        ItemScript[] allItem = Resources.LoadAll<ItemScript>("Items"); 
        List<ItemScript> listItem = new List<ItemScript>();
        foreach(ItemScript item in allItem)
        {
            listItem.Add(item);
        }
        return listItem;
    }

    [PunRPC]
    private void updateChest(string itemId , int itemAmount , int index)
    {
        bool check = true;
        foreach(ItemScript item in listOfItem)
        {

            if(item.Id == itemId)
            {
                primaryInventorySystem.AssignItemBySlotIndex(item,itemAmount,index);
                check = false;
                break;
            }
        }
        if(check)
        {
            primaryInventorySystem.AssignItemBySlotIndex(null,itemAmount,index);
        }
    }
}

[System.Serializable]
public struct ChestSaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position;
    public Quaternion Rotation;
    public PlaceableData ItemData;

    public ChestSaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation, PlaceableData _itemData)
    {
        InvSystem = _invSystem;
        Position = _position;
        Rotation = _rotation;
        ItemData = _itemData;
    }


}


