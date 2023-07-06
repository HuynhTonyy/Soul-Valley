using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{

    public static bool isShopClosed = true;
    [SerializeField] public ShopKeeperDisplay _shopKeeperDisplay;

    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
    }

    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
        UIController.isShopClosed = false;
    }

    private void Update()
    {
        if (!isShopClosed && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            close();
        }
    }
    public void close()
    {
        if (_shopKeeperDisplay.gameObject.activeInHierarchy)
        {
            isShopClosed = true;
            _shopKeeperDisplay.gameObject.SetActive(false);       
            InventoryUIControler.status = false; // Movement

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _shopKeeperDisplay.BuyTabDisplay.SetActive(true);
            _shopKeeperDisplay.SellTabDisplay.SetActive(false);
        }

    }


    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInv)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.DisplayShowWindow(shopSystem, playerInv);
        InventoryUIControler.status = true;
        isShopClosed = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
