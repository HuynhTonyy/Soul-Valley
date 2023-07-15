using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UniqueID))]

public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private ShopSystem _shopSystem;

    private PlayerInventoryHolder playerInv;


    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;
    public void Interact(Interactor interactor)
    {
        playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
        }
    }
    
    private void Awake()
    {
        _shopSystem = new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, _shopItemsHeld.BuyMarkUp, _shopItemsHeld.SellMarkUp);

        foreach(var items in _shopItemsHeld.Items)
        {
            //Debug.Log($"{items.itemData.DisplayName}: {items.Amount}");
            _shopSystem.AddToShop(items.itemData, items.Amount);
        }

    }

}
