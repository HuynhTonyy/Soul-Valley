using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/Seed")]
public class SeedData : ItemScript
{
    public int DayToGrow;
    public GameObject seedling, seed, harvestable;
    public ItemScript CropToYeild;
}
