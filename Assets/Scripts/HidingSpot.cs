using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool playerIsHiddenHere;
    [SerializeField] Sprite nonHideSprite;
    [SerializeField] Sprite hideSprite;

    public void ToggleHidding(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller != null)
        {
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

        // [PLACEHOLDER] show hidden animation
        gameObject.GetComponent<SpriteRenderer>().sprite = hideSprite;

        Debug.Log("Player hid in " + gameObject.name);
    }

    private void PlayerExitHideSpot()
    {
        playerIsHiddenHere = false;
        Debug.Log("Player came out of " + gameObject.name);
        // show hidden animation
        gameObject.GetComponent<SpriteRenderer>().sprite = nonHideSprite;

    }

}
