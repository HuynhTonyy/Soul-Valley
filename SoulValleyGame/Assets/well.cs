using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class well : MonoBehaviour, IIntractable
{
    public void Interact(Interactor interactor)
    {
        ToolData tool = interactor.hotBar.GetToolData(interactor.hotBar.selectedSlot);
        if (tool.toolType == ToolData.ToolType.WateringCan)
        {
            tool.currentDurability = tool.maxDurability;
        }
    }
}
