using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using FMOD.Studio;
using FMODUnity;

public class FarmLand : MonoBehaviourPunCallbacks, ITimeTracker

{
    public GameObject select;
    [Header("Crop")]
    public GameObject cropPrefab;
    public CropBehaviour cropPlanted = null;
    private EventInstance hoeingSound;
    private EventInstance wateringSound;
    private EventInstance harvestSound;
    FMOD.ATTRIBUTES_3D attributes;
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
        hoeingSound = AudioManager.instance.CreateInstance(FMODEvents.instance.hoeingSound);
        wateringSound = AudioManager.instance.CreateInstance(FMODEvents.instance.wateringSound);
        harvestSound = AudioManager.instance.CreateInstance(FMODEvents.instance.harvestSound);
        attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(transform.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(transform.up);
        hoeingSound.set3DAttributes(attributes);
        wateringSound.set3DAttributes(attributes);
        harvestSound.set3DAttributes(attributes);
        TimeManager.Instance.RegisterTracker(this);
        renderer = GetComponent<Renderer>();
        if(PhotonNetwork.IsMasterClient){
            SaveLoad.OnSaveData += SaveFarmData;
            SaveLoad.OnLoadGame += LoadFarmData;
        }
    }
    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    public bool Plant(SeedData seed)
    {
        
        if (!cropPlanted && landState == LandState.Tilled && seed)
        {
            GameObject cropObject = PhotonNetwork.Instantiate(cropPrefab.name,transform.position,Quaternion.identity);
            photonView.RPC("SetParentForCropObject", RpcTarget.AllBufferedViaServer, cropObject.GetComponent<PhotonView>().ViewID);
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.PLant(seed);
            GameEventManager.instance.inventoryEvent.UseItem(seed.Id,UseQuestStep.UseType.use);
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
    public bool Water()
    {
        if (landState == LandState.Tilled && cropPlanted != null && cropPlanted.cropState != CropBehaviour.CropState.Harvestable)
        {
            wateringSound.start();
            photonView.RPC("UpdateTimeWatered", RpcTarget.AllBufferedViaServer,
                            TimeManager.Instance.GetTimeStamp().year,
                            (int)TimeManager.Instance.GetTimeStamp().season,
                            TimeManager.Instance.GetTimeStamp().day,
                            TimeManager.Instance.GetTimeStamp().hour,
                            TimeManager.Instance.GetTimeStamp().minute);
            photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer, LandState.Watered);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Till()
    {
        if (landState == LandState.Dry && cropPlanted == null)
        {
            hoeingSound.start();
            photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer,LandState.Tilled);
        }

    }
    public void Harvest(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;
        CropBehaviour crop = GetComponentInChildren<CropBehaviour>();
        int random = Random.Range(1, crop.seedData.amountToYield);
        if (inventory.AddToInventory(crop.seedData.CropToYield,random));
        {
            harvestSound.start();
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
                Debug.Log(GameTimeStamp.CompareTimeStamp(timeWatered, timeStamp, false));
                photonView.RPC("UpdateLandState", RpcTarget.AllBufferedViaServer, LandState.Tilled);
            }
        }
    }
    void SaveFarmData(){
        SeedData seed = null;
        int growth = 1;
        CropBehaviour.CropState CropState = CropBehaviour.CropState.Seed;
        if(cropPlanted != null){
            seed = cropPlanted.seedData;
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
            if(GetComponentInChildren<CropBehaviour>()){
                PhotonNetwork.Destroy(GetComponentInChildren<CropBehaviour>().gameObject);
            }
            landState = LandState.Dry;
            cropPlanted = null;
            if(land.SeedData == null){
                photonView.RPC("UpdateLandState",RpcTarget.AllBufferedViaServer,land.LandState);
                timeWatered = new GameTimeStamp(0,(int)GameTimeStamp.Season.SPRING,0,0,0);
            }else{
                landState = LandState.Tilled;
                if(Plant(land.SeedData)){
                    timeWatered = land.TimeWatered;
                    photonView.RPC("UpdateLandState",RpcTarget.AllBufferedViaServer,landState);
                    cropPlanted.seedData = land.SeedData;
                    cropPlanted.growth = land.Growth;
                    cropPlanted.photonView.RPC("UpdateCropState", RpcTarget.AllBufferedViaServer, land.CropState); 
                }
            }
        }
    }


    [PunRPC]
    public void UpdateTimeWatered(int year, int season, int day, int hour, int minute)
    {
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
