using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secoundaryInventorySize;
    [SerializeField] protected InventorySystem secoundaryInventorySystem;

    public InventorySystem SecoundaryInventorySystem => secoundaryInventorySystem;
    protected override void Awake()
    {
        base.Awake();

        secoundaryInventorySystem = new InventorySystem(secoundaryInventorySize);
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.iKey.wasPressedThisFrame) OnDynamicInventoryDisplayRequested?.Invoke(secoundaryInventorySystem);
    }
}
