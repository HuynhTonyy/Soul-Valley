using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField createField,joinField;
    private bool isMasterLeaving = false;   

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom(){

        PhotonNetwork.CreateRoom(createField.text);
        //(,new RoomOptions{MaxPlayer = 4})
    }
    public void JoinRoom(){

        PhotonNetwork.JoinRoom(joinField.text);
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel("MainScene");
        }
    }
    
}
