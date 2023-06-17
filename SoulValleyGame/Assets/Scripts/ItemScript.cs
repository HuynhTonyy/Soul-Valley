using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    [Header("Only gameplay")]

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;

    public enum ItemType
    {

    }

}
