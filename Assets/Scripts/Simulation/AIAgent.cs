using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public enum State
{
    Calm,
    Alert,
    Attacked
}
public class AIAgent : MonoBehaviour
{

    public Transform target;
    public Transform enemyGFX;

    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    public static float health = 40f;
    // AI path
    [HideInInspector]
    Path path;
    int currentWaypoint = 0;
    // Responsable for creating the path
    Seeker seeker;
    // Movement
    Rigidbody2D rb;
    // Action
    [Header("Set of actions that are going to be executed")]
    public List<Action> actions;
    Action currAction;
    // Environment
    EnvironmentType currentEnvironment;
    // Detection level
    public float detectionLevel
    {
        set { detectionLevel = value; }
        get { return detectionLevel;  }
    }

    [HideInInspector]
    public EnvironmentType Environment
    {
        get { return currentEnvironment; }
        set { currentEnvironment = value; }
    }
    State currentState;
    [Header("Personality paremeter")]
    // Behavior
    [Range(0, 1)]
    public float courage;
    [Range(0, 1)]
    public float fearfull;
    [Range(0, 1)]
    public float carefull;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currAction = actions[0];
        // Call UpdatePath function every 0.5f in order to update the path
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    private void UpdatePath()
    {
        // Check if the seeker is not currently calculating a path
        // Generates a new a path
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {    
        if (path == null)
            return;
        // Process Movement
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

            // Update the localScale of graphic in order to flip the sprite
            if (force.x <= 0.01f)
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            else if (force.x >= 0.01f)
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        // Process Actions
        // -- Choose action
        if (currAction.IsComplete())
        {
            List<Action> possibleActions = actions.Where(x => x.environment == currentEnvironment && x.state == currentState).ToList();
            float currWellfareDif = Mathf.Infinity;
            foreach (Action action in possibleActions)
            {
                float courageDif = courage - action.courage;
                float fearfullDif = fearfull - action.fearfull;
                float carefullDif = carefull - action.carefull;
                // Calculate the average proximity between the behaviour parameters
                float wellfareDif = (courageDif + fearfullDif + carefullDif)/3f;
                if (wellfareDif < currWellfareDif)
                    currAction = action;

                currWellfareDif = wellfareDif;
            }
        }
        // -- Execute action
        currAction.Execute(this);
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
