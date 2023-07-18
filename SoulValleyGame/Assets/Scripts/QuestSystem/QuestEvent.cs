using System;

public class QuestEvent
{
    public event Action<String> onStartQuest;
    public event Action<String> onAdvanceQuest;
    public event Action<String> onFinishQuest;
    public event Action<Quest> onQuestStateChange;
    public event Action<ItemScript,int> onStartQuestStep;
    public void StartQuest(string id){
        if(onStartQuest != null){
            onStartQuest(id);
        }
    }
    public void AdvanceQuest(string id){
        if(onAdvanceQuest != null){
            onAdvanceQuest(id);
        }
    }
    public void FinishQuest(string id){
        if(onFinishQuest != null){
            onFinishQuest(id);
        }
    }
    public void QuestStateChange(Quest quest){
        if(onQuestStateChange != null){
            onQuestStateChange(quest);
        }
    }
    public void StartQuestStep(ItemScript item,int amount){
        if(onStartQuestStep != null){
            onStartQuestStep(item,amount);
        }
    }
}
