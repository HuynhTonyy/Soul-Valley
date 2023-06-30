using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public static RaycastHit hit;
    public Transform cam;
    public static FarmLand selectedLand;
    public static bool inRange;
    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            TimeManager.Instance.Tick();
        }
        inRange = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f);
        if (inRange)
        {
            if (hit.transform.tag == "FarmLand")
            {
                FarmLand farmLand = hit.collider.GetComponent<FarmLand>();
                if (selectedLand != null)
                {
                    selectedLand.Select(false);
                }
                selectedLand = farmLand;
                farmLand.Select(true);
            }
            else if (selectedLand != null)
            {
                selectedLand.Select(false);
            }
            if (hit.transform.tag == "Interactable")
            {
                if (Mouse.current.rightButton.wasPressedThisFrame && InventoryUIControler.isClosed)
                {
                    var interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null) interactable.Interact(this);
                }
            }
        }

    }
}

