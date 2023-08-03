using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;

    [SerializeField] Button btnSave, btnLoad;
    PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
        if(!PhotonNetwork.IsMasterClient){
            btnSave.gameObject.SetActive(false);
            // btnLoad.gameObject.SetActive(false);
        }
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        var saveData = data;
        SaveLoad.Save(saveData);
    }

    public static void LoadData(SaveData _data)
    {
        if(!PhotonNetwork.IsMasterClient) return;
        data = _data;
    }
    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}
