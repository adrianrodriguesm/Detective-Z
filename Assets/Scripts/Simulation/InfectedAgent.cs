using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfectedAgent : MonoBehaviour
{
    public int health;
    private AIAgent targetAgent;
    private List<AIAgent> agents;
    // AI path
    [HideInInspector]
    Path path;
    int currentWaypoint = 0;
    // Responsable for creating the path
    Seeker seeker;
    // Movement
    [Header("Pathfinding parameters")]
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<AIAgent>().ToList();
        // First Agent is randonly selected
        targetAgent = agents[Random.Range(0, agents.Count)];
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        // Call UpdatePath function every 0.5f in order to update the path
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    private void UpdatePath()
    {
        // Check if the seeker is not currently calculating a path
        // Generates a new a path
        if (seeker.IsDone())
            seeker.StartPath(rb.position, targetAgent.gameObject.transform.position, OnPathComplete);
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (!targetAgent.IsDead())
        {
            if (currentWaypoint < path.vectorPath.Count)
            {
                // Move the Agent
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction.normalized * speed * Time.fixedDeltaTime;
                rb.AddForce(force);

                // Update the waypoint
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint++;

            }
            else
                targetAgent.State = State.Attacked;
        }

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

}
