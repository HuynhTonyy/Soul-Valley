using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    SeedData seedToGrow;
    [Header("Stage of life")]
    private GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;
    public enum CropState
    {
        Seed,Seedling,Harvestable
    }
    public CropState cropState;
    public enum LandStatus
    {
        Dry, Watered
    }
    public LandStatus landStatus;
    int growth;
    int maxGrowth;
    public void PLant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;
        seed = Instantiate(seedToGrow.seed,transform);
        seedling = Instantiate(seedToGrow.seedling, transform);
        harvestable = Instantiate(seedToGrow.CropToYeild.ItemPreFab, transform);
        SwitchToState(CropState.Seed);
    }

    public void Grow()
    {
        growth++;
        if(growth >= maxGrowth)
        {
            SwitchToState(CropState.Harvestable);
        }
    }
    public void SwitchToState(CropState stateToSwitch)
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
