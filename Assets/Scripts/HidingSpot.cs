using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool playerIsHiddenHere;
    Vector3 lastPlayerPosition;

    public void ToggleHidding(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller != null)
        {
            lastPlayerPosition = controller.gameObject.transform.position;

            if (!playerIsHiddenHere)
            {
                HidePlayerInHere();
                // Hide the player in this gameObject position
                controller.HidePlayer(transform.position);

            }
            else if (playerIsHiddenHere)
            {
                PlayerExitHideSpot();
                controller.UnHidePlayer();
            }
        }
    }

    private void HidePlayerInHere()
    {
        playerIsHiddenHere = true;
        // show hidden animation
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        Debug.Log("Player hid in " + gameObject.name);
    }

    private void PlayerExitHideSpot()
    {
        playerIsHiddenHere = false;
        Debug.Log("Player came out of " + gameObject.name);
        // show hidden animation
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

    }

}
