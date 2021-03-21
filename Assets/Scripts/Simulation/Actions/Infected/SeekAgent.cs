using Pathfinding;
using System.Collections;
using UnityEngine;

public class SeekAgent : InfectedAction
{
    
    public Transform m_TargetPosition;

    public SeekAgent(InfectedAgent agent, Transform targetPosition) : base(agent)
    {
        m_TargetPosition = targetPosition;
        Debug.Log("Start seek agent action!");
    }

    public SeekAgent(InfectedAgent agent) : base(agent)
    {
        m_TargetPosition = StoryManager.Instance.GetAgentToSeek().transform;
    }

   

 
    public override void OnUpdate()
    {
        if (m_Agent.Path == null)
            return;

        float distance = Vector2.Distance(m_Agent.transform.position, m_TargetPosition.transform.position);
        if (m_Agent.CurrWayPoint < m_Agent.path.vectorPath.Count)
        {
            Debug.Log("Go to agent position");
            // Move the Agent
            Vector2 direction = ((Vector2)m_Agent.Path.vectorPath[m_Agent.CurrWayPoint] - m_Agent.Rigidbody.position).normalized;
            Vector2 force = direction.normalized * m_Agent.speed * Time.fixedDeltaTime;
            m_Agent.Rigidbody.AddForce(force);

            // Update the waypoint
            float distanceToNextWaypoint = Vector2.Distance(m_Agent.Rigidbody.position, m_Agent.Path.vectorPath[m_Agent.CurrWayPoint]);        
            if (distanceToNextWaypoint < m_Agent.nextWaypointDistance)
                m_Agent.CurrWayPoint++;

        }
        else if(distance < m_Agent.distanceThresholdToAttack)
        {
            AIAgent targetAgent = m_TargetPosition.GetComponent<AIAgent>();
            if (targetAgent)
            {
                
                m_Agent.Action = new AttackAgent(m_Agent, targetAgent);
            }
            else
            {
                m_TargetPosition = StoryManager.Instance.GetAgentToSeek().transform;
            }
           
        }


    }

    public override Vector3 GetTargetPosition()
    {
       return m_TargetPosition.position;
    }
}
