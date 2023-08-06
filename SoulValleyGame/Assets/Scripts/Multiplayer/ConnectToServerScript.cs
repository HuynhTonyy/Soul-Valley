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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
        SceneManager.LoadScene("Lobby");
    }
    public void LeaveGame(){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
        Debug.Log("Quit");
        Application.Quit();
    }

}
