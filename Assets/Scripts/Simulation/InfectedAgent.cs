using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackType
{
    Body,
    Knive,
    Pistol,
}

public class InfectedAgent : MonoBehaviour
{
    [Header("Infected Stats")]
    public int health;
    public float radiusAttack = 1f;
    public LayerMask layer;
    public int damage = 5;

    public InfectedAction action;
    private AIAgent targetAgent;

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
    EnvironmentType environment = EnvironmentType.Garden;
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
    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<AIAgent>().ToList();
        // First Agent is randonly selected
        targetAgent = agents[Random.Range(0, agents.Count)];
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        attackTypeRecived = new HashSet<AttackType>();
        currAction = new SeekAgent(this);
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

    }

    public bool IsDead()
    {
        return health <= 0f;
    }

    public void TakeDamage(int damage, AttackType type)
    {
        attackTypeRecived.Add(type);
        health -= damage;
        if (IsDead())
            GetComponent<Collider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Agent"))
            targetAgent = collision.collider.GetComponent<AIAgent>();
    }


}
