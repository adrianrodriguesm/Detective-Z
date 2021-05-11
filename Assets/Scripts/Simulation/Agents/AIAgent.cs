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
[System.Serializable]
public class ActionOrder
{
    public Action action;
    public int order;
}
public class AIAgent : MonoBehaviour
{
    InfectedAgent infected;
    public List<ActionOrder> actions;
    int currentOrder = 0;
    ActionOrder currAction;
    /**/
    public Action Action
    {
        get { return currAction.action; }
        set 
        {
            currentOrder = 0;
            currAction.action = value; 
        }
    }
    Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    [Header("Pathfinding parameters")]
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    [Header("Agent's health")]
    public float health = 40f;
    float minDistanceToDetectInfected = 10f;
    float initialHealth;
    bool dead = false;
    [Header("Action chooser delta")]
    [Range(0.1f, 0.5f)]
    public float deltaSelector;
    [Header("Blood asset when hurt")]
    public SpriteRenderer blood;
    public float offsetRadiusX;
    public float offsetRadiusY;
    public GameObject deadCharacter;
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
    public List<Action> actionsM;
    public DefaultAction defaultAction;

    /**/
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
    [Header("Intatiate when walking")]
    public List<WalkingObject> objectsToInstatiateWalking;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        infected = StoryManager.Instance.Infected;
        lockEnvironments = new HashSet<EnvironmentType>();
        objectsToInstatiateWalking = new List<WalkingObject>();
        ChooseFirstAction();
        // Call UpdatePath function every 0.4f in order to update the path
        InvokeRepeating("UpdatePath", 0f, 0.4f);
        initialHealth = health;
    }
    // Fist action is random
    void ChooseFirstAction()
    {
        List<ActionOrder> possibleActions = actions.Where(x => (x.action.environment == currentEnvironment || x.action.environment == EnvironmentType.Any) && x.action.state == currentState).ToList();
        currAction = possibleActions[Random.Range(0, possibleActions.Count)];
        currentOrder++;
        currAction.action.OnActionStart(this);
        currAction.action.OnActionPrepare(this);
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
        if (infected.gameObject.activeSelf && (infected.Environment == currentEnvironment
            || Vector2.Distance(infected.transform.position, transform.position) < minDistanceToDetectInfected
            || EnvironmentManager.Instance.AreAgentAlertInEnvironment(Environment))
            && State == State.Calm)
        {
            currentState = State.Alert;
            currentOrder = 0;
            foreach (var item in items)
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
            currAction.action.OnActionFinish(this);
            foreach(Item item in items)
            {
                item.gameObject.SetActive(false);
            }
            return;
        }
        // Process Actions
        // -- Choose action
        if (currAction.action.IsComplete(this) || currentState != currAction.action.actionState)
        {
            //Debug.Log(gameObject.name + " Changed action");
            ChangedAction();
        }
        // -- Execute action
        currAction.action.Execute(this);
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

            if(State != State.Calm)
            {
                // Instatiate walking storytelling element
                foreach (var storytellingElement in objectsToInstatiateWalking)
                    storytellingElement.GenerateStorytellingElement(transform.position);
            }

            /** /
            if(objectsToInstatiateWalking.Count > 0 && Vector2.Distance(transform.position, lastIntatiation) >= spacing)
            {
                foreach(var storytellingElement in objectsToInstatiateWalking)
                {
                    lastIntatiation = transform.position;
                    Instantiate(storytellingElement, transform.position, Quaternion.identity);
                }
                
            }

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
    public bool IsHurt()
    {
        return health != initialHealth;
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
            var childGPX = transform.Find("EnemyGPX");
            Destroy(childGPX.gameObject);
            Instantiate(deadCharacter, transform.position, Quaternion.identity);
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
        currAction.action.OnActionFinish(this);
        actions.Remove(currAction); 

        List<ActionOrder> possibleActions = actions.Where(x => (x.action.environment == currentEnvironment || x.action.environment == EnvironmentType.Any) && x.action.state == currentState 
                                        && !lockEnvironments.Contains(x.action.environment) && x.order > currentOrder && !x.action.block).ToList();

        if (currAction.action.CanRepeat && currAction.action != defaultAction)
            actions.Add(currAction);

        currAction.action = null;
        if (possibleActions.Count() > 0)
        {
            /**/
            float order = Mathf.Infinity;
            foreach (ActionOrder actionOrder in possibleActions)
            {
                if (order > actionOrder.order)
                    order = actionOrder.order;
            }
            possibleActions = actions.Where(x => x.order == order).ToList();
            /**/
            currAction = possibleActions[Random.Range(0, possibleActions.Count())];
            currentOrder = (int)order;
        }

        if (!currAction.action)
            currAction.action = defaultAction;
        
        currAction.action.OnActionStart(this);
        currAction.action.OnActionPrepare(this);

        if(currAction.action.block)
        {
            DecreaseCounter();
            ChangedAction();
        }

    }

    public void DecreaseCounter()
    {

        currentOrder--;
        currentOrder = currentOrder <= 0 ? 0 : currentOrder;
    }
}
