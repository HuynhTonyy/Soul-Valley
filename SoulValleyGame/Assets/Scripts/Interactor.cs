using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    public bool IsInteracting { get; set; }

    public static RaycastHit hit;
    public Transform cam;
    public static FarmLand selectedLand;
    public static bool inRange;
    private void Update()
    {
        inRange = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4f);
        if (inRange)
        {
            if(hit.transform.tag == "FarmLand")
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
            if (hit.transform.tag == "Interactable" && Mouse.current.rightButton.wasPressedThisFrame && InventoryUIControler.isClosed)
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null) StartInteraction(interactable);
                InventoryUIControler.isClosed = false;
            }
        }

    }
    private void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    void EndInteraction()
    {
        IsInteracting = false;
    }
}
