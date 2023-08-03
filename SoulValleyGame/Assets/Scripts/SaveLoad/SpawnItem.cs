using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnItem : MonoBehaviourPunCallbacks
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
            PhotonNetwork.Instantiate(item.itemData.ItemPreFab.name, item.position, item.rotation);
        }
    }
    void LoadChest(SaveData data)
    {
        foreach (var chestKey in data.chestDictionary.Keys)
        {
            ChestSaveData chestSaveData =  data.chestDictionary[chestKey];
            GameObject chest = PhotonNetwork.Instantiate(chestSaveData.ItemData.itemData.ItemPreFab.name, chestSaveData.Position, chestSaveData.Rotation);
            chest.GetComponent<ChestInventory>().LoadInventory(chestSaveData);
            chest.GetComponent<ChestInventory>().syncChest();
            Vector3 position = chestSaveData.Position;
            Quaternion rotation = chestSaveData.Rotation;
            chest.GetPhotonView().RPC("LoadChestPosition", RpcTarget.AllBufferedViaServer,
            chest.GetComponent<PhotonView>().ViewID,
            position.x, position.y, position.z);
            chest.GetPhotonView().RPC("LoadChestRotation", RpcTarget.AllBufferedViaServer,
            chest.GetComponent<PhotonView>().ViewID,
            rotation.x, rotation.y, rotation.z, rotation.w);
        }
    }
    [PunRPC]
    public void LoadChestPosition(int viewID, float x,float y,float z){
        GameObject chest = PhotonView.Find(viewID).gameObject;
        chest.transform.position = new Vector3(x,y,z);
    }
    [PunRPC]
    public void LoadChestRotation(int viewID, float x,float y,float z , float w){
        GameObject chest = PhotonView.Find(viewID).gameObject;
        chest.transform.rotation = new Quaternion(x,y,z,w);
    }
}
