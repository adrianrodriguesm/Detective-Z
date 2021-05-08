using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<Radio> radio = FindObjectsOfType<Radio>().ToList();
        foreach(var waypoint in rooms)
        {
            var radioInEnvironment = radio.Where(x => x.Active && x.environment.Equals(agent.Environment));
            if (waypoint.environment.Equals(agent.Environment) && radioInEnvironment.Count() == 0)
            {
                room = waypoint.waypoint;
                return;
            }
                
        }
        if(!room)
        {
            Waypoint point = null;
            List<Radio> radioInEnvironment;
            int count = 0;
            do
            {
                point = rooms[Random.Range(0, rooms.Count)];
                radioInEnvironment = radio.Where(x => x.Active && x.environment.Equals(point.environment)).ToList();
                count++;
            } while (radioInEnvironment.Count() > 0 && count < 4f);
            room = point.waypoint;

        }
           
       
    }
}
