using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private ShopSlot _assignedItemSlot;
    public ShopKeeperDisplay ParentDisplay { get; private set; }

    public float BuyMarkUp { get; private set; }
    public float SellMarkUp { get; private set; }

    private double SellPrice;
    private double BuyPrice;

    private int _tempAmount;

    float randomSellP, randomBuyP;

    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemName.text = "";
        _itemCount.text = "";
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlot slot, float buyMarkUp, float sellMarkUp)
    {
        _assignedItemSlot = slot;
        BuyMarkUp = buyMarkUp;
        SellMarkUp = sellMarkUp;
        _tempAmount = slot.StackSize;
        
        UpdateUISlott();
    }
    public void UpdateUISlott()
    {
        if(_assignedItemSlot.ItemData != null)
        {           
            SellPrice = Mathf.RoundToInt(_assignedItemSlot.ItemData.Value + _assignedItemSlot.ItemData.Value * SellMarkUp);
            BuyPrice = Mathf.RoundToInt(_assignedItemSlot.ItemData.Value + _assignedItemSlot.ItemData.Value * BuyMarkUp);
            _itemSprite.sprite = _assignedItemSlot.ItemData.icon;
            _itemSprite.color = Color.white;
            _itemCount.text = 'x' + _assignedItemSlot.StackSize.ToString();
            _itemName.text = _assignedItemSlot.ItemData.DisplayName;
            
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.preserveAspect = true;
            _itemSprite.color = Color.clear;
            _itemName.text = "";
            _itemCount.text = "";
        }
    }

    public void OnItemClick()
    {
        Debug.Log(BuyMarkUp);
        if (_assignedItemSlot.ItemData != null)
        {
            string buyStr = "Buy Price: " + BuyPrice;
            string selltr = "Sell Price: " + SellPrice;
            ParentDisplay.SetCurSelectedItem(_assignedItemSlot.ItemData, BuyPrice, SellPrice);
            ParentDisplay.SetItemPreview(_assignedItemSlot.ItemData.icon, _assignedItemSlot.ItemData.DisplayName, _assignedItemSlot.ItemData.Description, buyStr, selltr);
        }
    }

    
}
