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
    CropState cropState;
    public void PLant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;
        seed = Instantiate(seedToGrow.ItemPreFab,transform);
        seedling = Instantiate(seedToGrow.seedling, transform);
        harvestable = Instantiate(seedToGrow.CropToYeild.ItemPreFab, transform);
        SwitchToState(CropState.Seed);
    }

    void Grow()
    {
        
    }
    public void SwitchToState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
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
