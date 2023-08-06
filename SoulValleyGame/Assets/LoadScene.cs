using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadScene : MonoBehaviourPunCallbacks
{
    public InputField playerName;
    public GameObject playerNameDisplay;
    public GameObject playerPrefab;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerPrefab.GetComponent<PlayerCam>().enabled = false;
        playerPrefab.GetComponent<PlayerMovement>().enabled = false;
        playerPrefab.GetComponent<PlayerInventoryHolder>().enabled = false;
        /*StartCoroutine(TimeUpdate());*/
        /* Destroy(gameObject);*/
    }
    private void Update()
    {
        if(Keyboard.current.enterKey.isPressed)
        {
            SetPlayerName();
        }
    }
    public void SetPlayerName()
    {
        playerNameDisplay.GetComponent<TextMeshPro>().SetText(playerName.text.ToString());
        playerPrefab.GetComponent<PlayerCam>().enabled = true;
        playerPrefab.GetComponent<PlayerMovement>().enabled = true;
        playerPrefab.GetComponent<PlayerInventoryHolder>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        
    }
}
