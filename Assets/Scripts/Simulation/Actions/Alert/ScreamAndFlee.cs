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
    float dirTimer = 0;
    int currWaypointIndex = 0;
    [Header("Set of positions that the agent will go")]
    public List<Transform> waypoints;
    [Header("Timer in which the storytelling elemented will be generated")]
    public float timer = 1f;
    float currTimer = 0f;
    public override void Execute(AIAgent agent)
    {
        if (dirTimer <= 0f)
        {
            agent.target = waypoints[currWaypointIndex];
            ++currWaypointIndex;
            currWaypointIndex = currWaypointIndex < waypoints.Count ? currWaypointIndex : 0;
            dirTimer = changeDirectionTimer;
        }
        dirTimer -= Time.fixedDeltaTime;
       
        currTimer += Time.fixedDeltaTime;
        if(currTimer > timer)
        {
            Instantiate(storytellingElement, agent.gameObject.transform.position, Quaternion.identity);
            currTimer = 0;
        }
        
    }
    public override bool IsComplete(AIAgent agent)
    {
        return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
       
    }
}
