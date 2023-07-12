using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviourPun
{
    [SerializeField] private float rotationSpeed = 20f;

    public float PickUpRadius = 1f;
    public ItemScript itemData;

    private SphereCollider myCollider;

    PhotonView view;

    [SerializeField] private ItemPickUpSaveData itemSaveData;


    private string id;
    float minPositionY;
    float maxPositionY;
    bool isMax = false;

    private void Awake()
    {
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(itemData, transform.position, transform.rotation);

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
        myCollider.center = new Vector3(0, PickUpRadius/2, 0);
        minPositionY = transform.position.y;
        maxPositionY = transform.position.y + 0.1f;
        
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
        id = GetComponent<UniqueID>().ID;
        if(SaveGameManager.data.activeItems.ContainsKey(id))
        {
            SaveGameManager.data.activeItems[id] = itemSaveData;
        }
        else 
        {
            SaveGameManager.data.activeItems.Add(id,itemSaveData);
        }
    }

    private void LoadGame(SaveData data)
    {
        SaveLoad.OnLoadGame -= LoadGame;
        SaveGameManager.data.activeItems.Remove(id);
        Destroy(gameObject);
    }  

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;

        if (inventory.AddToInventory(itemData,1))
        {
            Debug.Log(itemData.icon);
            SaveGameManager.data.collectedItems.Add(id);
            SaveGameManager.data.activeItems.Remove(id);
            SaveLoad.OnLoadGame -= LoadGame;
            //Destroy(this.gameObject);
            //PhotonNetwork.Destroy(this.gameObject);
            // Transfer ownership to the current player              
            photonView.RequestOwnership();
            if (PhotonNetwork.IsMasterClient || photonView.IsMine)
            {
                photonView.RPC("DestroyItem", RpcTarget.All);
            }            
        }
    }
    
    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (!isMax)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, maxPositionY, transform.position.z), 0.001f);
            if (transform.position.y >= maxPositionY)
            {
                isMax = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, minPositionY, transform.position.z), 0.001f);
            if (transform.position.y <= minPositionY)
            {
                isMax = false;
            }
        }

    }
    [PunRPC]
    private void DestroyItem()
    {
        if (PhotonNetwork.IsMasterClient || photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}


[System.Serializable]
public struct ItemPickUpSaveData
{
    public ItemScript itemData;
    public Vector3 position;
    public Quaternion rotation;

    public ItemPickUpSaveData(ItemScript _itemData, Vector3 _position, Quaternion _rotation)
    {
        itemData = _itemData;
        position = _position;
        rotation = _rotation;
    }
}
