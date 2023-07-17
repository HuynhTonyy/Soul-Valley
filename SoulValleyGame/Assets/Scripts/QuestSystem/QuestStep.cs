using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    bool isFinished = false;
    string questID;
    public void InstantiateQuestStep(string questID){
        this.questID = questID;
    } 
    protected void FinishQuestStep(){
        if(!isFinished){
            isFinished = true;
            GameEventManager.instance.questEvent.AdvanceQuest(questID);
            Destroy(this.gameObject);
        }
    }
}
