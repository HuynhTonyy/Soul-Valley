using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    /*[SerializeField] private ItemNeedToSpawnData itemSpawnData;
    public ItemScript itemData;
 
    protected  void Awake()
    {
        SaveLoad.OnLoadGame += LoadItem;
        itemSpawnData = new ItemNeedToSpawnData(itemData, transform.position, transform.rotation);
    }

    *//*protected override void LoadItem(SaveData data) 
    {
        // check the save data for specific item - if exist load in
        if (data.activeItems.TryGetValue(GetComponent<UniqueID>().ID, out ItemNeedToSpawnData itemSpawnData))
        {
            this.itemData= itemSpawnData.itemData;
            this.transform.position = itemSpawnData.Position;
            this.transform.rotation = itemSpawnData.Rotation;
        }
    }*/
}
[System.Serializable]
public struct ItemNeedToSpawnData
{
    public ItemScript itemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemNeedToSpawnData(ItemScript _invData, Vector3 _position, Quaternion _rotation)
    {
        itemData = _invData;
        Position = _position;
        Rotation = _rotation;
    }
}
