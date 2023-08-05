
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
                GameEventManager.instance.questEvent.ImproveQuest(amountRequire);
                currentAmount = amountRequire;
                photonView.RPC("updateCurrent", RpcTarget.AllBufferedViaServer, amountRequire);
            }
            else{
                GameEventManager.instance.inventoryEvent.RemoveItem(itemData, amountCollect);
                GameEventManager.instance.questEvent.ImproveQuest(currentAmount + amountCollect);
                currentAmount += amountCollect;
                photonView.RPC("updateCurrent", RpcTarget.AllBufferedViaServer, currentAmount);
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
        if (currentAmount == amountRequire)
        {
            GameEventManager.instance.inventoryEvent.onAddItem -= Collected;
            FinishQuestStep();
        }
    }
}
