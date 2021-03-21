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

}
