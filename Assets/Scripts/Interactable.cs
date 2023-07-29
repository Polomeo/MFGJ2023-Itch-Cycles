using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    // based on video
    // How to INTERACT with Game Objects using UNITY EVENTS Tutorial
    // by BMo

    // This script is attached to the object with a Collider2D
    public bool isInRange;
    public KeyCode interactKey;
    public string interactText;
    public UnityEvent interactAction;

    private PlayerController playerController;

    // When enters the trigger, sets "isInRange" to True
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object collides with the player, and the player hasn't been found
        
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            
            // UI
            UIManager.Instance.DisplayInteractionText(interactText);
        }
    }

    // When exits trigger, "isInRange" turns False
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;

            // UI
            UIManager.Instance.HideText();
        }
    }

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        // If in range and not found, press the key to invoke the listeners (set in inspector)
        if (isInRange && !playerController.hasBeenFound)
        {
            if(Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
            }
        }
    }
}
