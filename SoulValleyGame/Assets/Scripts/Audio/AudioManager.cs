using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    public static AudioManager instance { get; private set; }

    private static List<EventInstance> eventInstancesList = new List<EventInstance>();

    private EventInstance musicEventInstance;
    static bool isMusicPlaying = false;
    int sceneIndex;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Audio Manager");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }
    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
    }
    private void Start()
    {
        
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
    private void HandleBackgroundMusic()
    {
        

        if (sceneIndex == 0 || sceneIndex == 1)
        {
            if (!isMusicPlaying)
            {
                CleanUp();
                InitializeMusic2D(FMODEvents.instance.lobbymusic2D);
                isMusicPlaying = true;
            }
            else
            {
                // If the music is already playing, do nothing.
            }
        }
        else if (sceneIndex == 2)
        {
            CleanUp();
            InitializeMusic2D(FMODEvents.instance.music2D);
            isMusicPlaying = false;
        }
    }
    private void InitializeMusic2D(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.setVolume(0.5f);
        musicEventInstance.start();

    }


    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstancesList)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

    }


    private void OnDestroy()
    {       
         /*CleanUp(); */  
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the sceneIndex variable when a new scene is loaded.
        sceneIndex = scene.buildIndex;
        // Handle background music based on the loaded scene.
        HandleBackgroundMusic();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
