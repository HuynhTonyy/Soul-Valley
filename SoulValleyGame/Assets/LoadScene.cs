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
        /*playerPrefab.GetComponent<PlayerCam>().gameObject.SetActive(false);
        playerPrefab.GetComponent<PlayerMovement>().gameObject.SetActive(false);*/
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
        playerNameDisplay.GetComponent<TextMeshProUGUI>().SetText(playerName.ToString());
        playerPrefab.GetComponent<PlayerCam>().gameObject.SetActive(true);
        playerPrefab.GetComponent<PlayerMovement>().gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(this.gameObject);
        
    }
}
