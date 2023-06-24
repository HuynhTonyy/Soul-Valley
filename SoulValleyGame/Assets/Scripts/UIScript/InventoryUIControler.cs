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


    internal static bool status = false;
    private void Awake()
    {
        playerBackpackPanel.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(false);
        backGround.enabled=false;

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
        if (chestPanel.gameObject.activeInHierarchy && (Keyboard.current.escapeKey.wasPressedThisFrame
            || Keyboard.current.bKey.wasPressedThisFrame))
        {
            
            chestPanel.gameObject.SetActive(false);
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(playerBackpackPanel.gameObject.activeInHierarchy && (Keyboard.current.escapeKey.wasPressedThisFrame
            || Keyboard.current.bKey.wasPressedThisFrame))
        {
            Debug.Log("hien");
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void DisplayInventory(InventorySystem invDisplay)
    {
        chestPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invDisplay);
        playerBackpackPanel.gameObject.SetActive(true);
        Cursor.visible = true;
        backGround.enabled = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
       
    }
    void DisplayPlayerBackpack(InventorySystem invDisplay)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invDisplay);
        Cursor.visible = true;
        backGround.enabled = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    
}
