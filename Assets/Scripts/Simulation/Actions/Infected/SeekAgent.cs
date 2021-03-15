using Pathfinding;
using System.Collections;
using UnityEngine;

public class SeekAgent : InfectedAction
{

    public Transform m_TargetPosition;

    public SeekAgent(InfectedAgent agent, Transform targetPosition) : base(agent)
    {
        m_TargetPosition = targetPosition;
        // Call UpdatePath function every 0.5f in order to update the path
        agent.InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public SeekAgent(InfectedAgent agent) : base(agent)
    {
        m_TargetPosition = StoryManager.Instance.GetAgentToSeek().transform;
        // Call UpdatePath function every 0.5f in order to update the path
        agent.InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

   

 
    public override void OnUpdate()
    {
        if (m_Agent.Path == null)
            return;

        if (m_Agent.CurrWayPoint < m_Agent.path.vectorPath.Count)
        {
            // Move the Agent
            Vector2 direction = ((Vector2)m_Agent.Path.vectorPath[m_Agent.CurrWayPoint] - m_Agent.Rigidbody.position).normalized;
            Vector2 force = direction.normalized * m_Agent.speed * Time.fixedDeltaTime;
            m_Agent.Rigidbody.AddForce(force);

            // Update the waypoint
            float distance = Vector2.Distance(m_Agent.Rigidbody.position, m_Agent.Path.vectorPath[m_Agent.CurrWayPoint]);
            if (distance < m_Agent.nextWaypointDistance)
                m_Agent.CurrWayPoint++;

        }
        else
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
