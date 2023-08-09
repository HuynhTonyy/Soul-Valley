using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class well : MonoBehaviour, IIntractable
{
    FMOD.ATTRIBUTES_3D attributes;
    private EventInstance fillWaterSound;
    private void Start()
    {
        fillWaterSound = AudioManager.instance.CreateInstance(FMODEvents.instance.fillWaterBucketSound);
        attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(transform.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(transform.up);
        fillWaterSound.set3DAttributes(attributes);
    }
    public void Interact(Interactor interactor)
    {
        ToolData tool = interactor.hotBar.GetToolData(interactor.hotBar.selectedSlot);
        if (tool.toolType == ToolData.ToolType.WateringCan)
        {
            fillWaterSound.start();
            tool.currentDurability = tool.maxDurability;
            GameEventManager.instance.inventoryEvent.UseItem(tool.Id,UseQuestStep.UseType.reNew);
        }
    }
}
