using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public bool isShopClosed = true;
    [SerializeField] public ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] public LoadScene loadScene;


    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
        loadScene.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
    }

    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
        isShopClosed = false;
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
