using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/ScreamAndFlee")]
public class ScreamAndFlee : Action
{
    [Header("Timer in which the direction will changed")]
    public float changeDirectionTimer;
    [System.NonSerialized]float dirTimer = 0;
    [System.NonSerialized] int currWaypointIndex = 0;
    [Header("Set of positions that the agent will go")]
    public List<Transform> waypoints;
    
    public override void Execute(AIAgent agent)
    {
        if (dirTimer <= 0f)
        {
            int index = currWaypointIndex < waypoints.Count ? currWaypointIndex : 0;
            agent.target = waypoints[index];
            ++currWaypointIndex;
            
            dirTimer = changeDirectionTimer;
        }
        dirTimer -= Time.fixedDeltaTime;
       
    }
    public override bool IsComplete(AIAgent agent)
    {
        //return currWaypointIndex >= waypoints.Count;
        return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        if(!agent.objectsToInstatiateWalking.Contains(storytellingElement))
            agent.objectsToInstatiateWalking.Add(storytellingElement);
    }
}
