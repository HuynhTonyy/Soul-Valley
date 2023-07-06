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
    [SerializeField] private TextMeshProUGUI _itemBPrice;
    [SerializeField] private TextMeshProUGUI _itemSPrice;
    [SerializeField] private ShopSlot _assignedItemSlot;
    public ShopKeeperDisplay ParentDisplay { get; private set; }

    public float MarkUp { get; private set; }

    private double SellPrice;
    private double BuyPrice;

    private int _tempAmount;

    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemName.text = "";
        _itemCount.text = "";
        _itemBPrice.text = "";
        _itemSPrice.text = "";
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    public void Init(ShopSlot slot, float markUp)
    {
        _assignedItemSlot = slot;
        MarkUp = markUp;
        _tempAmount = slot.StackSize;
        UpdateUISlott();
    }

    public void UpdateUISlott()
    {
        if(_assignedItemSlot.ItemData != null)
        {
            
            SellPrice = _assignedItemSlot.ItemData.Value + _assignedItemSlot.ItemData.Value * 0.25;
            BuyPrice = _assignedItemSlot.ItemData.Value + _assignedItemSlot.ItemData.Value * 0.5;
            _itemSprite.sprite = _assignedItemSlot.ItemData.icon;
            _itemSprite.color = Color.white;
            _itemCount.text = 'x' + _assignedItemSlot.StackSize.ToString();
            _itemName.text = _assignedItemSlot.ItemData.DisplayName;
            _itemSPrice.text = SellPrice.ToString();
            _itemBPrice.text = BuyPrice.ToString();
            
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.preserveAspect = true;
            _itemSprite.color = Color.clear;
            _itemName.text = "";
            _itemCount.text = "";
            _itemBPrice.text = "";
            _itemSPrice.text = "";
        }
        
    }
    
   

    public void OnItemClick()
    {
        if (_assignedItemSlot.ItemData != null)
        {
            Debug.Log("CLicked");
            string buyStr = "Buy Price: " + BuyPrice;
            string selltr = "Sell Price: " + SellPrice;
            ParentDisplay.SetCurSelectedItem(_assignedItemSlot.ItemData, BuyPrice);
            ParentDisplay.SetItemPreview(_assignedItemSlot.ItemData.icon, _assignedItemSlot.ItemData.DisplayName, _assignedItemSlot.ItemData.Description, buyStr, selltr);
        }
    }

    
}
