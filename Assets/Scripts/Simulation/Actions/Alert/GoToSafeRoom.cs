using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/GoToSafeRoom")]
public class GoToSafeRoom : Action
{
    public Transform room;
    [Range(1,5)]
    public float distanceToArrive = 2f;
    private void OnEnable()
    {
        
        
    }

    public override void Execute(AIAgent agent)
    {
        agent.target = room;
    }



    public override bool IsComplete(AIAgent agent)
    {
        return Vector2.Distance(agent.transform.position, room.position) <= distanceToArrive;
    }

    public override void OnActionFinish(AIAgent agent)
    {
      
    }

    public override void OnActionPrepare(AIAgent agent)
    {
    }
}
