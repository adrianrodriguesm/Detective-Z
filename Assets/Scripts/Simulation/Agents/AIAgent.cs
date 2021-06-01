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
    // Action
    [Header("Set of actions that are going to be executed")]
    public List<ActionOrder> actions;
    int currentOrder = 0;
    ActionOrder currAction;
    public Action Action
    {
        get { return currAction.action; }
        set 
        {
            currentOrder = 0;
            currAction.action = value; 
        }
    }
    AIDestinationSetter m_Setter;
    public Transform Target
    {
        get { return m_Setter.target; }
        set { m_Setter.target = value; }
    }
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
    public DefaultAction defaultAction;
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
        set 
        {
            currentOrder = 0;
            currentState = value; 
        }
        get { return currentState; }
    }
    [Header("Intatiate when walking")]
    public List<WalkingObject> objectsToInstatiateWalking;

    // Start is called before the first frame update
    void Start()
    {
        m_Setter = GetComponent<AIDestinationSetter>();
        infected = StoryManager.Instance.Infected;
        lockEnvironments = new HashSet<EnvironmentType>();
        objectsToInstatiateWalking = new List<WalkingObject>();
        ChooseFirstAction();
        m_Setter.target = transform;
        // Call UpdatePath function every 0.4f in order to update the path
        //InvokeRepeating("UpdatePath", 0f, 0.4f);
        initialHealth = health;
    }
    // Fist action is random
    void ChooseFirstAction()
    {
        List<ActionOrder> possibleActions = actions.Where(x => (x.action.environment == currentEnvironment || x.action.environment == EnvironmentType.Any) && x.action.state == currentState
        && !StoryManager.Instance.WasActionExecuted(x.action)).ToList();
        currAction = possibleActions[Random.Range(0, possibleActions.Count)];
        currentOrder++;
        currAction.action.OnActionStart(this);
        currAction.action.OnActionPrepare(this);

        if (!(currAction.action is DefaultAction) && !currAction.action.CanRepeat)
        {
            StoryManager.Instance.AddExecutedAction(currAction.action);
        }
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


        //ProcessMovement();
        if (State != State.Calm)
        {
            // Instatiate walking storytelling element
            foreach (var storytellingElement in objectsToInstatiateWalking)
                storytellingElement.GenerateStorytellingElement(transform.position);
        }

        if (!StoryManager.Instance.UsedRandomSeed)
            Random.InitState(System.DateTime.Now.Millisecond);

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
           
            ChangedAction();
            //Debug.Log(gameObject.name + " State: " + currentState + " Action: " + currAction.action.name);
        }
        // -- Execute action
        currAction.action.Execute(this);
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
            
            GetComponent<Collider2D>().enabled = false;
            var childGPX = transform.Find("EnemyGPX");
            Destroy(childGPX.gameObject);
            float distance = 1.8f;
            /**/
            if(!StoryManager.Instance.IsAgentNear(this, distance) || !ItemManager.Instance.IsValidPosition(transform.position, distance))
            {
                Instantiate(deadCharacter, transform.position, Quaternion.identity);
            }
            else
            {
                float instantiationDelta = 1f;
                Vector2 position = Vector2.zero;
                int counter = 0;
                int maxCount = 50;
                do
                {
                    if (counter > maxCount)
                    {
                        instantiationDelta *= 1.3f;
                        //distance = 2f;
                    }
                    float offsetXDelta = Random.Range(-instantiationDelta, instantiationDelta);
                    float offsetYDelta = Random.Range(-instantiationDelta, instantiationDelta);
                    position.x = transform.position.x + offsetXDelta;
                    position.y = transform.position.y + offsetYDelta;
                    counter++;
                } while (ItemManager.Instance.IsValidPosition(position, distance));
                transform.position = position;
                Instantiate(deadCharacter, position, Quaternion.identity);
                if(counter > maxCount)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        offsetX = Random.Range(-offsetRadiusX, offsetRadiusX);
                        offsetY = Random.Range(-offsetRadiusY, offsetRadiusY);
                        Instantiate(blood, new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z), Quaternion.identity);
                    }
                  
                }
            }
            foreach (var item in items)
                item.OnDeath(this);
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
        var storyManager = StoryManager.Instance;
        List<ActionOrder> possibleActions = actions.Where(x => (x.action.environment == currentEnvironment || x.action.environment == EnvironmentType.Any) && x.action.state == currentState 
                                        && !lockEnvironments.Contains(x.action.environment) && x.order > currentOrder && !x.action.block
                                        && !storyManager.WasActionExecuted(x.action)).ToList();

        /** /
        if (currAction.action.CanRepeat && currAction.action != defaultAction)
            actions.Add(currAction);
        /**/
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
            possibleActions = possibleActions.Where(x => x.order == order).ToList();
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
        
        if(!(currAction.action is DefaultAction) && !currAction.action.CanRepeat)
        {
            storyManager.AddExecutedAction(currAction.action);
        }

    }

    public void DecreaseCounter()
    {

        currentOrder--;
        currentOrder = currentOrder <= 0 ? 0 : currentOrder;
    }
}
