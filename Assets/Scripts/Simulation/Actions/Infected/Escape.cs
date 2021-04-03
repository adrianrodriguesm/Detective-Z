using UnityEditor;
using UnityEngine;


public class Escape : InfectedAction
{
    Transform m_Exit;
    float m_Timer = 0f;
    float m_ThresholdToEscape = 0.5f;
    public Escape(InfectedAgent agent) : base(agent)
    {
        float minDistance = Mathf.Infinity;
        foreach(var exitPoint in agent.exitPoints)
        {
            float distance = Vector2.Distance(agent.transform.position, exitPoint.position);
            if(distance < minDistance)
            {
                m_Exit = exitPoint;
                minDistance = distance;
            }
        }
        Debug.Log("Escape action!");
    }
    public override Vector3 GetTargetPosition()
    {
        if (m_Timer < m_Agent.timeToEscape)
            return m_Exit.position;

        return m_Agent.transform.position;
    }

    public override void OnUpdate()
    {
        Move();
        m_Timer += Time.fixedDeltaTime;
        if(m_Timer > m_Agent.timeToEscape)
        {
            m_Agent.InstatiateDeadAgent();
        }
        if(Vector2.Distance(m_Agent.transform.position, m_Exit.position) < m_ThresholdToEscape)
        {
            m_Agent.Escaped = true;
        }
    }
}
