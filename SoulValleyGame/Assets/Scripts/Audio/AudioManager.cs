using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private List<EventInstance> eventInstancesList;

    private EventInstance musicEventInstance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Audio Manager");
        }
        instance = this;
        eventInstancesList = new List<EventInstance>();
    }
    private void Start()
    {
        InitializeMusic2D(FMODEvents.instance.music2D);
    }

    // Update is called once per frame
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstancesList.Add(eventInstance);
        return eventInstance;
    }

    /*private void InitializeMusic(EventReference musicEventReference)
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(transform.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(transform.up);
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.set3DAttributes(attributes);
        musicEventInstance.start();
        
    }*/

    private void InitializeMusic2D(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.setVolume(0.5f);
        musicEventInstance.start();

    }

    private void CleanUp()
    {
        foreach(EventInstance eventInstance in eventInstancesList)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
