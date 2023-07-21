using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quest/QuestData", order = 0)]
public class QuestData : ScriptableObject {
    [field: SerializeField] public string  id {get; private set;}
    [Header("General")]
    public string displayName;
    [Header("Requirements")]
    public int goldRequirement;
    public QuestData[] questPrerequisites;
    [Header("Step")]
    public GameObject[] questStepPrefabs;
    [Header("Rewards")]
    public int goldReward;
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        #if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}


