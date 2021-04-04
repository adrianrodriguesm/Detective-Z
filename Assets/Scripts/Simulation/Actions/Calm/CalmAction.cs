using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/CalmAction")]
public class CalmAction : Action
{
   
    public override void Execute(AIAgent agent)
    {
        agent.target = agent.transform;
    }



    public override bool IsComplete(AIAgent agent)
    {
        return agent.State != State.Calm;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
       
    }
}
