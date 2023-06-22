using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class ItemScript : ScriptableObject
{
    public int Id;
    public string DisplayName;

    [Header("Only gameplay")]
    [TextArea(4, 4)]
    public string Description;

    [Header("Only UI")]
    public int MaxStackSize;

    [Header("Both")]
    public Sprite icon;

  

}
