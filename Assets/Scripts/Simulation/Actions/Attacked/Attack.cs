using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/Attack")]
public class Attack : Action
{
    InfectedAgent infected;
    public Weapon weapon;
    Transform weaponLocation;
    
   [Header("Paremeter of body attack (in case of weapon not being assing or weapon impossible to use)")]
    public int damage = 3;
    public float radiusAttack = 1f;
    public LayerMask layer;
    [Header("Clue placement offset")]
    public float offsetRadiusX = 1f;
    public float offsetRadiusY = 1f;
    public GameObject deadCharacter;
    bool attackWithBody = false;

    public override void Execute(AIAgent agent)
    {
        if (agent.IsDead())
            return;

        if (!infected)
            infected = StoryManager.Instance.Infected;
              

        if(weapon != null && !attackWithBody)
        {
            if(agent.HasWeapon(weapon))
            {
                // Updated target transform
                agent.target = infected.transform;
                AttackAndClueGeneration(weapon.damage, weapon.radiusAttack, weapon.attackType, agent.transform.position);
            }
            // Othewise find a weapon in environment
            else
            {
                if(!weaponLocation)
                {
                    List<Weapon> weapons = (List<Weapon>)FindObjectsOfType<Weapon>().ToList().Where(x => x.attackType == weapon.attackType);
                    float minDistance = Mathf.Infinity;
                    foreach(var weapon in weapons)
                    {
                        float distance = Vector2.Distance(agent.transform.position, weapon.transform.position);
                        if(distance < minDistance)
                        {
                            this.weapon = weapon;
                            weaponLocation = weapon.transform;
                            minDistance = distance;
                        }
                    }

                    // Check if the distance to the closest weapon is lower than the double of the distance to the infected
                    // otherwise attack with body (TODO: maybe force the execution of other action)
                    if (minDistance > 2 * Vector2.Distance(agent.transform.position, infected.transform.position))
                    {
                        attackWithBody = true;
                        return;
                    }
                        
             
                }
                // Updated the agent target in order to find the weapon
                agent.target = weaponLocation;
            }
           
        }
        else
        {
            attackWithBody = true;
            agent.target = infected.transform;
            AttackAndClueGeneration(damage, radiusAttack, AttackType.Body, agent.transform.position);
        }

        
    }

    private void AttackAndClueGeneration(float damage, float radius, AttackType type, Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, radius, layer);
        if (hit)
        {
            infected.TakeDamage(damage, type);

            float offsetX = Random.Range(-offsetRadiusX, offsetRadiusX);
            float offsetY = Random.Range(-offsetRadiusY, offsetRadiusY);
            Instantiate(storytellingElement, new Vector3(position.x + offsetX, position.y + offsetY, position.z), Quaternion.identity);
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
}
