using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour, IPointerClickHandler
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
            _itemCount.text = _assignedItemSlot.StackSize.ToString();
            _itemName.text = _assignedItemSlot.ItemData.DisplayName;
            _itemSPrice.text = SellPrice.ToString();
            _itemBPrice.text = SellPrice.ToString();
            
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
    
   

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_assignedItemSlot.ItemData != null)
        {
            string buyStr = "Buy Price: " + BuyPrice;
            string selltr = "Sell Price: " + SellPrice;
            ParentDisplay.SetItemPreview(_assignedItemSlot.ItemData.icon, _assignedItemSlot.ItemData.DisplayName, _assignedItemSlot.ItemData.Description, buyStr, selltr);
        }
    }
}
