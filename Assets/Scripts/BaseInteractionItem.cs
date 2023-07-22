using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractionItem : MonoBehaviour
{
    protected string interactionText;

    public virtual void ShowInteractionText()
    {
        Debug.Log(interactionText);
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting");
    }
}
