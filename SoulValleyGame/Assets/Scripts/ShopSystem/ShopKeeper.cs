using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;

[RequireComponent(typeof(UniqueID))]

public class ShopKeeper : MonoBehaviourPunCallbacks, IIntractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private ShopSystem _shopSystem;

    private PlayerInventoryHolder playerInv;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;
    public static UnityAction<int,int,int> OnShopChanged;
    public void Interact(Interactor interactor)
    {
        playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
        }
    }
    

    private void OnEnable()
    {
        ShopKeeper.OnShopChanged += ShopChanged;
    }

    private void OnDisable()
    {
        ShopKeeper.OnShopChanged -= ShopChanged;
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
    private void ShopChanged(int index,int amount,int gold)
    {
        photonView.RPC("UpdateShop", RpcTarget.AllBufferedViaServer,index,amount,gold);
    }

    [PunRPC]
    public void UpdateShop(int index,int amount,int gold )
    {
       _shopSystem.ShopInventory[index].setStackSize(amount);
       _shopSystem.setGold(gold);

    }

    

}

