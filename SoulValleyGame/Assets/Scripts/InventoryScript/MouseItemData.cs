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
    public float dropOffset = 3f;
    private Transform playerTransform;
    public int previousIndex;

    public void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";

        playerTransform = GetComponentInParent<Interactor>().transform;
        if(playerTransform == null)
        {
            Debug.Log("player Not Found");
        }
    }
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }
    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignInventorySlot.ItemData.icon;
        if (AssignInventorySlot.StackSize.ToString() == "1")
        {
            ItemCount.text = "";
        }
        else ItemCount.text = AssignInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    public void Update()
    {
        if(AssignInventorySlot.ItemData != null) // if has item - follow mouse
        {
            transform.position = Mouse.current.position.ReadValue();
            if(Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if(AssignInventorySlot.ItemData.ItemPreFab != null)
                {
                    Instantiate(AssignInventorySlot.ItemData.ItemPreFab, 
                        playerTransform.position + playerTransform.forward * dropOffset, Quaternion.identity);
                }
                if (AssignInventorySlot.StackSize > 1)
                {
                    AssignInventorySlot.AddToStack(-1);
                    UpdateMouseSlot(); 
                }
                else
                {
                    ClearSlot();
                }
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
    public static bool IsPointerOverUIObject()// 
    {
        PointerEventData evenDataCurrentPosition = new PointerEventData(EventSystem.current);
        evenDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(evenDataCurrentPosition, results);
        return results.Count > 0;
    }
}

