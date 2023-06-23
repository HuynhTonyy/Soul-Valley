using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUIControler : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;
    public Image backGround;

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
        backGround.enabled=false;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame
            || inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.iKey.wasPressedThisFrame)
        { 
            inventoryPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            Cursor.visible = false;
        }
    }

    private void DisplayInventory(InventorySystem invDisplay)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invDisplay);
        Cursor.visible = true;
        backGround.enabled = true;
    }
}
