using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.Events;

public class InventorySlot_UI : MonoBehaviour,IPointerClickHandler
{
    public Image itemSprite;
    public TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignInventorySlot;

    private Button btn; 
    public InventorySlot AssignInventorySlot => assignInventorySlot;
    public InventoryDisplay ParentDisplay {get;private set;}

    public Image image;
    public Color selectedColor, notSelectedColor;
    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onMiddleClick;
    public void Selected()
    {
        image.color = selectedColor;
    }
    public void Deselected()
    {
        image.color = notSelectedColor;
    }

    private void Awake()
    {
        Deselected();
        ClearSlot();

        itemSprite.preserveAspect = true;

        // btn = GetComponent<Button>();

        // btn?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>(); 

    }

    public void Init(InventorySlot slot)
    {
        assignInventorySlot = slot;
        UpdateUISlot(slot);
    }
    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.icon;
            itemSprite.color = Color.white;
            if (slot.StackSize > 1)
            {
                itemCount.text = slot.StackSize.ToString();
            }
            else itemCount.text = "";
        }
        else
        {
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemCount.text = "";
        }
    }
    public void UpdateUISlot()
    {
        if (assignInventorySlot != null) UpdateUISlot(assignInventorySlot);
    }
    public void ClearSlot()
    {
        assignInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }
    public void OnUISlotClick(int num)
    {
        // access display class function
        ParentDisplay?.SlotCliked(this,num);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnUISlotClick(0);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
          
            OnUISlotClick(1);
        }
    }
}
