using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool playerIsHiddenHere;
    [SerializeField] Sprite nonHideSprite;
    [SerializeField] Sprite hideSprite;

    private GameObject player;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

    }

    public void ToggleHidding()
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
        animator.SetBool("b_PlayerIsHiddenHere", playerIsHiddenHere);
    }

    private void PlayerExitHideSpot()
    {
        playerIsHiddenHere = false;
        animator.SetBool("b_PlayerIsHiddenHere", playerIsHiddenHere);
    }

}
