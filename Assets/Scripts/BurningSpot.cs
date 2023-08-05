using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSpot : MonoBehaviour
{
    public bool dollPlaced;

    private GameObject player;
    private Interactable interactable;
    [SerializeField] private List<GameObject> dolls;
    [SerializeField] private Animator animator;

    private string burnDollText = "Press E to Put Doll";
    private string pickKnifeText = "Press E to Take Knife";

    private void Start()
    {
        // Component
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();

        // Turn off all dolls
        HideDolls();
        dollPlaced = false;

        // Set starting text
        interactable.SetInteractText(burnDollText);

    }

    public void ReciveDoll()
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null && controller.isHoldingADoll == true && !dollPlaced)
        {
            if (controller != null)
            {
                controller.PutDollInPedestal();
            }

            // Reveal recieved doll
            for (int i = 0; i < dolls.Count; i++)
            {
                if (dolls[i].name == controller.dollInHandName)
                {
                    dolls[i].GetComponent<Renderer>().enabled = true;
                }
            }

            dollPlaced = true;
            GameManager.Instance.CheckIfAllDollsArePlaced();
        }
        else
        {
            Debug.Log("This place already has a doll or the player does not have one in hand.");
        }
    }

    public void HideDolls()
    {
        foreach (GameObject o in dolls)
        {
            o.GetComponent<Renderer>().enabled = false;
        }
    }

    public void BurnDoll()
    {
        HideDolls();
        animator.SetTrigger("t_BurnDoll");

        // Set Pick up knife Text
        interactable.SetInteractText(pickKnifeText);
    }
}
