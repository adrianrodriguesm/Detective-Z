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
    [Header("Pathfinding parameters")]
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    [Header("Agent's health")]
    public float health = 40f;
    bool dead = false;
    [Header("Action chooser delta")]
    [Range(0.1f, 0.5f)]
    public float deltaSelector;
    [Header("Blood asset when hurt")]
    public SpriteRenderer blood;
    public float offsetRadiusX;
    public float offsetRadiusY;

    public List<Item> items;
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
    public DefaultAction defaultAction;
    Action currAction;
    public Action Action
    {
        get { return currAction; }
        set { currAction = value; }
    }
    // Environment
    EnvironmentType currentEnvironment;
    HashSet<EnvironmentType> lockEnvironments;
    // Detection level this value changed depending on the executed action
    float detectionLevel;
    public float DetectionLevel
    {
        set { detectionLevel = Mathf.Clamp(value, 0f, 1f); }
        get { return detectionLevel; }
    }
   
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
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        infected = StoryManager.Instance.Infected;
        lockEnvironments = new HashSet<EnvironmentType>();
        ChooseFirstAction();
        // Call UpdatePath function every 0.5f in order to update the path
        InvokeRepeating("UpdatePath", 0f, 0.4f);
    }
    // Fist action is random
    void ChooseFirstAction()
    {
        List<Action> possibleActions = actions.Where(x => (x.environment == currentEnvironment || x.environment == EnvironmentType.Any) && x.state == currentState).ToList();
        currAction = possibleActions[Random.Range(0, possibleActions.Count)];
        currAction.OnActionStart(this);
        currAction.OnActionPrepare(this);
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
        if (infected.gameObject.activeSelf && State == State.Calm)
        {
            currentState = State.Alert;
            foreach(var item in items)
            {
                item.OnStateChanged(this);
            }
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
            foreach(Item item in items)
            {
                item.gameObject.SetActive(false);
            }
            return;
        }
        // Process Actions
        // -- Choose action
        if (currAction.IsComplete(this) || currentState != currAction.actionState)
        {
            Debug.Log(gameObject.name + " Changed action");
            ChangedAction();
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
            /** /
            // Update the localScale of graphic in order to flip the sprite
            if (force.x <= 0.01f)
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            else if (force.x >= 0.01f)
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            /**/
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

    public bool IsDead()
    {
        return health <= 0;
    }

    public void TakeDamage(int damage)
    {
        float offsetX = Random.Range(-offsetRadiusX, offsetRadiusX);
        float offsetY = Random.Range(-offsetRadiusY, offsetRadiusY);
        Instantiate(blood, new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z), Quaternion.identity);
        health -= damage;
        if (IsDead())
        {
            foreach(var item in items)
            {
                item.OnDeath(this);
            }
            GetComponent<Collider2D>().enabled = false;
        }
           
    }

    public bool HasWeapon(Weapon item)
    {
        return items.Contains(item);
    }

    public bool HasWeapons()
    {
        return items.Where(x => x is Weapon).Count() > 0;
    }

    public Weapon TryGetWeapon()
    {
        var weapons = items.Where(x => x is Weapon).ToList();
        if (weapons.Count() == 0)
            return null;

        return weapons[Random.Range(0, weapons.Count())] as Weapon;
    }

    public Weapon TryGetWeaponByTypeAttack(AttackType type)
    {
        var weapons = items.Where(x => x is Weapon).ToList();
        if (weapons.Count() == 0)
            return null;

       
        foreach (Weapon weapon in weapons)
        {
            if (weapon.attackType == type)
                return weapon;
        }
        // In case of not having the specific weapon result the fisrt of the list
        Weapon result = weapons[0] as Weapon;
        return result;
    }

    public void AddLockEnvironment(EnvironmentType environmentLock)
    {
        lockEnvironments.Add(environmentLock);
    }

    public void ChangedAction()
    {
        currAction.OnActionFinish(this);
        actions.Remove(currAction); 

        List<Action> possibleActions = actions.Where(x => (x.environment == currentEnvironment || x.environment == EnvironmentType.Any) && x.state == currentState 
                                        && !lockEnvironments.Contains(x.environment)).ToList();

        if (currAction.CanRepeat && currAction != defaultAction)
            actions.Add(currAction);

        currAction = null;
        float currWellfareDif = Mathf.Infinity;
        foreach (Action action in possibleActions)
        {
            float wellfareDif = action.detectionLevel;
            if (wellfareDif < currWellfareDif)
                currAction = action;

            currWellfareDif = wellfareDif;
        }

        if(!currAction)
            currAction = defaultAction;
        
        currAction.OnActionStart(this);
        currAction.OnActionPrepare(this);

    }

}
