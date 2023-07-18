using System;

public class InventoryEvent
{
    public event Action<ItemScript, int> onAddItem;
    public void AddItem(ItemScript itemData, int amount){
        if(onAddItem != null){
            onAddItem(itemData, amount);
        }
    }
}
