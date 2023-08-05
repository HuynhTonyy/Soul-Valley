using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InventoryUIControler : MonoBehaviour
{
    public  bool isClosed = true;
    [FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay playerBackpackPanel;
    public Image backGround;

    internal static bool status = false;
    private void Start()
    {
        playerBackpackPanel.gameObject.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        backGround.enabled=false;
    }
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnDynamicPlayerInventoryDisplayRequested += DisplayPlayerInventory;
    }
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnDynamicPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
    }
    public void close()
    {
        if (inventoryPanel.gameObject.activeInHierarchy)
        {
            inventoryPanel.gameObject.SetActive(false);
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (playerBackpackPanel.gameObject.activeInHierarchy)
        {      
            playerBackpackPanel.gameObject.SetActive(false);
            backGround.enabled = false;
            status = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void DisplayInventory(InventorySystem invDisplay,int offset, InventorySystem invDisplayBackPack,int offsetBackpack)
    {
        
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invDisplay,offset);
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invDisplayBackPack, offsetBackpack);
        RectTransform rectTransform = playerBackpackPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0,-88f);
        Cursor.visible = true;
        backGround.enabled = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void DisplayPlayerInventory(InventorySystem invDisplay, int offset)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invDisplay, offset);
        RectTransform rectTransform = playerBackpackPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        Cursor.visible = true;
        backGround.enabled = true;
        status = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
