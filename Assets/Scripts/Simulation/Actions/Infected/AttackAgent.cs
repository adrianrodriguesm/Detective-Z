using System.Collections;
using UnityEngine;


public class AttackAgent : InfectedAction
{
    AIAgent m_TargetAgent;
    float timer = 2f;
    float currTimer = 0f;
    public AttackAgent(InfectedAgent agent, AIAgent targetAgent) : base(agent)
    {
        m_TargetAgent = targetAgent;
        m_TargetAgent.State = State.Attacked;
        Debug.Log("Start attack agent action!");
    }

    public override void OnUpdate()
    {
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
        else if (!m_TargetAgent.IsDead())
        {
            Attack();
        }         
       else
       {
            if(currTimer > timer)
            {
                m_Agent.ResetBloodWalking();
                if (m_Agent.SuspectTarget != null)
                {
                    m_Agent.Action = new SeekAgent(m_Agent, m_Agent.SuspectTarget);
                } 
                else
                {
                    m_TargetAgent = StoryManager.Instance.GetAgentToSeek();
                    m_Agent.Action = new SeekAgent(m_Agent, m_TargetAgent.transform);
                }
                return;
            }
            currTimer += Time.fixedDeltaTime;
       }
           
    }

    void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(m_Agent.transform.position, m_Agent.radiusAttack, m_Agent.layer);
        if (hit)
        {
            m_TargetAgent.TakeDamage(m_Agent.damage);
        }
    }

    public override Vector3 GetTargetPosition()
    {
        return m_TargetAgent.transform.position;
    }
}
