using System.Collections;
using UnityEngine;


public abstract class InfectedAction
{
    public InfectedAgent m_Agent;
    public InfectedAction(InfectedAgent agent)
    {
        m_Agent = agent;
    }

    public abstract void OnUpdate();
    public abstract Vector3 GetTargetPosition();

}
