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
    List<RoomInfo> roomList = new List<RoomInfo>();

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.AddCallbackTarget(this);
    }

    public void JoinRoomInList(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinRoomFailed(short returnCode,string asdasd)
    {
        ErrorMessage("Room not exists or full!");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
    }
    public void CreateRoom(){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
        if (createField.text.Length > 0){
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            bool isExists = false;
            foreach(RoomInfo roomInfo in roomList){
                if(createField.text == roomInfo.Name && roomInfo.MaxPlayers > 0){
                    ErrorMessage("Room already exists!");
                    isExists = true;
                    break;
                }
            }
            if(!isExists)
                PhotonNetwork.CreateRoom(createField.text,roomOptions);
        }else{
            // errText.SetText("Room exists!");
            ErrorMessage("Enter room name!");
        }
    }
    private void ErrorMessage(string ErrorMessage){
        errText.SetText(ErrorMessage);
            errorBoxAnimator.SetTrigger("showTrig");
    }
    public void JoinRoom(){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
        if (joinField.text.Length == 0){
            ErrorMessage("Enter room name!");
            return;
        }
        if (PhotonNetwork.JoinRoom(joinField.text))
        {
            PhotonNetwork.JoinRoom(joinField.text);
        }
        else ErrorMessage("Room doesn't exist");
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel(2);
        }
    }
    public void ReturnToMenu(){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public void PlayClickSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.clickedSound, this.transform.position);
    }
}
