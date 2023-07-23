using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : BaseInteractionItem
{
    void Start()
    {
        interactionText = "Press E to Hide";
    }

    public override void ShowInteractionText()
    {
        base.ShowInteractionText();
    }

    public override void Interact()
    {
        Debug.Log("Hiding in " + gameObject.name);
    }

}
