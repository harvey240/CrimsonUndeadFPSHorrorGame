using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //Message displayed to player when looking at interactable
    [Tooltip("Leave this blank for pickup items - as it is set automatically")]
    public string promptMessage;

    // called by the player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // Wont have code here - this is a template function that will be overridden by subclasses
        // Using the template method design pattern.
    }
}
