
using UnityEngine;
using Photon.Pun;

public class CollectQuestStep : QuestStep
{
    [SerializeField] ItemScript itemData;
    int amountRequire = 5;
    int currentAmount = 0;
    private void OnEnable() {
        GameEventManager.instance.inventoryEvent.onAddItem += Collected;
    }
    private void OnDisable() {
        GameEventManager.instance.inventoryEvent.onAddItem -= Collected;
    }
    void Collected(ItemScript itemData, int amountCollect){
        if(this.itemData == itemData){
            if(currentAmount + amountCollect >= amountRequire){
                GameEventManager.instance.inventoryEvent.RemoveItem(itemData,amountRequire - currentAmount);
                photonView.RPC("updateCurrent",RpcTarget.AllBufferedViaServer,amountRequire);

            }else{
                GameEventManager.instance.inventoryEvent.RemoveItem(itemData,amountCollect);
                photonView.RPC("updateCurrent",RpcTarget.AllBufferedViaServer,currentAmount + amountCollect);
            }
        }
    }

    [PunRPC]
    public void updateCurrent(int amount)
    {
        
        currentAmount = amount;
        if(currentAmount == amountRequire){
            FinishQuestStep();
        }
    }
}
