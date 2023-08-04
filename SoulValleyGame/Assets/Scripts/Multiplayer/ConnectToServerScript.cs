using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class ConnectToServerScript : MonoBehaviourPunCallbacks
{
    // TODO Create a Login Lobby Scence   
    private bool isMasterLeaving = false;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public void LoadLobby(){
        SceneManager.LoadScene("Lobby");
    }
    public void LeaveGame(){
        Debug.Log("Quit");
        Application.Quit();
    }

}
