using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlotClickHandler : MonoBehaviour, IPointerClickHandler
{

    private ShopSlotUI _shopSlotUI;
    void Awake()
    {
        _shopSlotUI = GetComponent<ShopSlotUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _shopSlotUI.OnItemClick();
    }
   
}
