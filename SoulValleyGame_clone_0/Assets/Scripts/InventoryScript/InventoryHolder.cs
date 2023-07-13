using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviourPun,IPunObservable
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 9;

    public int Offset => offset;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    public static UnityAction<InventorySystem,int> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;
        primaryInventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // if(stream.IsWriting){
        //     stream.SendNext(primaryInventorySystem.inventorySlot);
        // }else if(stream.IsReading){
        //     Debug.Log((List<InventorySlot>)stream.ReceiveNext());
        //     // primaryInventorySystem = (InventorySystem) stream.ReceiveNext();
        // }
    }
}

[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InvSystem;
    public InventorySaveData(InventorySystem _invSystem)
    {
        InvSystem = _invSystem;
    }

}

