using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SearchingSpot : MonoBehaviour
{
    // Search logic
    public bool playerIsSearchingHere = false;
    public bool alreadySearch = false;
    [SerializeField] float searchTime = 3f;
    [SerializeField] Sprite alreadySearchSprite;
    [SerializeField] bool thisSpotHasADoll;

    private GameObject player;

    // Audio
    private AudioSource audioSource;
    [SerializeField] AudioClip interactionSound;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = player.GetComponent<AudioSource>();
    }

    public void SearchInSpot() 
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (!playerIsSearchingHere && !alreadySearch && !controller.isHoldingADoll)
            {
                StartCoroutine(Searching(controller));
                playerIsSearchingHere = true;
                
                // Audio
                if(interactionSound != null)
                {
                    audioSource.PlayOneShot(interactionSound);
                }
            }

            if (alreadySearch)
            {
                Debug.Log("Spot already searched!");
            }
        }
    }

    IEnumerator Searching(PlayerController controller)
    {
        Debug.Log("Searching...");
        
        // Tell the player that "is searching" and pass this GameObject position
        controller.IsSearching(transform.position);

        // Wait for the search to finish
        yield return new WaitForSeconds(searchTime);

        // Change to "already searched" sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = alreadySearchSprite;

        // If this searching spot has a doll, give it to the player
        if (thisSpotHasADoll)
        {
            controller.DollFound();
        }

        Debug.Log("Search complete.");
        
        // Tell the player that the searching is done
        controller.DoneSearching();

        playerIsSearchingHere = false;
        alreadySearch = true;
    }
}
