using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractionItem : MonoBehaviour
{
    protected string interactionText;
    protected bool displayingInteractionText = false;

    public virtual void ShowInteractionText()
    { 
        if(!displayingInteractionText)
        {
            Debug.Log(interactionText);
            displayingInteractionText = true;
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting");
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null)
        {

        }
    }
}
