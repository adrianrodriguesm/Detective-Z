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
        // Get the agent world position
        Vector3 agentPosition = agent.gameObject.transform.position;
        // Obtatin the current tile
        TileBase currTile = EnvironmentManager.Instance.GetTileFromWorldPosition(agentPosition, environment);
        // Define the storytelling element
        if (tilesToAffect.tiles.Contains(currTile))
            EnvironmentManager.Instance.SetTile(storytellingElementGenerated, environment, agentPosition);
    }

    public override bool IsComplete(AIAgent agent)
    {
        return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }
}
