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
    [Header("Timer in which the storytelling elemented will be generated")]
    public float timer = 1f;
    [System.NonSerialized] float currTimer = 0f;
    
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
       
        currTimer += Time.fixedDeltaTime;
        if(currTimer > timer)
        {
            Instantiate(storytellingElement, agent.gameObject.transform.position, Quaternion.identity);
            currTimer = 0;
        }
        
    }
    public override bool IsComplete(AIAgent agent)
    {
        if(currWaypointIndex >= waypoints.Count)
        {
            Debug.Log("Epappa");
        }
        return currWaypointIndex >= waypoints.Count;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
       
    }
}
