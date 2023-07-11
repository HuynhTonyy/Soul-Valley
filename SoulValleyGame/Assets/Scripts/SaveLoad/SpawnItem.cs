using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    void Awake()
    {
        SaveLoad.OnLoadGame += LoadItem;
        SaveLoad.OnLoadGame += LoadChest;
    }

    void LoadItem(SaveData data)
    {
        foreach (var activeKey in data.activeItems.Keys)
        {
            ItemPickUpSaveData item = data.activeItems[activeKey];
            Instantiate(item.itemData.ItemPreFab, item.position, item.rotation);
        }
    }
    void LoadChest(SaveData data)
    {
        // foreach ( var chestInWorld in FindObjectsOfType<ChestInventory>()){

        //     Destroy(chestInWorld.gameObject);
        // }
        foreach (var chestKey in data.chestDictionary.Keys)
        {
            ChestSaveData chestSaveData =  data.chestDictionary[chestKey];
            GameObject chest = Instantiate(chestSaveData.ItemData.itemData.ItemPreFab, chestSaveData.Position, chestSaveData.Rotation);
            chest.GetComponent<ChestInventory>().LoadInventory(chestSaveData);
        }
    }
}
