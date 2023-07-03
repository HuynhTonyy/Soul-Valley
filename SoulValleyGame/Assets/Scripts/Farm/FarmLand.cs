using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : MonoBehaviour, ITimeTracker
{
    public GameObject select;
    [Header("Crop")]
    public GameObject cropPrefab;
    public CropBehaviour cropPlanted = null;
    [SerializeField] Material dryMat, tilledMat, wateredMat;
    public enum LandState
    {
        Dry, 
        Tilled,
        Watered
    }
    public LandState landState = LandState.Dry;
    GameTimeStamp timeWatered;
    Renderer renderer;
    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
        renderer = GetComponent<Renderer>();
    }
    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    public bool Plant(SeedData seed)
    {
        if(cropPlanted == null && landState == LandState.Tilled)
        {
            GameObject cropObject = Instantiate(cropPrefab,transform);
            cropObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.PLant(seed);
            return true;
        }
        else
        {
            return false;
        }
    }
    void SwitchLandState(LandState stateToSwitch)
    {
        landState = stateToSwitch;
        switch (stateToSwitch)
        {
            case LandState.Dry:
                renderer.material = dryMat;
                break;
            case LandState.Tilled:
                renderer.material = tilledMat;
                break;
            case LandState.Watered:
                renderer.material = wateredMat;
                break;
        }
    }
    public void Water()
    {
        if(landState == LandState.Tilled && cropPlanted != null && cropPlanted.cropState != CropBehaviour.CropState.Harvestable)
        {
            SwitchLandState(LandState.Watered);
            timeWatered = TimeManager.Instance.GetTimeStamp();
        }

    }
    public void Till()
    {
        if (landState == LandState.Dry && cropPlanted == null)
        {
            SwitchLandState(LandState.Tilled);
        }

    }
    public void Harvest(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;
        List<Transform> children = new List<Transform>();
        foreach(Transform child in cropPlanted.transform)
        {
            children.Add(child);
        }
        if (inventory.AddToInventory(children[children.Count - 1].GetComponent<ItemPickUp>().itemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(children[children.Count  - 1].GetComponent<UniqueID>().ID);
            Destroy(cropPlanted.gameObject);
            cropPlanted = null;
        }
        SwitchLandState(LandState.Dry);
    }
    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if(landState == LandState.Watered)
        {
            cropPlanted.Grow();
            if(GameTimeStamp.CompareTimeStamp(timeWatered, timeStamp, false) >= 24 * 60)
            {
                SwitchLandState(LandState.Tilled);
            }
        }
    }
}
