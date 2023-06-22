using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignInventorySlot;

    public void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignInventorySlot.AssignItem(invSlot);
        ItemSprite.sprite = invSlot.ItemData.icon;
        if (invSlot.StackSize.ToString() == "1")
        {
            ItemCount.text = "";
        }
        else ItemCount.text = invSlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }
    public void Update()
    {
        if(AssignInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();
            if(Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot();
            }

        }
    }

    public void ClearSlot()
    {
        AssignInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData evenDataCurrentPosition = new PointerEventData(EventSystem.current);
        evenDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(evenDataCurrentPosition, results);
        return results.Count > 0;

    }
}

