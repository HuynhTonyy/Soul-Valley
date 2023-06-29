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
        if (Input.GetKey(KeyCode.O))
        {
            TimeManager.Instance.Tick();
        }
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
            if (hit.transform.tag == "Interactable")
            {
                if (Mouse.current.rightButton.wasPressedThisFrame && InventoryUIControler.isClosed)
                {
                    var interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null) StartInteraction(interactable);
                    InventoryUIControler.isClosed = false;
                }
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("Pickup");
                    var inventory = this.gameObject.GetComponent<PlayerInventoryHolder>();
                    if (!inventory) return;
                    if (inventory.AddToInventory(hit.collider.GetComponent<InteractableObject>().itemData, 1))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
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
