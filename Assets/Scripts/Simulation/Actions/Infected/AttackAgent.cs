using System.Collections;
using System.Linq;
using UnityEngine;


public class AttackAgent : InfectedAction
{
    AIAgent m_TargetAgent;
    float timer = 2f;
    float currTimer = 0f;
    public AttackAgent(InfectedAgent agent, AIAgent targetAgent) : base(agent)
    {
        agent.Target = targetAgent.transform;
        m_TargetAgent = targetAgent;
        m_TargetAgent.State = State.Attacked;
        Debug.Log("Start attack agent action!");
    }

    public override void OnUpdate()
    {
        //Move();

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
               
                if (m_Agent.SuspectTarget.Count() > 0)
                {
                    float nearTarget = Mathf.Infinity;
                    Transform targetTransform = null;
                    foreach (var transformTarget in m_Agent.SuspectTarget)
                    {
                        float distanceToTarget = Vector2.Distance(m_Agent.transform.position, transformTarget.position);
                        if (distanceToTarget < nearTarget)
                        {
                            targetTransform = transformTarget;
                            nearTarget = distanceToTarget;
                        }
                    }
                    m_Agent.Action = new SeekAgent(m_Agent, targetTransform);
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
