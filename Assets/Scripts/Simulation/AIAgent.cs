using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public enum State
{
    Calm,
    Alert,
    Seeking,
    Attacked
}
public class AIAgent : MonoBehaviour
{
    InfectedAgent infected;
    public Transform target;
    [Header("Default asset(sprite)")]
    public Transform enemyGFX;
    [Header("Pathfinding parameters")]
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    [Header("Agent's health")]
    public float health = 40f;
    bool dead = false;
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
    EnvironmentType currentEnvironment = EnvironmentType.Garden;
    public float detectionRadious;
    [HideInInspector]
    // Detection level this value changed depending on the executed action
    public float detectionLevel;

   
    [HideInInspector]
    public EnvironmentType Environment
    {
        get { return currentEnvironment; }
        set { currentEnvironment = value; }
    }
    State currentState;
    public State State
    {
        set { currentState = value; }
        get { return currentState; }
    }
    [Header("Personality paremeter")]
    // Behavior
    public Behaviour behaviour;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        infected = StoryManager.Instance.Infected;
        ChooseFirstAction();
        // Call UpdatePath function every 0.5f in order to update the path
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }
    // Fist action is random
    void ChooseFirstAction()
    { 
        List<Action> possibleActions = actions.Where(x => x.environment == currentEnvironment && x.state == currentState).ToList();
        currAction = possibleActions[Random.Range(0, possibleActions.Count)];
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
        if (dead)
            return;

        // Start Condition
        if (infected.Environment == Environment && State == State.Calm)
        {
            currentState = State.Alert;
        }


        ProcessMovement();
        ProcessAction();
        
    }

    private void ProcessAction()
    {
        if(IsDead())
        {
           
            dead = true;
            currAction.OnActionFinish(this);
            return;
        }
        // Process Actions
        // -- Choose action
        if (currentState != currAction.state || currAction.IsComplete(this))
        {
            
            currAction.OnActionFinish(this);
            actions.Remove(currAction);
            List<Action> possibleActions = actions.Where(x => (x.environment == currentEnvironment || x.environment == EnvironmentType.Any) && x.state == currentState).ToList();
            float currWellfareDif = Mathf.Infinity;
            foreach (Action action in possibleActions)
            {
                float wellfareDif = Behaviour.CalculateWellfare(behaviour, action.behaviour);
                if (wellfareDif < currWellfareDif)
                    currAction = action;

                currWellfareDif = wellfareDif;
            }
        }
        // -- Execute action
        currAction.Execute(this);
    }

    void ProcessMovement()
    {
        if (path == null || IsDead())
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
    }

    private void OnPathComplete(Path p)
    {
        if(IsDead())
            CancelInvoke("UpdatePath");

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        
    }

    public void OnAgentDead()
    {
        infected = StoryManager.Instance.Infected;

    }
    public bool IsDead()
    {
        return health <= 0;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (IsDead())
            GetComponent<Collider2D>().enabled = false;
    }
    // TODO IMPLEMENT
    public void ForceExecutionOfKeyAction(Action action)
    {

    }

}
