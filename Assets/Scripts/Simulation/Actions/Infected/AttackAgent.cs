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
        Move();

        if (!m_TargetAgent.IsDead())
        {
            Attack();
        }         
        else
        {
            if(currTimer > timer)
            {
                if (StoryManager.Instance.AllAgentAreDead())
                {
                    m_Agent.Action = new Escape(m_Agent);
                    return;
                }
               
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
