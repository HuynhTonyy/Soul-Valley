using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FarmLand : MonoBehaviourPunCallbacks, ITimeTracker

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
            GameObject cropObject = PhotonNetwork.Instantiate(cropPrefab.name,transform.position,Quaternion.identity);
            photonView.RPC("SetParentForCropObject", RpcTarget.AllBufferedViaServer, cropObject.GetComponent<PhotonView>().ViewID);
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
            photonView.RPC("UpdateTimeWatered", RpcTarget.AllBufferedViaServer,
                            TimeManager.Instance.GetTimeStamp().year, 
                            (int)TimeManager.Instance.GetTimeStamp().season, 
                            TimeManager.Instance.GetTimeStamp().day,
                            TimeManager.Instance.GetTimeStamp().hour, 
                            TimeManager.Instance.GetTimeStamp().minute);
            photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer,LandState.Watered);
        }
    }
    public void Till()
    {
        if (landState == LandState.Dry && cropPlanted == null)
        {
            photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer,LandState.Tilled);
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
            photonView.RPC("DestroyObject", RpcTarget.AllBufferedViaServer);
            photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer, LandState.Dry);
            
        }
    }
    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if(landState == LandState.Watered)
        {
            cropPlanted.Grow();
            if(GameTimeStamp.CompareTimeStamp(timeWatered, timeStamp, false) >= 24 * 60)
            {
                photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer, LandState.Tilled);
            }
        }
    }
    void SaveFarmData(){
        SeedData seed = null;
        int growth = 1;
        CropBehaviour.CropState CropState = CropBehaviour.CropState.Seed;
        if(cropPlanted != null){
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
        if(!PhotonNetwork.IsMasterClient) return;
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


    [PunRPC]
    public void UpdateTimeWatered(int year, int season, int day, int hour, int minute)
    {
        SwitchLandState(LandState.Watered);
        timeWatered = new GameTimeStamp(year, (GameTimeStamp.Season)season, day, hour, minute);
    }
    [PunRPC]
    public void UpdateLandState(LandState landState)
    {
        SwitchLandState(landState);
    }
    [PunRPC]
    public void SetParentForCropObject(int ViewID)
    {
        // Set the parent for the instantiated GameObject on non-master clients
        GameObject cropObject = PhotonView.Find(ViewID).gameObject;
        cropObject.transform.SetParent(transform);
        cropObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        cropPlanted = cropObject.GetComponent<CropBehaviour>();
    }
    [PunRPC]
    public void DestroyObject()
    {
        Destroy(cropPlanted.gameObject);
        cropPlanted = null;
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
