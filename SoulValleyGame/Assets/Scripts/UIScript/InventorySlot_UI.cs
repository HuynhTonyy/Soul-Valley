using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignInventorySlot;

    private Button btn;

    public InventorySlot AssignInventorySlot => assignInventorySlot;
    public InventoryDisplay ParentDisplay {get;private set;}
    private void Awake()
    {
        ClearSlot();

        btn = GetComponent<Button>();
        btn?.onClick.AddListener(OnUISlotClick);

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
            ClearSlot();
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
    public void OnUISlotClick()
    {
        // access display class function
        ParentDisplay?.SlotCliked(this);
    }

}
