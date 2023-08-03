using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
[RequireComponent(typeof(UniqueID))]
public class PlayerSaveData : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        SaveLoad.OnLoadGame += LoadPlayerData;
        SaveLoad.OnSaveData += SavePlayerData;
    }
    private void SavePlayerData(){
        if(PhotonNetwork.IsMasterClient){
            photonView.RPC("SavePlayer",RpcTarget.AllBufferedViaServer);
        }else if(photonView.IsMine){
            SavePlayer();
        }
    }
    private void LoadPlayerData(SaveData saveData){
        if(saveData.playerData.TryGetValue(GetComponent<UniqueID>().ID,out PlayerData value))
        {
            transform.position = value.PlayerPosition;
            GetComponentInChildren<Camera>().gameObject.transform.rotation = Quaternion.Euler(value.PlayerRotation.x,0,0);
            GetComponent<PlayerInventoryHolder>().setPrimarySystem(value.PlayerInven); 
        }
    }
    [PunRPC]
    private void SaveAllPlayer(){
        SavePlayer();
    }
    private void SavePlayer(){
        var playerData = new PlayerData(transform.position,gameObject.GetComponentInChildren<Camera>().gameObject.transform.rotation,
        gameObject.GetComponent<PlayerInventoryHolder>().PrimaryInventorySystem);
        string playerId = GetComponent<UniqueID>().ID;
        if(SaveGameManager.data.playerData.ContainsKey(playerId))
            SaveGameManager.data.playerData[playerId] = playerData;
        else 
            SaveGameManager.data.playerData.Add(playerId,playerData);
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

