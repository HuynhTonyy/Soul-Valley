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
    static ShopKeeperDisplay shopKeeperDisplay;
    public bool isEscape = false;
    public Image backGround;
    public GameObject player, volumeHolder;

    PlayerCam cam;
    PlayerMovement move;

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
        shopKeeperDisplay = GetComponentInParent<ShopKeeperDisplay>();
        backGround.enabled = false;
        btnSave.gameObject.SetActive(false);
        btnLoad.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
        volumeHolder.SetActive(false);
        cam = player.GetComponent<PlayerCam>();
        move =player.GetComponent<PlayerMovement>();

    }
    public void open()
    {
        isEscape = true;
        cam.enabled = false;
        move.enabled = false;
        backGround.enabled = true;
        btnSave.gameObject.SetActive(true);
        btnLoad.gameObject.SetActive(true);
        btnExit.gameObject.SetActive(true);
        volumeHolder.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void close()
    {
        isEscape = false;
        cam.enabled = true;
        move.enabled = true;
        backGround.enabled = false;
        btnSave.gameObject.SetActive(false);
        btnLoad.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
        volumeHolder.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            var saveData = data;
            SaveLoad.Save(saveData);
        }
        else
        {
            shopKeeperDisplay.errText.SetText("Only the room Owner can save / load the game");
            shopKeeperDisplay.errorBoxAnimator.SetTrigger("showTrig");
            return;
        }
        
    }

    public static void LoadData(SaveData _data)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            shopKeeperDisplay.errText.SetText("Only the room Owner can save / load the game");
            shopKeeperDisplay.errorBoxAnimator.SetTrigger("showTrig");
            return;
        }
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
