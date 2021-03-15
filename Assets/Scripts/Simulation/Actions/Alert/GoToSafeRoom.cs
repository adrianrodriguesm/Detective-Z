using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/GoToSafeRoom")]
public class GoToSafeRoom : Action
{
    public Transform room;
    
   
    public override void Execute(AIAgent agent)
    {
        agent.target = room;
        // If reach the point barricades the door
        // Check the environment
        // Grab a weapon
    }



    public override bool IsComplete(AIAgent agent)
    {
        return agent.State != State.Calm;
    }

    public override void OnActionFinish(AIAgent agent)
    {
      
    }
}
