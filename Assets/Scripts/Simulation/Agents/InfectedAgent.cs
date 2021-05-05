using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackType
{
    Body = 0,
    Knive = 1,
    Pistol = 2,
}

public class InfectedAgent : MonoBehaviour
{
    [Header("Infected Stats")]
    public float radiusAttack = 1f;
    public LayerMask layer;
    public int damage = 5;
    public float distanceThresholdToAttack = 3f;
    [Header("Storytelling Element")]
    public GameObject infectedBlood;
    public float offsetRadiusX = 1f;
    public float offsetRadiusY = 1f;
    public List<GameObject> infectedBloodWalking;
    public float spacingBetweenWalkingBlood;
    Vector2 lastBloodInstatiatePos = Vector2.zero;
    [Header("Timer for gradient of blood")]
    [Range(0, 10)]
    public float timerBloodWalkingGradient;
    float currTimer = 0f;
    int bloodWalkingIndex = 0;
    [Header("Posibles exits point in which the infected can scape")]
    public List<Transform> exitPoints;
    public float timeToEscape = Mathf.Infinity;
    [Header("Storytelling Elements")]
    public GameObject deadInfectedAgent;
    bool escaped = false;
    public bool Escaped
    {
        get { return escaped; }
        set { escaped = value; }
    }
    bool dead = false;
    public bool Dead
    {
        get { return dead; }
    }
    private List<AIAgent> agents;
    // AI path
    [HideInInspector]
    public Path path;
    public Path Path
    {
        get { return path; }
        set { path = value; }
    }
    int currentWaypoint = 0;
    public int CurrWayPoint
    {
        get { return currentWaypoint; }
        set { currentWaypoint = value; }
    }

    // Responsable for creating the path
    Seeker seeker;
    public Seeker Seeker
    {
        get { return seeker; }
        set { seeker = value; }
    }
    // Movement
    [Header("Pathfinding parameters")]
    public float speed = 5f;
    public float nextWaypointDistance = 3f;
    Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        set { rb = value; }
        get { return rb; }
    }
    // TODO
    EnvironmentType environment = EnvironmentType.Any;
    public EnvironmentType Environment
    {
        set { environment = value; }
        get { return environment; }
    }

   
    HashSet<AttackType> attackTypeRecived;
    InfectedAction currAction;
    public InfectedAction Action
    {
        get { return currAction; }
        set { currAction = value; }
    }

    Transform suspectTarget;
    public Transform SuspectTarget
    {
        get { return suspectTarget; }
        set 
        {
            suspectTarget = value;
            if(suspectTarget && Action is SeekAgent)
                Action = new SeekAgent(this, suspectTarget);
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<AIAgent>().ToList();
        
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        attackTypeRecived = new HashSet<AttackType>();
        // First Agent is randonly selected

        AIAgent targetAgent = null;
        float minDistance = Mathf.Infinity;
        foreach(var agent in agents)
        {
            if (Vector2.Distance(agent.transform.position, transform.position) < minDistance)
                targetAgent = agent;
        }
        currAction = new SeekAgent(this, targetAgent.transform);

        

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (this == null)
            return;
        // Check if the seeker is not currently calculating a path
        // Generates a new a path
        if (Seeker.IsDone())
            Seeker.StartPath(Rigidbody.position, Action.GetTargetPosition(), OnPathComplete);           
    }

    public void ResetBloodWalking()
    {
        currTimer = 0f;
        bloodWalkingIndex = 0;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            Path = p;
            CurrWayPoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currAction.OnUpdate();

        if (attackTypeRecived.Count > 0)
        {
            if (Action is AttackAgent)
                return;

            if (currTimer > timerBloodWalkingGradient)
            {
                currTimer = 0;
                bloodWalkingIndex++;
                bloodWalkingIndex = Mathf.Clamp(bloodWalkingIndex, 0, infectedBloodWalking.Count - 1);
            }
            if (bloodWalkingIndex < infectedBloodWalking.Count && Vector2.Distance(lastBloodInstatiatePos, transform.position) > spacingBetweenWalkingBlood)
            {
                lastBloodInstatiatePos = transform.position;
                Instantiate(infectedBloodWalking[bloodWalkingIndex], transform.position, Quaternion.identity);
            }
            currTimer += Time.fixedDeltaTime;
        }
    }

    public bool IsDeadOrEscaped()
    {
        return dead || escaped;
    }

    public void InstatiateDeadAgent()
    {
        Debug.Log("Agent Dead");
        dead = true;
        var childGPX = transform.Find("InfectedGPX");
        Destroy(childGPX.gameObject);
        Instantiate(deadInfectedAgent, transform.position, Quaternion.identity, transform);
    }

    public void TakeDamage(AttackType type, AIAgent agent)
    {
        float offsetX = Random.Range(-offsetRadiusX, offsetRadiusX);
        float offsetY = Random.Range(-offsetRadiusY, offsetRadiusY);
        Instantiate(infectedBlood, new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z), Quaternion.identity);

        InfectedAction action = currAction as AttackAgent;
        // If is attackes by an agent that is not a target
        if(action == null)
        {
            currAction = new AttackAgent(this, agent);
        }
        attackTypeRecived.Add(type);
    }
}
