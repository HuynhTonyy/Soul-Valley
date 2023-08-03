using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData
{
    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;

    public SerializableDictionary<string, ChestSaveData> chestDictionary;

    public SerializableDictionary<string, FarmSaveData> farmDictionary;

    public SerializableDictionary<string,PlayerData> playerData;

    public GameTimeStamp timeData ;

    public SaveData()
    {
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        chestDictionary = new SerializableDictionary<string, ChestSaveData>();
        farmDictionary = new SerializableDictionary<string, FarmSaveData>();
        playerData = new SerializableDictionary<string, PlayerData>();
    }
}
