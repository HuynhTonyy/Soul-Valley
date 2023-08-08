using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class SaveGameManager : MonoBehaviourPunCallbacks
{
    public static SaveData data;
    [SerializeField] Button btnSave, btnLoad, btnExit;
    bool isEscape = false;
    public Image backGround;
    public GameObject player;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
        if(!PhotonNetwork.IsMasterClient){
            btnSave.gameObject.SetActive(false);
            btnLoad.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        backGround.gameObject.SetActive(false);
        btnSave.gameObject.SetActive(false);
        btnLoad.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Keyboard.current.escapeKey.isPressed)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
            if(isEscape){
                player.GetComponent<PlayerCam>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<PlayerInventoryHolder>().enabled = true;
                isEscape = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                backGround.gameObject.SetActive(false);
                btnSave.gameObject.SetActive(false);
                btnLoad.gameObject.SetActive(false);
                btnExit.gameObject.SetActive(false);

            }else{
                isEscape = true;
                player.GetComponent<PlayerCam>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<PlayerInventoryHolder>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                backGround.gameObject.SetActive(true);
                btnSave.gameObject.SetActive(true);
                btnLoad.gameObject.SetActive(true);
                btnExit.gameObject.SetActive(true);
            }
        }
    }
    public void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
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
    
    public void PlayClickedSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
    }
}
