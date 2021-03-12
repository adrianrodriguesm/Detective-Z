using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
[CreateAssetMenu(menuName = "Actions/AttackWithBody")]
public class AttackWithBody : Action
{
    InfectedAgent infected;
    public int damage = 3;
    public float radiusAttack = 20f;
    public LayerMask layer;
    public int offsetRadius = 1;
    public override void Execute(AIAgent agent)
    {
        infected = StoryManager.Instance.Infected;
        // Updated target transform
        agent.target = infected.transform;
        Debug.Log("Attack!!");
        Collider2D hit = Physics2D.OverlapCircle(agent.transform.position, radiusAttack, layer);
        if (hit != null)
        {
            infected.TakeDamage(damage);

            Vector3Int positionInCell = EnvironmentManager.Instance.GetTileCellPosition(agent.transform.position, environment);
            for (int index = 1; index <= offsetRadius; index++)
            {
                // Top
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x + offsetRadius, positionInCell.y + offsetRadius, positionInCell.z));
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x - offsetRadius, positionInCell.y + offsetRadius, positionInCell.z));
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x,                positionInCell.y + offsetRadius, positionInCell.z));
                // Center
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x + offsetRadius, positionInCell.y, positionInCell.z));
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x,                positionInCell.y, positionInCell.z));
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x - offsetRadius, positionInCell.y, positionInCell.z));
                // Bottom
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x + offsetRadius, positionInCell.y - offsetRadius, positionInCell.z));
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x,                positionInCell.y - offsetRadius, positionInCell.z));
                EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, environment, new Vector3Int(positionInCell.x - offsetRadius, positionInCell.y - offsetRadius, positionInCell.z));
            }
        }
           

    }

    

    public override bool IsComplete(AIAgent agent)
    {
        return agent.health == 0 || infected.health == 0;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }
}
