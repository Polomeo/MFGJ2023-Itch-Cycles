using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    // Waypoints
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float minWaypointDistance = 0.5f;
    [SerializeField] private int currentWaypoint;
    [SerializeField] private float chaseSpeed = 5f;

    // Ladders
    [SerializeField] private GameObject[] ladderSpots;
    [SerializeField] private GameObject closerLadderSpot;
    [SerializeField] private GameObject secondCloserLadderSpot;
    [SerializeField] private float distance;
    [SerializeField] private float nearestDistance = 10000;

    // Conditions
    private bool isWaiting;
    private bool needClimbing;
    private bool isClimbing;
    private bool playerSpotted;

    // Components
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

    // Manny' s behaviour rework

    // isLadderNeeded? (nextWaypoint.y != currentWaypoint.y)

    // True: go to closer ladder, Rigidbody kinematic, move to the other side of ladder, Rigidbody dynamic, isLadderNeeded false

    // False: go to waypoint and wait

    // Informs the Game Manager the current enemy room
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            GameManager.Instance.SetEnemyRoom(collision.gameObject.name);
        }
    }
    public void PlayerIsSpotted()
    {
        playerSpotted = true;
    }
    private void Patrol()
    {
        if (needClimbing)
        {
            ClimbLadder();
        }
        
        else
        {
            // if Waypoint is on the same floor
            float distanceToWaypoint = Math.Abs(waypoints[currentWaypoint].position.x - transform.position.x);

            // If not in current waypoint X position, go to it
            if (distanceToWaypoint > minWaypointDistance)
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
    private IEnumerator WaitInRoom()
    {
        isWaiting = true;

        // Wait for a few seconds in the room
        yield return new WaitForSeconds(waitTime);
        
        // Set the next waypoint
        currentWaypoint++;
        
        // Reset to the first waypoint
        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }

        isWaiting = false;

        // check if is a ladder is needed for the next waypoint
        IsLadderNeeded();


        // [TO IMPLEMENT] Start wandering sound
    }

    private void IsLadderNeeded()
    {
        // If the last waypoint has a different tag than the next, it means they are in different floors
        if(currentWaypoint != 0 && waypoints[currentWaypoint - 1].gameObject.tag != waypoints[currentWaypoint].gameObject.tag)
        {
            Debug.Log("Climbing needed");
            
            // Select closer ladder spot and the next closer to it
            for (int i = 0; i < ladderSpots.Length; i++)
            {
                distance = Vector3.Distance(transform.position, ladderSpots[i].transform.position);

                if (distance < nearestDistance)
                {
                    // sets the former closer one in the second closer one position
                    secondCloserLadderSpot = closerLadderSpot;

                    // sets the closer one
                    closerLadderSpot = ladderSpots[i];

                    // sets the distance
                    nearestDistance = distance;
                }
            }
            
            needClimbing = true;
        }
    }

    private void ClimbLadder()
    {
        // Move towards the closest ladder point X axis position
        transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(closerLadderSpot.transform.position.x, transform.position.y), speed * Time.deltaTime);

        // When the ladder is reached
        if(transform.position.x == closerLadderSpot.transform.position.x)
        {
            isClimbing = true;
            rb.isKinematic = true;
        }

        if(isClimbing)
        {
            // Move towards Y axis position of the second closed ladder (the other end of the current ladder)
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(closerLadderSpot.transform.position.x, secondCloserLadderSpot.transform.position.y), speed * Time.deltaTime);

            // if reached the end of the ladder
            if(transform.position.y == secondCloserLadderSpot.transform.position.y)
            {
                // Return to normal
                closerLadderSpot = null;
                secondCloserLadderSpot = null;
                distance = 0;
                nearestDistance = 10000;
                rb.isKinematic = false;
                needClimbing = false;
                isClimbing = false;
            }
        }
    }



  

    
}
