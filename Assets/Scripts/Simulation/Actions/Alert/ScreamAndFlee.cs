using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/ScreamAndFlee")]
public class ScreamAndFlee : Action
{
    //public WalkingObject storytellingElement;
    public Item peePants;
    [Header("Timer in which the direction will changed")]
    public float changeDirectionTimer;
    [System.NonSerialized]float dirTimer = 0;
    [System.NonSerialized] int currWaypointIndex = 0;
    [Header("Set of positions that the agent will go")]
    public List<GameObject> waypointsObj;
    [System.NonSerialized] List<Transform> waypoints;

    public override void Execute(AIAgent agent)
    {
        if (dirTimer <= 0f)
        {
            int index = currWaypointIndex < waypoints.Count ? currWaypointIndex : 0;
            agent.Target = waypoints[index];
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
        /** /
        if(!agent.objectsToInstatiateWalking.Contains(storytellingElement))
            agent.objectsToInstatiateWalking.Add(storytellingElement);
        /**/
        agent.items.Add(peePants);
        float distance = -Mathf.Infinity;
        Vector2 infectedPos = StoryManager.Instance.Infected.transform.position;
        GameObject currWaypointsObj = null;
        foreach (var waypointObj in waypointsObj)
        {
            float currDistance = Vector2.Distance(waypointObj.transform.position, infectedPos);
            if (distance < currDistance)
            {
                currWaypointsObj = waypointObj;
                distance = currDistance;
            }
               
        }

        if(currWaypointsObj)
            waypoints = currWaypointsObj.GetComponentsInChildren<Transform>().ToList();
         
    }
}
