using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using FMOD.Studio;
using FMODUnity;

public class QuestPoint : MonoBehaviourPunCallbacks, IIntractable
{
    [SerializeField] QuestData[] questDatas;
    QuestState currentQuestState;
    [SerializeField] bool startPoint = true;
    [SerializeField] bool finishPoint = true;
    private EventInstance questAcceptedSound;
    private EventInstance questCompletedSound;
    FMOD.ATTRIBUTES_3D attributes;
    int questIndex = 0;

    private void Start()
    {
        attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(transform.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(transform.up);
        questAcceptedSound = AudioManager.instance.CreateInstance(FMODEvents.instance.questAcceptedSound);
        questCompletedSound = AudioManager.instance.CreateInstance(FMODEvents.instance.questCompletedSound);

        questAcceptedSound.set3DAttributes(attributes);
        questCompletedSound.set3DAttributes(attributes);
    }

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
        return questIndex >= questDatas.Length;
    }
    public void Interact(Interactor interactor)
    {

        if(CheckQuestExists()){
            return;// refesh on the next day
        }
        if(currentQuestState.Equals(QuestState.Can_Start) && startPoint){
            questAcceptedSound.start();
            GameEventManager.instance.questEvent.StartQuest(questDatas[questIndex].id);
        }
            
        else if(currentQuestState.Equals(QuestState.Can_Finish) && finishPoint){
            questCompletedSound.start();
            GameEventManager.instance.questEvent.FinishQuest(questDatas[questIndex].id);
            photonView.RPC("updateQuestIndex",RpcTarget.AllBufferedViaServer,questIndex+1);
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
        if(!CheckQuestExists()){
            photonView.RPC("updateQuestState",RpcTarget.AllBufferedViaServer,
                GameObject.FindWithTag("QuestManager").GetComponent<QuestManager>().GetQuestByID(questDatas[questIndex].id).state);
        }
    }
}
