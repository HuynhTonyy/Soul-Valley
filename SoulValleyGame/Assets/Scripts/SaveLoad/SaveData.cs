using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData
{
    public List<string> collectedItems;

    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;

    public SerializableDictionary<string, ChestSaveData> chestDictionary;

    public SerializableDictionary<string, FarmSaveData> farmDictionary;
    
    public InventorySaveData playerInventory;

    public PlayerData playerData ;
    public GameTimeStamp timeData ;

    public SaveData()
    {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        chestDictionary = new SerializableDictionary<string, ChestSaveData>();
        farmDictionary = new SerializableDictionary<string, FarmSaveData>();
        playerInventory = new InventorySaveData();
        playerData = new PlayerData();
    }
}
