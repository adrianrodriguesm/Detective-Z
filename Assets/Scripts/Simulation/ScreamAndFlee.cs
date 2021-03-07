using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "Actions/ScreamAndFlee")]
public class ScreamAndFlee : Action
{
    // Timer in order to changed the direction
    public float changeDirectionTimer;
    float dirTimer = 0;
    int currWaypointIndex = 0;
    public List<Transform> waypoints;
    public override void Execute(AIAgent agent)
    {
       Debug.Log("Screen");
        if (dirTimer <= 0f)
        {
            agent.target = waypoints[currWaypointIndex];
            ++currWaypointIndex;
            currWaypointIndex = currWaypointIndex < waypoints.Count ? currWaypointIndex : 0;
            dirTimer = changeDirectionTimer;
        }
        dirTimer-=Time.fixedDeltaTime;
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override void OnActionFinish()
    {
        
    }
}
