using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviourPunCallbacks
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 9;

    public int Offset => offset;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    public static UnityAction<InventorySystem,int,InventorySystem,int> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;
        primaryInventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
    
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

