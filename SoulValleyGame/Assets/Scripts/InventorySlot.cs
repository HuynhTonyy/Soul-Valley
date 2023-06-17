using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject droppedItem = eventData.pointerDrag;
            InventoryItem draggableItem = droppedItem.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }

    }
}
