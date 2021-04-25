using UnityEditor;
using UnityEngine;


public class Escape : InfectedAction
{
    InfectedEntrance m_Exit;
    float m_Timer = 0f;
    float m_ThresholdToEscape = 2f;
    public Escape(InfectedAgent agent) : base(agent)
    {
        float minDistance = Mathf.Infinity;
        foreach(var exitPoint in StoryManager.Instance.ExitPoints)
        {
            if (exitPoint.Entered)
                continue;

            float distance = Vector2.Distance(agent.transform.position, exitPoint.transform.position);
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
            return m_Exit.transform.position;

        return m_Agent.transform.position;
    }

    public override void OnUpdate()
    {
        Move();
        m_Timer += Time.fixedDeltaTime;
        if(m_Timer > m_Agent.timeToEscape && !m_Agent.Dead)
        {
            m_Agent.InstatiateDeadAgent();
        }
        if(Vector2.Distance(m_Agent.transform.position, m_Exit.transform.position) < m_ThresholdToEscape)
        {
            m_Exit.Entry();
            m_Agent.Escaped = true;
            m_Agent.gameObject.SetActive(false);
        }
    }
}
