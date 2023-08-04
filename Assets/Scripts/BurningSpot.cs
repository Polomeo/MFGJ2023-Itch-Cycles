using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSpot : MonoBehaviour
{
    public bool dollPlaced;
    [SerializeField] private Animator animator;

    private GameObject player;
    [SerializeField] private List<GameObject> dolls;

    private void Start()
    {
        // Component
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        // Turn off all dolls
        HideDolls();
        dollPlaced = false;
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
    }
}
