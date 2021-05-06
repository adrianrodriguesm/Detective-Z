using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/GoToSafeRoom")]
public class GoToSafeRoom : Action
{
    public List<Waypoint> rooms;
    [System.NonSerialized] Transform room;
    [Range(1,5)]
    public float distanceToArrive = 2f;
    private void OnEnable()
    {
        
        
    }

    public override void Execute(AIAgent agent)
    {
        agent.Target = room;
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
        foreach(var waypoint in rooms)
        {
            if (waypoint.environment.Equals(agent.Environment))
            {
                room = waypoint.waypoint;
                return;
            }
                
        }
        if(!room)
            room = rooms[Random.Range(0, rooms.Count - 1)].waypoint;
       
    }
}
