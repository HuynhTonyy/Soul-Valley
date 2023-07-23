using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class CurrencySystem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI souldCoinValues;

    public int gold = 500;

    public void SpendCoin(int spendAmount)
    {
        photonView.RPC("RemoveGold", RpcTarget.AllBufferedViaServer,(int)spendAmount);
    }

    public void GainCoin(int gainAmount)
    {
        photonView.RPC("AddGold", RpcTarget.AllBufferedViaServer,(int)gainAmount);
    }

    void Update()
    {
        souldCoinValues.SetText("Currency: " + gold);
    }
     
    [PunRPC]
    public void AddGold(int gold)
    {
        this.gold += gold;  
    }
    [PunRPC]
    public void RemoveGold(int gold)
    {
        this.gold -= gold;  
    }
    
}
