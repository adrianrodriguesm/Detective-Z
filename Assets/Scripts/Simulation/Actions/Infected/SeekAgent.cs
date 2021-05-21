using Pathfinding;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SeekAgent : InfectedAction
{
    
    public Transform m_TargetPosition;

    public SeekAgent(InfectedAgent agent, Transform targetPosition) : base(agent)
    {
        m_TargetPosition = targetPosition;
        //Debug.Log("Start seek agent action!");
    }

    public SeekAgent(InfectedAgent agent) : base(agent)
    {
        m_TargetPosition = StoryManager.Instance.GetAgentToSeek().transform;
    }
 
    public override void OnUpdate()
    {
        Move();
        float distance = Vector2.Distance(m_Agent.transform.position, m_TargetPosition.transform.position);
        if(distance < m_Agent.distanceThresholdToAttack)
        {
            AIAgent targetAgent = m_TargetPosition.GetComponent<AIAgent>();
            if (targetAgent)
            {
                
                m_Agent.Action = new AttackAgent(m_Agent, targetAgent);
            }
            else
            {
                m_Agent.SuspectTarget.Remove(m_TargetPosition);
                if(m_Agent.SuspectTarget.Count() > 0)
                {
                    float nearTarget = Mathf.Infinity;
                    foreach(var transformTarget in m_Agent.SuspectTarget)
                    {
                        float distanceToTarget = Vector2.Distance(m_Agent.transform.position, transformTarget.position);
                        if (distanceToTarget < nearTarget)
                        {
                            m_TargetPosition = transformTarget;
                            nearTarget = distanceToTarget;
                        }
                    }
                }
                else
                    m_TargetPosition = StoryManager.Instance.GetAgentToSeek().transform;
            }
           
        }


    }

    public override Vector3 GetTargetPosition()
    {
       return m_TargetPosition.position;
    }
}
