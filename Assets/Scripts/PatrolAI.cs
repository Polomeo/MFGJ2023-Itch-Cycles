using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private Transform[] waypoints;

    [SerializeField] private float chaseSpeed = 5f;

    [SerializeField] private int currentWaypoint;

    private bool isWaiting;
    private bool playerSpotted;

    void Update()
    {
        if(playerSpotted && GameManager.Instance.isGameActive)
        {
            Vector3 player = GameObject.FindWithTag("Player").transform.position;

            if (transform.position != player)
            {
                // Run to player
                transform.position = Vector2.MoveTowards(transform.position,
                    player, chaseSpeed * Time.deltaTime);
            }
            else
            {
                GameManager.Instance.GameOver();
            }

        }
        else if (GameManager.Instance.isGameActive)
        {
            Patrol();
        }

    }

    public void PlayerIsSpotted()
    {
        playerSpotted = true;
    }

    private IEnumerator WaitInRoom()
    {
        isWaiting = true;

        // Wait for a few seconds in the room
        yield return new WaitForSeconds(waitTime);
        
        // Set the next waypoint
        currentWaypoint++;
        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }
        isWaiting = false;

        // [TO IMPLEMENT] Start wandering sound
    }

    // Informs the Game Manager the current enemy room
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            GameManager.Instance.SetEnemyRoom(collision.gameObject.name);
        }
    }

    private void Patrol()
    {
        // If not in current waypoint, go to it
        if (transform.position != waypoints[currentWaypoint].position)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                waypoints[currentWaypoint].position, speed * Time.deltaTime);
        }
        else if (!isWaiting)
        {
            StartCoroutine(WaitInRoom());
        }
    }

  

    
}
