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
    [SerializeField] private Sprite idleSprite;

    private string burnDollText = "Press E to Place Doll";
    private string pickKnifeText = "Press E to Take Knife and Attack";

    private bool dollsBurn;

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
        StartCoroutine(playBurningAnimation());
    }

    IEnumerator playBurningAnimation()
    {
        animator.SetTrigger("t_BurnDoll");
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(animationLength);
        
        // Set Pick up knife Text
        interactable.SetInteractText(pickKnifeText);
        dollsBurn = true;
    }

    public void GiveKnifeToPlayer()
    {
        if(dollsBurn)
        {
            // Give the knife to the player
            player.GetComponent<PlayerController>().GetKnife();

            // Disable renderer
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // Deactivate the interaction
            GameManager.Instance.RemoveInteraction();
        }
    }
}
