using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ServerManager : MonoBehaviourPunCallbacks
{
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.InRoom){
            SceneManager.LoadScene("MainMenu");
            PhotonNetwork.LeaveRoom();
        }
    }

}
