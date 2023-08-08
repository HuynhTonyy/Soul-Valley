using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public bool isPlaceable = true;
    SkinnedMeshRenderer[] renderers;
    [SerializeField] Material matEnable, matDisable;
    private void Start() {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        changeMaterial(matEnable);
    }
    private void OnTriggerEnter(Collider other) {
        isPlaceable = false;
        changeMaterial(matDisable);
    }
    private void OnTriggerStay(Collider other) {
        isPlaceable = false;
        changeMaterial(matDisable);
    }
    private void OnTriggerExit(Collider other) {
        isPlaceable = true;
        changeMaterial(matEnable);
    }
    private void changeMaterial(Material materialToChange){
        foreach(SkinnedMeshRenderer renderer in renderers){
            renderer.material = materialToChange;
        }
    }
}
