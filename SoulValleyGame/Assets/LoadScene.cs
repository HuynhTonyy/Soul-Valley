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
    public GameObject loadingScene;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponentInParent<PlayerCam>().enabled = false;
        GetComponentInParent<PlayerMovement>().enabled = false;
        GetComponentInParent<PlayerInventoryHolder>().enabled = false;
        /*StartCoroutine(TimeUpdate());*/
        /* Destroy(gameObject);*/
    }
    public void SetPlayerName()
    {
        GetComponentInParent<PlayerCam>().enabled = true;
        GetComponentInParent<PlayerMovement>().enabled = true;
        GetComponentInParent<PlayerInventoryHolder>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loadingScene.SetActive(false);
        photonView.RPC("showName", RpcTarget.AllBufferedViaServer, gameObject.GetComponent<PhotonView>().ViewID,playerName.text.ToString());
    }
    [PunRPC]
    public void showName(int viewID, string playerName)
    {
        PhotonView.Find(viewID).GetComponent<TextMeshPro>().SetText(playerName);
    }
}
