using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData
{
    public List<string> collectedItems;

    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;

    public SerializableDictionary<string, InventorySaveData> chestDictionary;
    
    public InventorySaveData playerInventory;

    public PlayerData playerData ;

    public SaveData()
    {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        playerInventory = new InventorySaveData();
        playerData = new PlayerData();
    }
}
