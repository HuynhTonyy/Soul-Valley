using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    public int Id;
    public string DisplayName;

    [Header("Only gameplay")]
    [TextArea(4, 4)]
    public string Description;

    [Header("Only UI")]
    public bool stackable = true;
    public bool tool = true;

    [Header("Both")]
    public Sprite image;

    public enum ItemType
    {

    }

}
