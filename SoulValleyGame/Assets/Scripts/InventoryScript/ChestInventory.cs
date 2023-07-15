using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    [SerializeField] private PlaceableData itemData;

    String id;
    PhotonView view;
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

        // check the save data for specific chest inventory - if exist load in
        /*if(data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            primaryInventorySystem = chestData.InvSystem;
        }*/
    }
    
    public void LoadInventory(ChestSaveData chestData)
    {
        primaryInventorySystem = chestData.InvSystem;
    }
    public void Interact(Interactor interactor)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0);
        interactor.gameObject.GetComponentInChildren<InventoryUIControler>().isClosed = false;
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
        PhotonNetwork.Destroy(gameObject);
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


