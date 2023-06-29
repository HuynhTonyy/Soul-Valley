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

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(itemData, transform.position, transform.rotation);

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void Start()
    {
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void LoadGame(SaveData data)
    {
        Debug.Log(data.collectedItems);
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
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
