using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public bool isPlaceable = true;
    Renderer renderer;
    [SerializeField] Material matEnable, matDisable;
    private void Start() {
        renderer = GetComponent<Renderer>();
        renderer.material = matEnable;
    }
    private void OnTriggerEnter(Collider other) {
        isPlaceable = false;
        renderer.material = matDisable;
    }
    private void OnTriggerStay(Collider other) {
        isPlaceable = false;
        renderer.material = matDisable;
    }
    private void OnTriggerExit(Collider other) {
        isPlaceable = true;
        renderer.material = matEnable;
    }
}
