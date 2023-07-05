using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class PlayerSaveData : MonoBehaviour
{
    private PlayerData myPlayerData = new PlayerData();

    private void Start()
    {
        SaveLoad.OnLoadGame += LoadMyPlayerData;
        SaveLoad.OnSaveData += SaveMyPlayerData;
    }
    private void SaveMyPlayerData(){
        var playerData = new PlayerData(transform.position,gameObject.GetComponentInChildren<PlayerCam>().transform.rotation,
        GetComponent<PlayerInventoryHolder>().PrimaryInventorySystem);
        string playerId = GetComponent<UniqueID>().ID;
        if(SaveGameManager.data.playerData.ContainsKey(playerId)){
            SaveGameManager.data.playerData[playerId] = playerData;
        }
        else SaveGameManager.data.playerData.Add(playerId,playerData);
        
    }
    private void LoadMyPlayerData(SaveData saveData){
        if(saveData.playerData.TryGetValue(GetComponent<UniqueID>().ID,out PlayerData value))
        {
            transform.position = value.PlayerPosition;
            gameObject.GetComponentInChildren<PlayerCam>().transform.rotation = value.PlayerRotation;
            GetComponent<PlayerInventoryHolder>().setPrimarySystem(value.PlayerInven); 
        }
       
    }
}

[System.Serializable]
public struct PlayerData 
{
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;
    public InventorySystem PlayerInven;

    public PlayerData( Vector3 _position, Quaternion _rotation, InventorySystem _playerInven)
    {
        PlayerPosition = _position;
        PlayerRotation = _rotation;
        PlayerInven = _playerInven;
    }
}

