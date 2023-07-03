using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerSaveData : MonoBehaviour
{
    private PlayerData myPlayerData = new PlayerData();

    private void Start()
    {
        SaveGameManager.data.playerData = new PlayerData();

    }
    private void Update()
    {
        myPlayerData.PlayerPosition = transform.position;
        myPlayerData.PlayerRotation = transform.rotation;
    }
    protected virtual void Awake()
    {
        SaveGameManager.data.playerData = myPlayerData;
        SaveGameManager.SaveData();
    }
    protected abstract void LoadMyPlayerData(SaveData saveData);

}

[System.Serializable]
public struct PlayerData 
{
    public int PlayerCurrency;
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;

    public PlayerData(int _currency, Vector3 _position, Quaternion _rotation)
    {
        PlayerCurrency = _currency;
        PlayerPosition = _position;
        PlayerRotation = _rotation;
    }
}

