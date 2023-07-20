using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CropBehaviour : MonoBehaviourPunCallbacks, IPunObservable
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
    public void PLant(SeedData seedToGrow)
    {
        seed = PhotonNetwork.Instantiate(seedToGrow.seed.name,transform.position,Quaternion.identity);
        seedling = PhotonNetwork.Instantiate(seedToGrow.seedling.name,transform.position,Quaternion.identity);
        harvestable = PhotonNetwork.Instantiate(seedToGrow.harvestable.name,transform.position,Quaternion.identity);
        seed.transform.SetParent(transform);
        seedling.transform.SetParent(transform);
        harvestable.transform.SetParent(transform);
        maxGrowth = GameTimeStamp.HoursToMinutes(GameTimeStamp.DaysToHours(seedToGrow.DayToGrow));
        SwitchCropState(CropState.Seed);
        if(!PhotonNetwork.IsMasterClient){
            photonView.RPC("SetParentForObject", RpcTarget.MasterClient, 
            seed.GetPhotonView().ViewID,
            seedling.GetPhotonView().ViewID,
            harvestable.GetPhotonView().ViewID,
            maxGrowth,
            (int)cropState);
        
        }
        
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // if(stream.IsWriting){
        //     stream.SendNext(maxGrowth);
        //     stream.SendNext(cropState);
        // }else{
        //     maxGrowth = (int) stream.ReceiveNext();
        //     SwitchCropState((CropState)stream.ReceiveNext());
        // }
    }
    [PunRPC]
    public void SetParentForObject(int seedViewID,int seedlingViewID,int harvestableViewID, int maxGrowth, int cropState)
    {
        // Set the parent for the instantiated GameObject on non-master clients
        seed = PhotonView.Find(seedViewID).gameObject;
        seedling = PhotonView.Find(seedlingViewID).gameObject;
        harvestable = PhotonView.Find(harvestableViewID).gameObject;
        seed.transform.SetParent(transform);
        seedling.transform.SetParent(transform);
        harvestable.transform.SetParent(transform);
        this.maxGrowth = maxGrowth;
        SwitchCropState((CropState)cropState);
        
    }
}
