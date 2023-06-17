using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image item;
    public Color selected, notSelected;

    public void Awake()
    {
        NotSelect();
    }

    public void Select()
    {
        item.color = selected;
    }
    public void NotSelect()
    {
        item.color = notSelected;
    }

    // drag and drop
    public void OnDrop(PointerEventData eventData){
        if (transform.childCount == 0)
        {
            GameObject droppedItem = eventData.pointerDrag;
            InventoryItem draggableItem = droppedItem.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }

    }
}
