using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    /*public UnityAction<IInteractable> OnInteractableComplete { get; set; }*/

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        var chestSaveData = new ChestSaveData(primaryInventorySystem,transform.position,transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    private void LoadInventory(SaveData data)
    {
        // check the save data for specific chest inventory - if exist load in
        if(data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    public void Interact(Interactor interactor)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem);
        InventoryUIControler.isClosed = false;
    }
    /*public void EndInteraction()
    {
       
    }*/

}

[System.Serializable]
public struct ChestSaveData
{
    public InventorySystem invSystem;
    public Vector3 position;
    public Quaternion rotation;

    public ChestSaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        invSystem = _invSystem;
        position = _position;
        rotation = _rotation;
    }
}
