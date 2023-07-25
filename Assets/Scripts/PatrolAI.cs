using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;
    [SerializeField] float waitTime = 1.5f;
    [SerializeField] private Transform[] waypoints;

    [SerializeField] private int currentWaypoint;
    private bool isWaiting;

    void Update()
    {
        // If not in current waypoint, go to it
        if(transform.position != waypoints[currentWaypoint].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                waypoints[currentWaypoint].position, speed * Time.deltaTime);
        }
        else if (!isWaiting)
        {
            StartCoroutine(WaitInRoom());
        }
    }

    IEnumerator WaitInRoom()
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
}
