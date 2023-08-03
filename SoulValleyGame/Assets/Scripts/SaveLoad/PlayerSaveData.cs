using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System.IO;

[RequireComponent(typeof(UniqueID))]
public class PlayerSaveData : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        SaveLoad.OnLoadGame += LoadPlayerData;
        SaveLoad.OnSaveData += SavePlayerData;
    }
    private void SavePlayerData(){
        if(photonView.IsMine){
            SavePlayer();
            if(PhotonNetwork.IsMasterClient){
                photonView.RPC("SaveAllPlayer",RpcTarget.OthersBuffered);
            }   
        }
    }
    private void LoadPlayerData(SaveData saveData){
        photonView.RPC("LoadAllPlayer",RpcTarget.AllBufferedViaServer);
    }
    private void SavePlayer(){
        var playerData = new PlayerData(gameObject.transform.position,
        gameObject.GetComponentInChildren<Camera>().gameObject.transform.rotation,
        gameObject.GetComponent<PlayerInventoryHolder>().PrimaryInventorySystem);
        string playerId = GetComponent<UniqueID>().ID;
        if(SaveGameManager.data.playerData.ContainsKey(playerId))
            SaveGameManager.data.playerData[playerId] = playerData;
        else 
            SaveGameManager.data.playerData.Add(playerId,playerData);
    }
    [PunRPC]
    public void SaveAllPlayer(){
        SavePlayer();
        string dir = Application.persistentDataPath + SaveLoad.directory;

        GUIUtility.systemCopyBuffer = dir; 

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(SaveGameManager.data, true);
        File.WriteAllText(dir + SaveLoad.fileName, json);
        Debug.Log("Saving game");
    }
    [PunRPC]
    public void LoadAllPlayer(){
        string fullPath = Application.persistentDataPath + SaveLoad.directory + SaveLoad.fileName;
        SaveData data = new SaveData();
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Loading player data");
            if(data.playerData.TryGetValue(GetComponent<UniqueID>().ID,out PlayerData value))
            {
                transform.position = value.PlayerPosition;
                GetComponentInChildren<Camera>().gameObject.transform.rotation = Quaternion.Euler(value.PlayerRotation.x,0,0);
                GetComponent<PlayerInventoryHolder>().setPrimarySystem(value.PlayerInven); 
            }
        }
        else
        {
            Debug.Log("Save File Doesn't Exist");
        }
    }
}

[System.Serializable]
public struct PlayerData 
{
    public Vector3 PlayerPosition;
    public  Quaternion PlayerRotation;
    public InventorySystem PlayerInven;

    public PlayerData( Vector3 _position, Quaternion _rotation, InventorySystem _playerInven)
    {
        PlayerPosition = _position;
        PlayerRotation = _rotation;
        PlayerInven = _playerInven;
    }
}

