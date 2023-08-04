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
    public Animator errorBoxAnimator;
    public TextMeshProUGUI errText;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnJoinRoomFailed(short returnCode,string asdasd)
    {
        Debug.Log("JoinRoom fail");
        errText.SetText("Room not exists!");
        errorBoxAnimator.SetTrigger("showTrig");
    }
    public void CreateRoom(){
        if(PhotonNetwork.CurrentRoom.Name != createField.text ){
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 1;
            PhotonNetwork.CreateRoom(createField.text,roomOptions);
        }else{
            errText.SetText("Room exists!");
            errorBoxAnimator.SetTrigger("showTrig");
        }
    }
    public void JoinRoom(){
        if(joinField.text.Length == 0){
            errText.SetText("Enter room name!");
            errorBoxAnimator.SetTrigger("showTrig");
            return;
        }
        PhotonNetwork.JoinRoom(joinField.text);
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient){
                PhotonNetwork.LoadLevel("MainScene");
            }
    }
    public void ReturnToMenu(){
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
