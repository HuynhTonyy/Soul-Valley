using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;

    [HideInInspector] public ItemScript item;
    [HideInInspector] public int itemCount = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitializeItem(ItemScript newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        Count();
    }

    public void Count()
    {
        countText.text = itemCount.ToString();
        bool textActive = itemCount > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Draging");
        transform.position = Input.mousePosition;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
