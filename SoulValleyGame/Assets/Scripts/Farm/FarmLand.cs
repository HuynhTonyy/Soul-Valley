using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmLand : MonoBehaviour
{
    public GameObject select;
    [Header("Crop")]
    public GameObject cropPrefab;
    CropBehaviour cropPlanted = null;
    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    public bool Interact(SeedData seed)
    {
        if(cropPlanted == null)
        {
            Debug.Log(transform.position);
            GameObject cropObject = Instantiate(cropPrefab, transform);
            cropObject.transform.localPosition = new Vector3(0, 0, 0);
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.PLant(seed);
            return true;
        }
        else
        {
            return false;
        }
    }
}
