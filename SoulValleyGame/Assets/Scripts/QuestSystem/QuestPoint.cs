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
            photonView.RPC("updateQuestState",RpcTarget.AllBufferedViaServer,(int)quest.state);
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

            photonView.RPC("updateQuestIndex",RpcTarget.AllBufferedViaServer,questIndex+1);
            if(!CheckQuestExists()){
                Debug.Log(GameObject.FindWithTag("QuestManager").GetComponent<QuestManager>().GetQuestByID(questDatas[questIndex].id).data.displayName.ToString());
                photonView.RPC("updateQuestState",RpcTarget.AllBufferedViaServer,
                    GameObject.FindWithTag("QuestManager").GetComponent<QuestManager>().GetQuestByID(questDatas[questIndex+1].id).state);
            }
        } 
    }
    [PunRPC]
    public void updateQuestState(int state)
    {
        currentQuestState = (QuestState)state;
      
    }
    [PunRPC]
    public void updateQuestIndex(int index)
    {
        questIndex = index;
    }
}
