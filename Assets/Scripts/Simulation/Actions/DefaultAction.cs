using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/DefaultAction")]
public class DefaultAction : Action
{
    public override void Execute(AIAgent agent)
    {
        agent.target = agent.transform;
    }

    public override bool IsComplete(AIAgent agent)
    {
        return true;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }

    public override void OnActionPrepare(AIAgent agent)
    {
       
    }
}
