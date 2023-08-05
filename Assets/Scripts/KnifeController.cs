using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public bool enemyInRangeOfAttack;

    // Checks if enemy is in range of attack
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PatrolAI enemy = collision.gameObject.GetComponent<PatrolAI>();
        
        if (enemy != null)
        {
            enemyInRangeOfAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyInRangeOfAttack = false;   
    }
}
