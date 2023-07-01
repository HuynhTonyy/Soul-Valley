using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;

    public float PickUpRadius = 1f;
    public ItemScript itemData;

    private SphereCollider myCollider;

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
        myCollider.center = new Vector3(0, 0, 0);
        minPositionY = transform.position.y;
        maxPositionY = transform.position.y + 0.1f;
        
    }

    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
        
    }

    private void LoadGame(SaveData data)
    {
        Debug.Log(data.collectedItems);
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
        else 
        {

            foreach(var activeKey in data.activeItems)
            {
                string key = activeKey.Key;
                int instanceID = activeKey.Value.itemData.GetInstanceID();
                Vector3 position = new Vector3(activeKey.Value.position.x, activeKey.Value.position.y, activeKey.Value.position.z);
                // get itemPrefab throw = instanceID

              
                Instantiate(itemData.ItemPreFab, position , Quaternion.identity);
            }
            
        }

        /*foreach (var activeKey in data.activeItems.Keys)
        {
            foreach (var collectKey in data.collectedItems)
            {
                if (activeKey != collectKey)
                {
                    Instantiate(itemData.GetInstanceID,, Quaternion.identity);
                }
            }
                
        }*/
        
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }   

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;

        if (inventory.AddToInventory(itemData,1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
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
