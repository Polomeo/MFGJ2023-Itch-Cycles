using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingSpot : BaseInteractionItem
{
    void Start()
    {
        interactionText = "Press E to Search";
    }

    public override void ShowInteractionText()
    {
        base.ShowInteractionText();
    }

    public override void Interact()
    {
        Debug.Log("Interacting with Searching Spot");
    }

}
