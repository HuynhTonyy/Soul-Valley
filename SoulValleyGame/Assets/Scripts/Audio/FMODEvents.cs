using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    // Start is called before the first frame update
    [field : Header("Collect SFX")]
    [field: SerializeField] public EventReference itemCollected { get; private set; }
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
