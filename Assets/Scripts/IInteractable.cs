using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string interactionText { get; }
    public void Interact();
}
