using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/Wait")]
public class Wait : Action
{
    public override void Execute(AIAgent agent)
    {
        
    }

    public override bool IsComplete(AIAgent agent)
    {
        return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        agent.Target = agent.transform;
    }
}
