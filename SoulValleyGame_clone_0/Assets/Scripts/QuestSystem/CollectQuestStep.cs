
using UnityEngine;

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
                currentAmount = amountRequire;
            }else{
                currentAmount += amountCollect;
                GameEventManager.instance.inventoryEvent.RemoveItem(itemData,amountCollect);
            }
            if(currentAmount == amountRequire){
                FinishQuestStep();
            }
        }
    }
}
