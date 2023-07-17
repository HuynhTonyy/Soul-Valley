using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IIntractable
{
    /*public UnityAction<IInteractable> OnInteractableComplete { get; set; }*/

    public void Interact(Interactor interactor);

    /*public void EndInteraction();*/
}
