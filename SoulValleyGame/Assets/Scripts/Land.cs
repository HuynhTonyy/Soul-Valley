using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public enum LandStatus
    {
        Soil, FramLand, Watered
    }
    public LandStatus landStatus;
    new Renderer renderer;
    public Material soilMat, framLandMat, wateredMat;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        switchLandStatus(LandStatus.Soil);
    }


    void switchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.FramLand:
                materialToSwitch = framLandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                break;
        }
        renderer.material = materialToSwitch;
    }
}
