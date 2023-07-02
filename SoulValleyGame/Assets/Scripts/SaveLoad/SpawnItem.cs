using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{

    void Awake()
    {
        SaveLoad.OnLoadGame += LoadItem;
    }

    void LoadItem(SaveData data)
    {
        foreach (var activeKey in data.activeItems.Keys)
        {
            ItemPickUpSaveData item;
            data.activeItems.TryGetValue(activeKey, out item);
            Instantiate(item.itemData.ItemPreFab, item.position, item.rotation);
        }
        LoadChest(data);
    }
    
    void LoadChest(SaveData data)
    {
        foreach (var chestKey in data.chestDictionary.Keys)
        {
            InventorySaveData inventorySaveData;
            data.chestDictionary.TryGetValue(chestKey, out inventorySaveData);

            GameObject chest = inventorySaveData.itemData.ItemPreFab;
            Instantiate(chest, inventorySaveData.Position,inventorySaveData.Rotation);
            chest.GetComponent<ChestInventory>();
        }
    }
}
