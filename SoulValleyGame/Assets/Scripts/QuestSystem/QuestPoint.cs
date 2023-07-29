using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class QuestPoint : MonoBehaviourPunCallbacks, IIntractable
{
    [SerializeField] QuestData[] questDatas;
    QuestState currentQuestState;
    [SerializeField] bool startPoint = true;
    [SerializeField] bool finishPoint = true;
    int questIndex = 0;
    private void OnEnable() {
        GameEventManager.instance.questEvent.onQuestStateChange += QuestStateChange;
    }
    void OnDisable() {
        GameEventManager.instance.questEvent.onQuestStateChange -= QuestStateChange;
    }
    
    void QuestStateChange(Quest quest){
        if(quest.data.id.Equals(questDatas[questIndex].id)){
            photonView.RPC("updateQuest",RpcTarget.AllBufferedViaServer,(int)quest.state,questIndex);
        }
    }
    bool CheckQuestExists(){
        return (questIndex >= questDatas.Length);
    }
    public void Interact(Interactor interactor)
    {
        if(CheckQuestExists()){
            return;// refesh on the next day
        }
        if(currentQuestState.Equals(QuestState.Can_Start) && startPoint){
            GameEventManager.instance.questEvent.StartQuest(questDatas[questIndex].id);
        }
            
        else if(currentQuestState.Equals(QuestState.Can_Finish) && finishPoint){
            GameEventManager.instance.questEvent.FinishQuest(questDatas[questIndex].id);
            questIndex++;
            if(!CheckQuestExists()){
                currentQuestState = GameObject.FindWithTag("QuestManager").GetComponent<QuestManager>().GetQuestByID(questDatas[questIndex].id).state;
            }
        } 
    }
    
    [PunRPC]
    public void updateQuest(int state,int index)
    {
        currentQuestState = (QuestState)state;
        questIndex = index;
    }
}
