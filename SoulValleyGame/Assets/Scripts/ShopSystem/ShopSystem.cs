using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]

public class ShopSystem
{
    [SerializeField] private List<ShopSlot> _shopInventory;
    [SerializeField] private int _availableGold;
    [SerializeField] private float _buyMarkUp;
    [SerializeField] private float _sellMarkUp;

    public List<ShopSlot> ShopInventory => _shopInventory;

    public float BuyMarkUp => _buyMarkUp;
    public float SellMarkUp => _sellMarkUp;
    public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
        _availableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);

        for(int i = 0; i < size; i++)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }

    public void AddToShop(ItemScript data, int amount)
    {
        // If the item already exists in the shop, it increases the quantity of the item by the specified amount.
        if (ContainsItem(data, out ShopSlot shopSlot)) 
        {
            shopSlot.AddToStack(amount);
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);    
        // If the item does not exist or there is no free slot available, it assigns the item to a new slot with the specified amount.
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.FirstOrDefault(i => i.ItemData == null); // find the first slot where the slots data == null

        if(freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }
        //Always get a free slot
        return freeSlot;
    }

    public bool ContainsItem(ItemScript itemToAdd, out ShopSlot shopSlot)// any slot have item to add 
    {
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd); // check if the item already in the shop => get the shop slot the item currently in
        //Debug.Log(invSlot.Count);
        return shopSlot != null;
    }
}
