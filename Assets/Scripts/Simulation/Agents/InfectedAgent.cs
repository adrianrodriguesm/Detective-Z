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
    [Range(0,20)]
    public float timeToEscape = Mathf.Infinity;
    [Range(0,7)]
    public float deltaToEscape = 2f;
    [Header("Storytelling Elements")]
    public GameObject deadInfectedAgent;
    public WalkingObject infectedBloodWalk;
    bool escaped = false;
    bool wasAttacked = false;
    AttackType m_LastAttackTypeReceived = AttackType.Body;
    public AttackType LastAttackTypeReceived
    {
        get { return m_LastAttackTypeReceived; }
    }
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

    // Responsable for creating the path
    Seeker seeker;
    public Seeker Seeker
    {
        get { return seeker; }
        set { seeker = value; }
    }
    AIDestinationSetter m_DestinationSetter;
    public Transform Target
    {
        get { return m_DestinationSetter.target; }
        set { m_DestinationSetter.target = value; }
    }
    // Movement
    EnvironmentType environment = EnvironmentType.Any;
    public EnvironmentType Environment
    {
        set { environment = value; }
        get { return environment; }
    }

    InfectedAction currAction;
    public InfectedAction Action
    {
        get { return currAction; }
        set { currAction = value; }
    }

    List<Transform> suspectTargets;
    public List<Transform> SuspectTarget
    {
        get { return suspectTargets; }
       
    }

    [Header("Intatiate when walking")]
    public List<WalkingObject> objectsToInstatiateWalking;
    // Start is called before the first frame update
    void Start()
    {
        m_DestinationSetter = GetComponent<AIDestinationSetter>();
        agents = FindObjectsOfType<AIAgent>().ToList();
        timeToEscape -= Random.Range(0, deltaToEscape);
        seeker = GetComponent<Seeker>();
        suspectTargets = new List<Transform>();
        // First Agent is randonly selected
        AIAgent targetAgent = null;
        float minDistance = Mathf.Infinity;
        foreach(var agent in agents)
        {
            float distance = Vector2.Distance(agent.transform.position, transform.position);
            if (distance < minDistance)
            {
                targetAgent = agent;
                minDistance = distance;
            }
                
        }
        currAction = new SeekAgent(this, targetAgent.transform);

    }
    
    public Vector2 GetTargetPostion()
    {
        return Action.GetTargetPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        currAction.OnUpdate();
        // e.g Glass, Wood
        foreach(var walkingObject in objectsToInstatiateWalking)
            walkingObject.GenerateStorytellingElement(transform.position);

        if (wasAttacked)
            infectedBloodWalk.GenerateStorytellingElement(transform.position);
    }

    public void AddSuspectTarget(Transform target)
    {
        suspectTargets.Add(target);
        if (target && Action is SeekAgent)
            Action = new SeekAgent(this, target);

        
    }

    public void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public bool IsDeadOrEscaped()
    { 
        return dead || escaped;
    }

    public void InstatiateDeadAgent()
    {
        //Debug.Log("Agent Dead");
        dead = true;
        Destroy(gameObject);
        Instantiate(deadInfectedAgent, transform.position, Quaternion.identity);
    }

    public void TakeDamage(AttackType type, AIAgent agent, bool bleed)
    {
        if(bleed)
        {
            float offsetX = Random.Range(-offsetRadiusX, offsetRadiusX);
            float offsetY = Random.Range(-offsetRadiusY, offsetRadiusY);
            Instantiate(infectedBlood, new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z), Quaternion.identity);
        }
        m_LastAttackTypeReceived = type;
        InfectedAction action = currAction as AttackAgent;
        // If is attackes by an agent that is not a target
        if(action == null)
            currAction = new AttackAgent(this, agent);
        
        infectedBloodWalk.Reset();
        wasAttacked = true;

    }
}
