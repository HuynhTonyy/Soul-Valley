using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuestPoint : MonoBehaviour, IIntractable
{
    [SerializeField] QuestData questData;
    string questID;
    QuestState currentQuestState;
    [SerializeField] bool startPoint = true;
    [SerializeField] bool finishPoint = true;
    private void OnEnable() {
        GameEventManager.instance.questEvent.onQuestStateChange += QuestStateChange;
    }
    void OnDisable() {
        GameEventManager.instance.questEvent.onQuestStateChange -= QuestStateChange;
    }
    private void Awake() {
        questID = questData.id;
    }
    

    void QuestStateChange(Quest quest){
        if(quest.data.id.Equals(questID)){
            currentQuestState = quest.state;
        }
    }
    

    public void Interact(Interactor interactor)
    {
        if(currentQuestState.Equals(QuestState.Can_Start) && startPoint)
            GameEventManager.instance.questEvent.StartQuest(questID);
        else if(currentQuestState.Equals(QuestState.Can_Finish) && finishPoint)
            GameEventManager.instance.questEvent.FinishQuest(questID);
    }
}
