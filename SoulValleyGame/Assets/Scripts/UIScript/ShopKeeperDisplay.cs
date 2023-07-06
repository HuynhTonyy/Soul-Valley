using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI _shopSlotPrefab;

    [Header("Shopping Func")]
    [SerializeField] private Button _buyBtn;
    [SerializeField] private Button _sellBtn;

    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI _itemPreviewName;
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;
    [SerializeField] private TextMeshProUGUI _itemBuyPrice;
    [SerializeField] private TextMeshProUGUI _itemSellPrice;

    [SerializeField] private GameObject _itemListContentPanel;

    public GameObject BuyTabDisplay;
    public GameObject SellTabDisplay;

    private ItemScript curSelectedItemData;
    private double itemBuyPrice;

    private ShopSystem _shopSystem;
    private PlayerInventoryHolder _playerInventoryHolder;


    public void DisplayShowWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventoryHolder)
    {
        _shopSystem = shopSystem;
        _playerInventoryHolder = playerInventoryHolder;

        RefreshDisplay();
        DisplayShopInventory();
    }

    private void RefreshDisplay()
    {
        ClearSlots();
        _itemPreviewSprite.sprite = null;
        _itemPreviewSprite.color = Color.clear;
        _itemPreviewName.SetText("");
        _itemPreviewDescription.SetText("");
        _itemBuyPrice.SetText("");
        _itemSellPrice.SetText("");
        _buyBtn.gameObject.SetActive(false);
        _sellBtn.gameObject.SetActive(false);
        _buyBtn.onClick.AddListener(BuyItems);
    }
    public void SetItemPreview(Sprite img,string itemName, string itemDes, string itemBPrice, string itemSPrice)
    {
        _itemPreviewSprite.sprite = img;
        _itemPreviewSprite.color = Color.white;
        _itemPreviewName.SetText(itemName);
        _itemPreviewDescription.SetText(itemDes);
        _itemBuyPrice.SetText(itemBPrice);
        _itemSellPrice.SetText(itemSPrice);
        _buyBtn.gameObject.SetActive(true);
        _sellBtn.gameObject.SetActive(true);
    }
    private void ClearSlots()
    {
        foreach(var item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

    }

    private void DisplayShopInventory()
    {
        foreach(var item in _shopSystem.ShopInventory)
        {
            if (item.ItemData == null) continue;

            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    public void SetCurSelectedItem(ItemScript item, double buyPrice)
    {
        curSelectedItemData = item;
        itemBuyPrice = buyPrice;
    }

    private void BuyItems()
    {
        if (PlayerStats.playerSoulCoin < (int)itemBuyPrice) return;
        if (!_playerInventoryHolder.PrimaryInventorySystem.HasFreeSlot(out InventorySlot freeslot)) return;

        if (_shopSystem.PurchaseItem(curSelectedItemData, 1))
        {
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(curSelectedItemData, 1);
            PlayerStats.SpendCoin((int)itemBuyPrice);
            _shopSystem.GainGoid((int)itemBuyPrice);
        };
        
        ClearSlots();
        DisplayShopInventory();
    }

    
}
