using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/CalmAction")]
public class CalmAction : Action
{
   
    public override void Execute(AIAgent agent)
    {
        
    }



    public override bool IsComplete(AIAgent agent)
    {
        return agent.State != State.Calm;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
        
    }
}
