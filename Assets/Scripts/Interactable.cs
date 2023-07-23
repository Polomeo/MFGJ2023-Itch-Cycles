using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // based on video
    // How to INTERACT with Game Objects using UNITY EVENTS Tutorial
    // by BMo

    // This script is attached to the object with a Collider2D
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;

    // When enters the trigger, sets "isInRange" to True
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player enter range of " +  gameObject.name);
        }
    }

    // When exits trigger, "isInRange" turns False
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player exit range of " + gameObject.name);
        }
    }

    private void Update()
    {
        // If in range, and press the key, invoke the listeners (set in inspector)
        if (isInRange)
        {
            if(Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
            }
        }
    }
}
