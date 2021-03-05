using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIEnemy : MonoBehaviour
{
    public Transform target;
    public Transform enemyGFX;

    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    // AI path
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    // Responsable for creating the path
    Seeker seeker;
    // Movement
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
       
        // Call UpdatePath function every 0.5f in order to update the path
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    void UpdatePath()
    {
        // Check if the seeker is not currently calculating a path
        // Generates a new a path
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        // Check if therer are more waypoints in the path
        // -- Check is our current waypoint is greater than 
        // -- the number of waypoints of the path
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Move the Agent
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        rb.AddForce(force);

        // Update the waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;

        // Update the localScale of graphic in order to flip the sprite
        if (force.x < 0)
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        else
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);

    }
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
