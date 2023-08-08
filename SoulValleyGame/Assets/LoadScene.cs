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
    public void SetPlayerName()
    {
        playerPrefab.GetComponent<PlayerCam>().enabled = true;
        playerPrefab.GetComponent<PlayerMovement>().enabled = true;
        playerPrefab.GetComponent<PlayerInventoryHolder>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        photonView.RPC("showName", RpcTarget.AllBufferedViaServer,playerNameDisplay.GetComponent<PhotonView>().ViewID,playerName.text.ToString());
    }
    [PunRPC]
    public void showName(int viewID, string playerName)
    {
        PhotonView.Find(viewID).GetComponent<TextMeshPro>().SetText(playerName);
    }
}
