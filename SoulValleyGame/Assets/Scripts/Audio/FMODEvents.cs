using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Music2D")]
    [field: SerializeField] public EventReference music2D { get; private set; }
    // Start is called before the first frame update
    [field : Header("Collect SFX")]
    [field: SerializeField] public EventReference itemCollected { get; private set; }
    [field: Header("Clicked SFX")]
    [field: SerializeField] public EventReference clickedSound { get; private set; }

    [field: Header("Footsteps SFX")]
    [field: SerializeField] public EventReference footSteps { get; private set; }

    [field: Header("FootstepsSprint SFX")]
    [field: SerializeField] public EventReference footStepsSprint { get; private set; }
    public static FMODEvents instance { get; private set; }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager");
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
