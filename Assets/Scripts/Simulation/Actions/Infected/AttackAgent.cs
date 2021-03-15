using System.Collections;
using UnityEngine;


public class AttackAgent : InfectedAction
{
    AIAgent m_TargetAgent;

    public AttackAgent(InfectedAgent agent, AIAgent targetAgent) : base(agent)
    {
        m_TargetAgent = targetAgent;
        m_TargetAgent.State = State.Attacked;
    }

    public override void OnUpdate()
    {
       if(!m_TargetAgent.IsDead())
            Attack();
       else
       {
            m_TargetAgent = StoryManager.Instance.GetAgentToSeek();
            m_Agent.Action = new SeekAgent(m_Agent, m_TargetAgent.transform);
       }
           
    }

    void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(m_Agent.transform.position, m_Agent.radiusAttack, m_Agent.layer);
        if (hit != null)
        {
            m_TargetAgent.TakeDamage(m_Agent.damage);
        }
    }

    public override Vector3 GetTargetPosition()
    {
        return m_Agent.transform.position;
    }
}
