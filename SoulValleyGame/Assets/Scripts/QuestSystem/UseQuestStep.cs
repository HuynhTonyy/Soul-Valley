
using UnityEngine;
using Photon.Pun;

public class UseQuestStep : QuestStep
{
    [SerializeField] ItemScript itemData;
    public int amountRequire = 2;
    public int currentAmount = 0;
    public enum UseType{
        use,
        reNew
    }
    public UseType useType;
    private void OnEnable() {
        GameEventManager.instance.inventoryEvent.onUseItem += Used;
    }
    void Used(string itemID,UseType useType){
        if(itemData.Id == itemID && this.useType == useType){
            currentAmount++;
            GameEventManager.instance.questEvent.ImproveQuest(currentAmount);
            photonView.RPC("updateCurrent", RpcTarget.AllBufferedViaServer, currentAmount);
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
            GameEventManager.instance.inventoryEvent.onUseItem -= Used;
            FinishQuestStep();
        }
    }
}
