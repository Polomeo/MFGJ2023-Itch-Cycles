using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSpot : MonoBehaviour
{
    [SerializeField] private GameObject ladderSpotConnected;
    [SerializeField] private float ladderSpeed = 3f;

    private GameObject player;
    private bool playerIsClimbingHere;
    private bool interactionEnabled = true;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (playerIsClimbingHere)
        {
            ClimbToTheOtherEnd();
        }
    }

    // Player press E in collider
    public void ClimbingLadder()
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        
        if(controller != null && !playerIsClimbingHere)
        {
            // Player moves to the transform.x of the ladder
            // Player deactivates its movement and control
            controller.IsClimbing(transform.position);

            playerIsClimbingHere = true;
        }
    }

    private void ClimbToTheOtherEnd()
    {
        if(player.transform.position != ladderSpotConnected.transform.position)
        {
            // Move towards the other end of the ladder
            player.transform.position = Vector2.MoveTowards(player.transform.position,
                    ladderSpotConnected.transform.position, ladderSpeed * Time.deltaTime);

            // Deactivate the Interaction for both sides
            if (interactionEnabled)
            {
                gameObject.GetComponent<Interactable>().enabled = false;
                ladderSpotConnected.GetComponent<Interactable>().enabled = false;
                interactionEnabled = false;
            }
            
            Debug.Log("Climbing");
        }
        else
        {
            // Re-activate the interaction for both sides
            gameObject.GetComponent<Interactable>().enabled = true;
            ladderSpotConnected.GetComponent<Interactable>().enabled = true;
            interactionEnabled = true;

            // Inform the Player Controller
            playerIsClimbingHere = false;
            player.GetComponent<PlayerController>().DoneClimbing();


            Debug.Log("Done climbing!");
        }
    }
}
