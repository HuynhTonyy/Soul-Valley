using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/Tool")]
public class ToolData : ItemScript
{
    public enum ToolType
    {
        WateringCan,
        Hoe,
        Axe,
        Pickaxe,
        Shovel,
        Hammer
    }
    public ToolType toolType;
}
