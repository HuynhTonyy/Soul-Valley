using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.InputSystem;

public class LoadScene : MonoBehaviourPunCallbacks
{
    // public TMP_InputField playerName;

    private void Start()
    {
        // StartCoroutine(TimeUpdate());
        Destroy(gameObject);
    }
    // IEnumerator TimeUpdate()
    // {
    //     if(Keyboard.current.EnterKet.isPressed && playerName.text.Length >0)
    //     {
    //         PhotonNetwork.NickName = playerName.text;
    //         Destroy(gameObject);
    //     }
        
    // }
}
