using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class CurrencySystem : MonoBehaviour,IPunObservable
{

    public TextMeshProUGUI souldCoinValues;

    public int gold = 500;

    public void SpendCoin(int spendAmount)
    {
        gold -= spendAmount;
    }

    public void GainCoin(int gainAmount)
    {
        gold += gainAmount;
    }

    void Update()
    {
        souldCoinValues.SetText("Currency: " + gold);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(gold);
        }
        else{
            gold = (int)stream.ReceiveNext();
        }
    }
}
