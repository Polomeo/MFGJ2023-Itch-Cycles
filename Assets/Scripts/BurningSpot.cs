using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSpot : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void BurnDoll()
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        
        // if Player has a doll in hand
        if (controller != null && controller.isHoldingADoll)
        {
            controller.BurningDoll();
        }
    }
}
