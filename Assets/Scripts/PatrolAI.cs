using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PatrolAI : MonoBehaviour
{
    // Waypoints
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float totalSpeed; // speed + 10% for each doll burned
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float minWaypointDistance = 0.5f;
    [SerializeField] private int currentWaypoint;
    [SerializeField] private int nextWaypoint;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float jumpAttackDistance = 3f;

    // Ladders
    [SerializeField] private GameObject[] ladderSpots;
    [SerializeField] private GameObject closerLadderSpot;
    [SerializeField] private GameObject secondCloserLadderSpot;
    [SerializeField] private float distance;
    [SerializeField] private float nearestDistance = 10000;

    // Conditions
    [Header("Conditions")]
    [SerializeField] private bool isWaiting;
    [SerializeField] private bool needClimbing;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool playerSpotted;
    [SerializeField] private bool isAttacking;
    public bool isEscapingFromPlayer;

    // Components
    private Rigidbody2D rb;
    private Animator animator;

    // Audio
    [Header("Audio")]
    [SerializeField] AudioClip caughtLaughSFX;
    [SerializeField] AudioClip startPatrollingSFX;
    [SerializeField] AudioClip updatePatrolSFX_1;
    [SerializeField] AudioClip updatePatrolSFX_2;
    private AudioSource audioSource;
    private bool startPatrollingSFXPlayed = false;

    private void Start()
    {
        // Components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Initial values
        totalSpeed = speed;
    }

    void Update()
    {
        if(playerSpotted && !isEscapingFromPlayer)
        {
            Vector3 player = GameObject.FindWithTag("Player").transform.position;
            rb.isKinematic = true;
            
            // Audio

            if (transform.position != player)
            {
                // Run to player
                transform.position = Vector2.MoveTowards(transform.position,
                    player, chaseSpeed * Time.deltaTime);

                // Attack animation
                float distance = Vector3.Distance(transform.position, player);
                Debug.Log("Distance to player: " + distance.ToString());

                if (distance < jumpAttackDistance && !isAttacking)
                {
                    isAttacking = true;
                    animator.SetBool("b_isAttacking", isAttacking);
                }
            }
            else
            {
                isAttacking = false;
                animator.SetBool("b_isAttacking", isAttacking);
                rb.isKinematic = false;

                GameManager.Instance.GameOver();
            }

        }
        else
        {
            Patrol();
        }

    }

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
        audioSource.PlayOneShot(caughtLaughSFX);
    }

    public void SetTotalSpeed(int dollPlaced)
    {
        // 50% extra for each doll placed
        totalSpeed = speed + (speed * dollPlaced / 2f);
        Debug.Log("Enemy speed increased by " +  (50 * dollPlaced).ToString() + "% !");

        // Audio Cue
        if (dollPlaced == 1)
        {
            audioSource.PlayOneShot(updatePatrolSFX_1);
        }
        else if (dollPlaced == 2)
        {
            audioSource.PlayOneShot(updatePatrolSFX_2);
        }
    }

    #region PATROL_AI
    private void Patrol()
    {
        if (needClimbing)
        {
            ClimbLadder();
        }
        
        else
        {
            // Audio
            if (!startPatrollingSFXPlayed)
            {
                if(startPatrollingSFX != null)
                {
                    audioSource.PlayOneShot(startPatrollingSFX);
                    startPatrollingSFXPlayed = true;
                }
            }

            // if Waypoint is on the same floor
            float distanceToWaypoint = Math.Abs(waypoints[nextWaypoint].position.x - transform.position.x);

            // If not in current waypoint X position, go to it
            if (distanceToWaypoint > minWaypointDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    waypoints[nextWaypoint].position, totalSpeed * Time.deltaTime);

                // Animation - Look at Waypoint
                if (waypoints[nextWaypoint].position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else if (waypoints[nextWaypoint].position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                
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
        startPatrollingSFXPlayed = false;

        // Animation
        animator.SetBool("b_isWaiting", isWaiting);

        // Wait for a few seconds in the room
        yield return new WaitForSeconds(waitTime);
        
        // Set the next waypoint
        //currentWaypoint++;
        ChooseNextWaypoint();

        isWaiting = false;
        animator.SetBool("b_isWaiting", isWaiting);

        // check if is a ladder is needed for the next waypoint
        IsLadderNeeded();


        // [TO IMPLEMENT] Start wandering sound
    }

    private void ChooseNextWaypoint()
    {
        // Save current waypoint (was the last "next waypoint")
        currentWaypoint = nextWaypoint;

        // 10 side dice roll
        int dice = UnityEngine.Random.Range(0, 9);
        Debug.Log("Dice = " + dice.ToString());
        
        // 1/6 chance to go to previous waypoint
        if (dice == 0)
        {
            nextWaypoint--;

            // If the waypoint index ends up beeing less than 0
            if(nextWaypoint < 0)
            {
                // Go to waypoint 1
                nextWaypoint = 1;
            }
        }
        // 8/9 chances to go to the next waypoint
        else
        {
            nextWaypoint++;

            // Reset to the first waypoint if last waypoint reached
            if (nextWaypoint == waypoints.Length)
            {
                // Reverse the array of waypoints
                System.Array.Reverse(waypoints);

                // Set the waypoint to the first of the array
                nextWaypoint = 0;
            }
        }
    }

    private void IsLadderNeeded()
    {
        // If the last waypoint has a different tag than the next, it means they are in different floors
        if(nextWaypoint != 0 && waypoints[currentWaypoint].gameObject.tag != waypoints[nextWaypoint].gameObject.tag)
        {
            Debug.Log("Climbing needed");
            
            // Select closer ladder spot
            for (int i = 0; i < ladderSpots.Length; i++)
            {
                distance = Vector3.Distance(transform.position, ladderSpots[i].transform.position);

                if (distance < nearestDistance)
                {
                    // sets the closer one
                    closerLadderSpot = ladderSpots[i];

                    // sets the distance
                    nearestDistance = distance;
                }
            }

            // Select the second closer ladder spot (the other end of the ladder)
            if(closerLadderSpot != null && secondCloserLadderSpot == null) 
            {
                Debug.Log("Enemy: Searching for Second Closer Ladder Spot...");
                nearestDistance = 10000;

                for (int i = 0; i < ladderSpots.Length; i++)
                {
                    distance = Vector3.Distance(transform.position, ladderSpots[i].transform.position);

                    // If the distance is lesser than the nearest and is not the nearest
                    if(distance < nearestDistance && ladderSpots[i] != closerLadderSpot)
                    {
                        secondCloserLadderSpot = ladderSpots[i];
                        nearestDistance = distance;
                    }
                }
            }
            
            needClimbing = true;
        }
    }

    private void ClimbLadder()
    {
        // Move towards the closest ladder point X axis position
        transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(closerLadderSpot.transform.position.x, transform.position.y), totalSpeed * Time.deltaTime);

        // When the ladder is reached
        if(transform.position.x == closerLadderSpot.transform.position.x)
        {
            isClimbing = true;
            rb.isKinematic = true;
            animator.SetBool("b_isClimbing", isClimbing);
        }

        if(isClimbing)
        {
            // Move towards Y axis position of the second closed ladder (the other end of the current ladder)
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(closerLadderSpot.transform.position.x, secondCloserLadderSpot.transform.position.y), totalSpeed * Time.deltaTime);

            // if reached the end of the ladder
            if(transform.position.y == secondCloserLadderSpot.transform.position.y)
            {
                // Return to normal behaviour
                closerLadderSpot = null;
                secondCloserLadderSpot = null;
                distance = 0;
                nearestDistance = 10000;
                rb.isKinematic = false;
                needClimbing = false;
                isClimbing = false;
                animator.SetBool("b_isClimbing", isClimbing);

            }
        }
    }
    #endregion

    public void EscapeFromPlayer()
    {
        isEscapingFromPlayer = true;
        animator.SetBool("b_isScared", isEscapingFromPlayer);
        
        // Don't wait in rooms, just run
        waitTime = 0.1f;
    }




}
