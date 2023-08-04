using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX,maxX,minZ,maxZ;

    private void Start(){
        Vector3 ramdonPosition = new Vector3(Random.Range(maxX,minX),transform.position.y,Random.Range(maxZ,minZ));
        
        PhotonNetwork.Instantiate(playerPrefab.name,ramdonPosition,Quaternion.identity);
        
    }
}
