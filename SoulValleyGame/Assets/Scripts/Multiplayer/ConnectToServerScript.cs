using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class ConnectToServerScript : MonoBehaviourPunCallbacks
{
    // TODO Create a Login Lobby Scence   

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
    
}
