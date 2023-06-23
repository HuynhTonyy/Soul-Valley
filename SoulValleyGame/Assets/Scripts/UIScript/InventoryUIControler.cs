using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUIControler : MonoBehaviour
{
    public DynamicInventoryDisplay chestPanel;
    public DynamicInventoryDisplay playerBackpackPanel;
    public Image backGround;

    private void Awake()
    {
        playerBackpackPanel.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(false);
        backGround.enabled=false;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
    }
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    // Update is called once per frame
    void Update()
    {
        if (chestPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame
            || chestPanel.gameObject.activeInHierarchy && Keyboard.current.iKey.wasPressedThisFrame)
        { 
            chestPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            Cursor.visible = false;
        }
        if(playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame
            || playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.iKey.wasPressedThisFrame)
        {
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            Cursor.visible = false;
        }
    }

    void DisplayInventory(InventorySystem invDisplay)
    {
        chestPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invDisplay);
        Cursor.visible = true;
        backGround.enabled = true;
    }
    void DisplayPlayerBackpack(InventorySystem invDisplay)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invDisplay);
        Cursor.visible = true;
        backGround.enabled = true;
    }
}
