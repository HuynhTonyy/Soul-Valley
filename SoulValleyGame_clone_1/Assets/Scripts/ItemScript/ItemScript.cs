using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// A scriptable object, define what an item in the game
// 

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class ItemScript : ScriptableObject
{
    public int Id;
    public string DisplayName;
    public int MaxStackSize;
    public int Value = 0;
    public string Description = "";
    public Sprite icon;
    public GameObject ItemPreFab;

}
