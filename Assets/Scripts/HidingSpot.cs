using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HidingSpot : MonoBehaviour
{
    public bool playerIsHiddenHere;

    private GameObject player;
    private Animator animator;

    // Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip hiddingClip;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        
        // Audio
        if(hiddingClip != null)
        {
            audioSource.PlayOneShot(hiddingClip);
        }
    }

    private void PlayerExitHideSpot()
    {
        playerIsHiddenHere = false;
        animator.SetBool("b_PlayerIsHiddenHere", playerIsHiddenHere);
    }

}
