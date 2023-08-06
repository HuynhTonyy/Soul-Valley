using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;

[RequireComponent(typeof(UniqueID))]

public class ShopKeeper : MonoBehaviourPunCallbacks, IIntractable, ITimeTracker
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    private ShopItemList _shopItemsHeldClone;
    [SerializeField] private ShopSystem _shopSystem;
    private PlayerInventoryHolder playerInv;
    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;
    public static UnityAction<int,int,int> OnShopChanged;
    public bool isInAction = false;

    public void Interact(Interactor interactor)
    {
        playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null && !isInAction)
        {
            interactor.shopKeeper= this.gameObject;
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
            photonView.RPC("UpdateShopState", RpcTarget.AllBufferedViaServer);
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
    
    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
        _shopSystem = new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, _shopItemsHeld.BuyMarkUp, _shopItemsHeld.SellMarkUp);
        _shopItemsHeldClone = _shopItemsHeld;
        foreach (var items in _shopItemsHeld.Items)
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

    [PunRPC]
    public void UpdateShopState()
    {
       this.isInAction = true;
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if(timeStamp.hour == 10)
        {
            RefreshShop();
        }
    }

    private void RefreshShop()
    {
        int index = 0;
        foreach (var items in _shopItemsHeld.Items)
        {
            Debug.Log(items.Amount);
            _shopSystem.ShopInventory[index].setStackSize(items.Amount);
            index += 1;
        }
    }

    
}

