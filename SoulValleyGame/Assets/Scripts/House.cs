using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class House : MonoBehaviour, IIntractable
{
    int roomCount;
    int amountReady;
    List<GameObject> playerList = new List<GameObject>();
    private void Update()
    {
        roomCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }
    public void Interact(Interactor interactor)
    {
        if(TimeManager.Instance.GetTimeStamp().hour > 12 && TimeManager.Instance.GetTimeStamp().hour < 7)
        {
            interactor.isInAction = true;
            amountReady++;
            playerList.Add(interactor.gameObject);
            if (amountReady >= roomCount)
            {
                while (TimeManager.Instance.GetTimeStamp().hour != 7)
                {
                    TimeManager.Instance.Tick();
                }
                playerList.Clear();
                amountReady = 0;
            }
           
        }
        
    }
    public void QuitQueue(GameObject player)
    {
        playerList.Remove(player);
        amountReady--;
    }

}
