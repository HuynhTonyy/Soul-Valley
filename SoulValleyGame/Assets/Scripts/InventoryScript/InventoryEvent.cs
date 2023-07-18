using System;

public class InventoryEvent
{
    public event Action<ItemScript, int> onAddItem;
    public event Action<ItemScript, int> onRemoveItem;
    public void AddItem(ItemScript itemData, int amount){
        if(onAddItem != null){
            onAddItem(itemData, amount);
        }
    }
    public void RemoveItem(ItemScript itemData, int amount){
        if(onRemoveItem != null){
            onRemoveItem(itemData, amount);
        }
    }
}
