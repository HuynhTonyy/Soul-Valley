
using UnityEngine;
using Photon.Pun;


public class CollectQuestStep : QuestStep
{
    [SerializeField] ItemScript itemData;
    public int amountRequire = 2;
    public int currentAmount = 0;
    private void OnEnable() {
        GameEventManager.instance.inventoryEvent.onAddItem += Collected;
    }
 
    void Collected(ItemScript itemData, int amountCollect){
        if(this.itemData == itemData){
            if(currentAmount + amountCollect >= amountRequire){
                GameEventManager.instance.inventoryEvent.RemoveItem(itemData, amountRequire - currentAmount);
                photonView.RPC("updateCurrent", RpcTarget.AllBufferedViaServer, amountRequire);
            }
            else{
                GameEventManager.instance.inventoryEvent.RemoveItem(itemData, amountCollect);
                photonView.RPC("updateCurrent", RpcTarget.AllBufferedViaServer, currentAmount + amountCollect);
            }
        }
    }
    [PunRPC]
    public void updateCurrent(int amount)
    {
        SetCurrentCheckFinish(amount);
    }
    void SetCurrentCheckFinish(int amount)
    {
        currentAmount = amount;
        GameEventManager.instance.inventoryEvent.onAddItem -= Collected;
        GameEventManager.instance.questEvent.ImproveQuest(currentAmount);
        if (currentAmount == amountRequire)
        {
            FinishQuestStep();
        }
    }
}
