using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmLand : MonoBehaviour, ITimeTracker, IPunObservable
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
    LandState landState = LandState.Dry;
    GameTimeStamp timeWatered;
    new Renderer renderer;
    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
        renderer = GetComponent<Renderer>();
        SaveLoad.OnSaveData += SaveFarmData;
        SaveLoad.OnLoadGame += LoadFarmData;
    }
    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    public bool Plant(SeedData seed)
    {
        if(cropPlanted == null && landState == LandState.Tilled && seed != null)
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
        if (inventory.AddToInventory(children[children.Count - 1].GetComponent<CropHarvest>().itemData, 1))
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
    void SaveFarmData(){
        SeedData seed = null;
        int growth = 1;
        CropBehaviour.CropState CropState = CropBehaviour.CropState.Seed;
        if(cropPlanted != null){
            seed = cropPlanted.seedToGrow;
            growth = cropPlanted.growth;
            CropState = cropPlanted.cropState;
        }
        FarmSaveData landSaveData = new FarmSaveData(seed,growth,CropState,timeWatered,landState);
        if(SaveGameManager.data.farmDictionary.ContainsKey(GetComponent<UniqueID>().ID)){
            SaveGameManager.data.farmDictionary[GetComponent<UniqueID>().ID] = landSaveData;
        }else{
            SaveGameManager.data.farmDictionary.Add(GetComponent<UniqueID>().ID, landSaveData);
        }
    }
    void LoadFarmData(SaveData data){
        if(data.farmDictionary.TryGetValue(GetComponent<UniqueID>().ID,out FarmSaveData land)){
            if(GetComponentInChildren<CropBehaviour>() != null){
                Destroy(GetComponentInChildren<CropBehaviour>().gameObject);
            }
            cropPlanted = null;
            if(land.SeedData == null){
                SwitchLandState(land.LandState);
            }else{
                SwitchLandState(LandState.Tilled);
                if(Plant(land.SeedData)){
                    cropPlanted.growth = land.Growth;
                    cropPlanted.SwitchCropState(land.CropState);
                    SwitchLandState(LandState.Watered);
                    timeWatered = land.TimeWatered;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext(cropPlanted.seedToGrow);
            stream.SendNext(cropPlanted.growth);
            stream.SendNext(cropPlanted.cropState);
            stream.SendNext(timeWatered);
            stream.SendNext(landState);
        }
        else if(stream.IsReading)
        {
            cropPlanted.seedToGrow = (SeedData)stream.ReceiveNext();
            cropPlanted.growth = (int)stream.ReceiveNext();
            cropPlanted.cropState = (CropBehaviour.CropState)stream.ReceiveNext();
            timeWatered = (GameTimeStamp)stream.ReceiveNext();
            landState = (FarmLand.LandState)stream.ReceiveNext();
        }
    }
}

[System.Serializable]
public struct FarmSaveData
{
    public SeedData SeedData;
    public int Growth;
    public CropBehaviour.CropState CropState;
    public GameTimeStamp TimeWatered;
    public FarmLand.LandState LandState;
    public FarmSaveData(SeedData _seedData, int _growth,CropBehaviour.CropState _cropState,GameTimeStamp _timeWatered, FarmLand.LandState _landState)
    {
        SeedData = _seedData;
        Growth = _growth;
        CropState = _cropState;
        TimeWatered = _timeWatered;
        LandState = _landState;
    }
}
