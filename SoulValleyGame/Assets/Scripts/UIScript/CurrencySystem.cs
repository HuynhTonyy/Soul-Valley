using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class CurrencySystem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI souldCoinValues;

    public int gold = 500;
    private void Start() {
        SaveLoad.OnSaveData += SaveMoneyData;
        SaveLoad.OnLoadGame += LoadMoneyData;
    }
    void SaveMoneyData(){
        SaveGameManager.data.moneyData = gold;
    }
    void LoadMoneyData(SaveData data){
        if(!PhotonNetwork.IsMasterClient) return;
        photonView.RPC("SetGold", RpcTarget.AllBufferedViaServer,(int)data.moneyData);
    }

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
        souldCoinValues.SetText(gold.ToString());
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
    [PunRPC]
    public void SetGold(int gold)
    {
        this.gold = gold;  
    }
    
}
