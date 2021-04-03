using System.Collections;
using UnityEngine;


public abstract class InfectedAction
{
    public InfectedAgent m_Agent;
    public InfectedAction(InfectedAgent agent)
    {
        m_Agent = agent;
        m_Agent.CurrWayPoint = 0;
    }

    public abstract void OnUpdate();
    public abstract Vector3 GetTargetPosition();

    protected virtual void Move()
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
            //Debug.Log("Go to agent position");
            if (distance < m_Agent.nextWaypointDistance)
                m_Agent.CurrWayPoint++;

        }
    }

}
