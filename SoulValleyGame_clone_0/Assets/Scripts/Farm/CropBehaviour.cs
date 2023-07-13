using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    public SeedData seedToGrow;
    [Header("Stage of life")]
    private GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;
    public enum CropState
    {
        Seed,Seedling,Harvestable
    }
    public CropState cropState;
    public int growth = 1;
    int maxGrowth;
    public void PLant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;
        seed = Instantiate(seedToGrow.seed,transform);
        seedling = Instantiate(seedToGrow.seedling, transform);
        harvestable = Instantiate(seedToGrow.harvestable, transform);
        maxGrowth = GameTimeStamp.HoursToMinutes(GameTimeStamp.DaysToHours(seedToGrow.DayToGrow));
        SwitchCropState(CropState.Seed);
    }

    public void Grow()
    {
        growth++;
        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchCropState(CropState.Seedling);
        }else if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchCropState(CropState.Harvestable);
        }
    }
    public void SwitchCropState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        cropState = stateToSwitch;
        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                break;
        }
        cropState = stateToSwitch;

    }
}
