
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
    void Collected(ItemScript itemData, int amount){
        if(this.itemData == itemData){
            if(amount +  currentAmount >= amountRequire){
                currentAmount = amountRequire;
                FinishQuestStep();
            }
            else 
                currentAmount += amount;
        }
    }
}
