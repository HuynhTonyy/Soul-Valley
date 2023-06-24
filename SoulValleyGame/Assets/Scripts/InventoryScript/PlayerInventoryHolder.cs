using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secoundaryInventorySize;
    [SerializeField] protected InventorySystem secoundaryInventorySystem;


    public InventorySystem SecoundaryInventorySystem => secoundaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;

    protected override void Awake()
    {
        base.Awake();
        secoundaryInventorySystem = new InventorySystem(secoundaryInventorySize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            OnPlayerBackpackDisplayRequested?.Invoke(secoundaryInventorySystem);
        }

    }
    public bool AddToInventory(ItemScript item, int amount)
    {
        if (primaryInventorySystem.AddToInventory(item, amount))
        {
            return true;
        }
        else if(secoundaryInventorySystem.AddToInventory(item, amount))
        {
            return true;
        }
        return false;
    }
}
