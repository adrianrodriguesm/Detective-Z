using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
[CreateAssetMenu(menuName = "Actions/AttackWithGun")]
public class AttackWithGun : Action
{
    InfectedAgent infected;
    public int damage = 3;
    public float radiusAttack = 20f;
    public LayerMask layer;
    public float offsetRadius = 1f;

    public GameObject deadCharacter;
    public override void Execute(AIAgent agent)
    {
        if (infected == null)
            infected = StoryManager.Instance.Infected;

        // Updated target transform
        agent.target = infected.transform;
        Collider2D hit = Physics2D.OverlapCircle(agent.transform.position, radiusAttack, layer);
        if (hit != null && !agent.IsDead())
        {
            //infected.TakeDamage(damage, AttackType.Body);

            Vector3Int positionInCell = EnvironmentManager.Instance.GetTileCellPosition(agent.transform.position, agent.Environment);

            float offsetX = Random.Range(-offsetRadius, offsetRadius);
            float offsetY = Random.Range(-offsetRadius, offsetRadius);
            Instantiate(storytellingElement, new Vector3(agent.transform.position.x + offsetX, agent.transform.position.y + offsetY, positionInCell.z), Quaternion.identity);
            /** /
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x + offsetRadius, positionInCell.y + offsetRadius, positionInCell.z));
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x - offsetRadius, positionInCell.y + offsetRadius, positionInCell.z));
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x,                positionInCell.y + offsetRadius, positionInCell.z));
            // Center
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x + offsetRadius, positionInCell.y, positionInCell.z));
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x,                positionInCell.y, positionInCell.z));
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x - offsetRadius, positionInCell.y, positionInCell.z));
            // Bottom
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x + offsetRadius, positionInCell.y - offsetRadius, positionInCell.z));
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x,                positionInCell.y - offsetRadius, positionInCell.z));
            EnvironmentManager.Instance.SetTileCellPosition(storytellingElementGenerated, agent.Environment, new Vector3Int(positionInCell.x - offsetRadius, positionInCell.y - offsetRadius, positionInCell.z));
            /**/

        }
    }



    public override bool IsComplete(AIAgent agent)
    {
        return agent.IsDead();
    }

    public override void OnActionFinish(AIAgent agent)
    {
        if (agent.IsDead())
        {
            var childGPX = agent.transform.Find("EnemyGPX");
            Destroy(childGPX.gameObject);
            Instantiate(deadCharacter, agent.transform.position, Quaternion.identity, agent.transform);

        }
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
