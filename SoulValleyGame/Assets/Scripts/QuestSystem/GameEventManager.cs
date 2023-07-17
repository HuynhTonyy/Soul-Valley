using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance {get; private set;} 
    public QuestEvent questEvent;
    void Awake()
    {
        instance = this;
        questEvent = new QuestEvent();
    }
}
