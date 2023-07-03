using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerData myPlayerData = new PlayerData();

    private void Start()
    {
        SaveLoad.OnLoadGame += LoadMyPlayerData;
        SaveLoad.OnSaveData += SaveMyPlayerData;
    }
    private void SaveMyPlayerData(){
        var playerData = new PlayerData(transform.position,gameObject.GetComponentInChildren<PlayerCam>().transform.rotation);
        SaveGameManager.data.playerData = playerData;
    }
    private void LoadMyPlayerData(SaveData saveData){
        transform.position = saveData.playerData.PlayerPosition;
        gameObject.GetComponentInChildren<PlayerCam>().transform.rotation = saveData.playerData.PlayerRotation;
    }
}

[System.Serializable]
public struct PlayerData 
{
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;

    public PlayerData( Vector3 _position, Quaternion _rotation)
    {
        PlayerPosition = _position;
        PlayerRotation = _rotation;
    }
}

