using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IIntractable, IPunObservable
{
    [SerializeField] private PlaceableData itemData;

    string id;
    PhotonView view;
    public bool isUsed = false;

    List<ItemScript> listOfItem = new List<ItemScript>();
    protected override void Awake()
    {
        base.Awake();
        
    }
    private void Start() {
        id = GetComponent<UniqueID>().ID;
        if(PhotonNetwork.IsMasterClient)    
            SaveLoad.OnSaveData += SaveChest;
        view = GetComponent<PhotonView>();
        listOfItem = getAllItem();
    }
    private void SaveChest()
    {
        if(!SaveGameManager.data.chestDictionary.ContainsKey(id)){
            SaveGameManager.data.chestDictionary.Add(id,new ChestSaveData(primaryInventorySystem,transform.position,transform.rotation, itemData));
        }else{
            SaveGameManager.data.chestDictionary[id] = new ChestSaveData(primaryInventorySystem,transform.position,transform.rotation, itemData);
        }
    }
    public void DestroyChestOnLoad()
    {
        SaveLoad.OnSaveData -= SaveChest;
        view.RPC("DestroyItem", RpcTarget.AllBufferedViaServer);
    }
    public void LoadInventory(ChestSaveData chestData)
    {
        primaryInventorySystem = chestData.InvSystem;
        Debug.Log(primaryInventorySystem.InventorySlots.Count);
        Debug.Log(primaryInventorySystem.InventorySlots[0].StackSize);
    }
    public void Interact(Interactor interactor)
    {
        if(isUsed) return;
        photonView.RPC("chestState",RpcTarget.AllBufferedViaServer);
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0,interactor.gameObject.GetComponent<PlayerInventoryHolder>().PrimaryInventorySystem,9);
        interactor.gameObject.GetComponentInChildren<InventoryUIControler>().isClosed = false;
    }
    public void syncChest()
    {
        listOfItem = getAllItem();
        List<string> listItemID = new List<string>();
        List<int> listItemAmount = new List<int>();
        foreach (InventorySlot item in primaryInventorySystem.InventorySlots){
            Debug.Log(item.ItemData);
            if(item.ItemData){
                listItemID.Add(item.ItemData.Id);
                listItemAmount.Add(item.StackSize);
            }else{
                listItemID.Add(null);
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
                    Debug.Log(item.name);
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
                photonView.RPC("updateChest",RpcTarget.AllBufferedViaServer,inventorySlot.ItemData,inventorySlot.StackSize,i,gameObject.GetComponent<PhotonView>().ViewID);
            }
            else
            {
                photonView.RPC("updateChest",RpcTarget.AllBufferedViaServer,inventorySlot.ItemData.Id,inventorySlot.StackSize,i, gameObject.GetComponent<PhotonView>().ViewID);
            }
        }
    }
    public void DestroyChestByPlayer(){
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
        photonView.RPC("a",RpcTarget.MasterClient,view.ViewID);
        if(PhotonNetwork.IsMasterClient){
            SaveLoad.OnSaveData -= SaveChest;
        }
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
    private void updateChest(string itemId , int itemAmount , int index, int viewID)
    {
        ChestInventory chest = PhotonView.Find(viewID).GetComponent<ChestInventory>();
        bool check = true;
        foreach(ItemScript item in listOfItem)
        {
            if(item.Id == itemId)
            {
                chest.primaryInventorySystem.AssignItemBySlotIndex(item,itemAmount,index);
                check = false;
                break;
            }
        }
        if(check)
        {
            chest.primaryInventorySystem.AssignItemBySlotIndex(null,itemAmount,index);
        }
    }
    [PunRPC]
    private void chestState()
    {
        isUsed = true;
    }
    [PunRPC]
    public void LoadChestPosition(int viewID, float x,float y,float z){
        GameObject chest = PhotonView.Find(viewID).gameObject;
        chest.transform.position = new Vector3(x,y,z);
    }
    [PunRPC]
    public void LoadChestRotation(int viewID, float x,float y,float z , float w){
        GameObject chest = PhotonView.Find(viewID).gameObject;
        chest.transform.rotation = new Quaternion(x,y,z,w);
    }
    [PunRPC]
    public void a(int viewID){
        string id = PhotonView.Find(viewID).GetComponent<UniqueID>().ID;
        SaveGameManager.data.chestDictionary.Remove(id);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        return;
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


