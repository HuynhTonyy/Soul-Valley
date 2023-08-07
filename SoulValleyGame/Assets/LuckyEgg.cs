using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LuckyEgg : MonoBehaviourPunCallbacks
{
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] List<GameObject> pets;
    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == 6){
            Debug.Log("Is ground");
            PhotonNetwork.Instantiate(pets[Random.Range(0,pets.Count)].name,transform.position,Quaternion.identity);
            Debug.Log("spawn success");
            Destroy(gameObject);
        }
    }
}
