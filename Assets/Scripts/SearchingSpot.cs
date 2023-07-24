using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingSpot : MonoBehaviour
{
    public bool playerIsSearchingHere = false;
    public bool alreadySearch = false;
    [SerializeField] float searchTime = 3f;

    public void SearchInSpot(GameObject player) 
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (!playerIsSearchingHere && !alreadySearch)
            {
                StartCoroutine(Searching(controller));
                playerIsSearchingHere = true;
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
        // [PLACEHOLDER] Change to search animation
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        
        // Tell the player that "is searching" and pass this GameObject position
        controller.IsSearching(transform.position);

        // Wait for the search to finish
        yield return new WaitForSeconds(searchTime);

        // [PLACEHOLDER] Change to "already searched" sprite
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        Debug.Log("Search complete.");
        
        // Tell the player that the searching is done
        controller.DoneSearching();

        playerIsSearchingHere = false;
        alreadySearch = true;
    }
}
