using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Quest
{
    public QuestData data;
    public QuestState state;
    int currentQuestStepIndex;
    public Quest(QuestData questData){
        this.data = questData;
        this.state = QuestState.Requirement_Not_Met;
        currentQuestStepIndex = 0;
    }
    public void MoveToNextStep(){
        currentQuestStepIndex++;
    }
    public bool CurrentStepExists(){
        return (currentQuestStepIndex <data.questStepPrefabs.Length);
    }
    public void InstantiateCurrentQuestStep(Transform parentTransform){
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null){
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab,parentTransform).GetComponent<QuestStep>();
            questStep.InstantiateQuestStep(data.id);
        }
    }
    public GameObject GetCurrentQuestStepPrefab(){
        GameObject currentStepPrefab = null;
        if(CurrentStepExists()){
            currentStepPrefab = data.questStepPrefabs[currentQuestStepIndex];
        }
        return currentStepPrefab;
    }

}
