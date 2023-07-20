using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ServerManager : MonoBehaviourPunCallbacks
{
    private bool isMasterLeaving = false;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Nếu là Master Client, gán giá trị true cho custom property "IsMaster"
            ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable();
            hashTable.Add("IsMaster", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
        }
        isMasterLeaving = PhotonNetwork.IsMasterClient;
    }
    public override void OnLeftRoom()
    {
        // Xử lý việc rời phòng
        if (isMasterLeaving)
        {
            // Nếu Master Client rời phòng, thì tất cả các người chơi khác cũng rời phòng
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable());
                PhotonNetwork.LeaveRoom();
            }
        }
        else
        {
            // Nếu không phải Master Client rời phòng, chỉ cần đồng bộ hóa custom property của người chơi
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.IsLocal && changedProps.ContainsKey("IsMaster") && !(bool)changedProps["IsMaster"])
        {
            // Nếu custom property "IsMaster" của Master Client bị xóa (không còn là Master Client nữa)
            // tức là Master Client đã rời phòng, người chơi local cũng rời phòng
            PhotonNetwork.LeaveRoom();
        }
    }
}
