using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Audio Manager");
        }
        instance = this;
    }

    // Update is called once per frame
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
