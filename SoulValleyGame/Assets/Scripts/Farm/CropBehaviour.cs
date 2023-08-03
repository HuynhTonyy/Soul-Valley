using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CropBehaviour : MonoBehaviourPunCallbacks
{
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
    public SeedData seedData;
    public void PLant(SeedData seedToGrow)
    {
        seedData = seedToGrow;
        seed = PhotonNetwork.Instantiate(seedToGrow.seed.name,transform.position,Quaternion.identity);
        seedling = PhotonNetwork.Instantiate(seedToGrow.seedling.name,transform.position,Quaternion.identity);
        harvestable = PhotonNetwork.Instantiate(seedToGrow.harvestable.name,transform.position,Quaternion.identity);
        photonView.RPC("SetParentForObject", RpcTarget.AllBufferedViaServer, 
                seed.GetComponent<PhotonView>().ViewID,
                seedling.GetComponent<PhotonView>().ViewID,
                harvestable.GetComponent<PhotonView>().ViewID,
                GameTimeStamp.HoursToMinutes(GameTimeStamp.DaysToHours(seedToGrow.DayToGrow)));
        photonView.RPC("UpdateCropState", RpcTarget.AllBufferedViaServer, CropState.Seed);
    }

    public void Grow()
    {
        growth++;
        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            photonView.RPC("UpdateCropState", RpcTarget.AllBufferedViaServer, CropState.Seedling);
        }else if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            photonView.RPC("UpdateCropState", RpcTarget.AllBufferedViaServer, CropState.Harvestable);
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
    }
    [PunRPC]
    public void SetParentForObject(int seedViewID,int seedlingViewID,int harvestableViewID, int maxGrowth)
    {
        // Set the parent for the instantiated GameObject on non-master clients
        seed = PhotonView.Find(seedViewID).gameObject;
        seedling = PhotonView.Find(seedlingViewID).gameObject;
        harvestable = PhotonView.Find(harvestableViewID).gameObject;
        seed.transform.SetParent(transform);
        seedling.transform.SetParent(transform);
        harvestable.transform.SetParent(transform);
        seed.transform.localPosition = new Vector3(0, 0, 0);
        seedling.transform.localPosition = new Vector3(0, 0, 0);
        harvestable.transform.localPosition = new Vector3(0, 0, 0);
        // this.maxGrowth = maxGrowth;
        this.maxGrowth = 16;
    }
    [PunRPC]
    public void UpdateCropState(CropState cropState)
    {
        SwitchCropState(cropState);
    }
}
