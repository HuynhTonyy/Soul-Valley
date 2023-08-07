using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// A scriptable object, define what an item in the game
// 

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class ItemScript : ScriptableObject
{
    [field: SerializeField] public string Id {get; private set;}
    public string DisplayName;
    public int MaxStackSize;
    public int Value = 0;
    public string Description = "";
    public Sprite icon;
    public GameObject ItemPreFab;
    
    void OnValidate()
    {
        #if UNITY_EDITOR
            Id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

}
