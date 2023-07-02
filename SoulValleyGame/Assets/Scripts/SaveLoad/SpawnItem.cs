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
            ItemPickUpSaveData item;
            data.activeItems.TryGetValue(activeKey, out item);
            Instantiate(item.itemData.ItemPreFab, item.position, item.rotation);
        }
    }
    
    void LoadChest(SaveData data)
    {
        ChestInventory[] chestInventorys = FindObjectsOfType<ChestInventory>();
        foreach (var chestKey in data.chestDictionary.Keys)
        {
            int lenght = 1;
            foreach (ChestInventory chestInventory in chestInventorys)
            {
                if(chestKey == chestInventory.gameObject.GetComponent<UniqueID>().ID)
                {
                    break;
                }else if(chestKey != chestInventory.gameObject.GetComponent<UniqueID>().ID && lenght == chestInventorys.Length)
                {
                    data.chestDictionary.TryGetValue(chestKey, out ChestSaveData chestSaveData);
                    GameObject chest = chestSaveData.ItemData.ItemPreFab;
                    Instantiate(chest, chestSaveData.Position, chestSaveData.Rotation);
                    chest.GetComponent<ChestInventory>().LoadInventory(data, chestKey);
                }
                lenght++;
            }
            
        }
    }
}
