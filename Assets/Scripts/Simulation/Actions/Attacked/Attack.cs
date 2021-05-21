using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/Attack")]
public class Attack : Action
{
    InfectedAgent infected;
    public GameObject weaponObject;
    [Header("Distance in which the agent will be able to catch the item")]
    [Range(0, 5)]
    public float distanceToAdd = 3f;
    [System.NonSerialized] Weapon weapon;
    [System.NonSerialized] Transform weaponLocation;
   

   [Header("Paremeter of body attack (in case of weapon not being assing or weapon impossible to use)")]
    public int damage = 3;
    public float radiusAttack = 1f;
    public LayerMask layer;
    [Header("Clue placement offset")]
    public float offsetRadiusX = 1f;
    public float offsetRadiusY = 1f;
    public float distanceToWait = 5f;
    public GameObject deadCharacter;
    [System.NonSerialized] bool attackWithBody = false;
    private void OnEnable()
    {
        if (weaponObject)
            weapon = weaponObject.GetComponent<Weapon>();


    }    

    public override void Execute(AIAgent agent)
    {
        if (agent.IsDead())
            return;           

        if(!attackWithBody)
        {
            if (weapon)
            {
                //Debug.Log(weapon);
                if (agent.HasWeapon(weapon) &&
                    ((agent.State.Equals(State.Attacked) && !attackAction) || attackAction))
                {
                    // Updated target transform
                    AttackAndUpdateTarget(agent, weapon.radiusAttack, weapon.attackType);

                }
                // Othewise find a weapon in environment
                else
                {
                    if(weapon.IsUsed)
                    {
                        weapon = null;
                        attackWithBody = true;
                    }
                    else
                    {
                        // Updated the agent target in order to find the weapon
                        agent.Target = weaponLocation;
                        if (Vector2.Distance(agent.transform.position, weaponLocation.position) <= distanceToAdd)
                            weapon.OnItemAdded(agent);
                    }

                }

            }
            else if (!weapon && !attackAction && agent.HasWeapons())
            {
                weapon = agent.TryGetWeapon();
                // Updated target transform
                AttackAndUpdateTarget(agent, weapon.radiusAttack, weapon.attackType);
                agent.Target = infected.transform;
                AttackAndClueGeneration(weapon.radiusAttack, weapon.attackType, agent);
            }
            else
            {
                attackWithBody = true;
            }
        }
        else
        {
            attackWithBody = true;
            agent.Target = infected.transform;
            AttackAndUpdateTarget(agent, radiusAttack, AttackType.Body);
        }

        
    }

    private void AttackAndUpdateTarget(AIAgent agent, float radius, AttackType attackType)
    {
        if (infected.Action is AttackAgent && infected.GetTargetPostion() != (Vector2)agent.transform.position
                       && Vector2.Distance(infected.transform.position, agent.transform.position) < distanceToWait)
        {
            agent.Target = agent.transform;
        }
        else
        {
            agent.Target = infected.transform;
            AttackAndClueGeneration(radius, attackType, agent);
        }
    }

    private void AttackAndClueGeneration(float radius, AttackType type, AIAgent agent)
    {
        actionState = State.Attacked;
        agent.State = State.Attacked;
        Collider2D hit = Physics2D.OverlapCircle(agent.transform.position, radius, layer);
        if (hit)
        {
            bool makeBleed = (type.Equals(AttackType.Body) && !attackAction) ? false : true;
            infected.TakeDamage(type, agent, makeBleed);
        }
            
    }

    public override bool IsComplete(AIAgent agent)
    {
        return agent.IsDead();
    }

    public override void OnActionFinish(AIAgent agent)
    {
        if (agent.IsDead())
            weapon = null;
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        infected = StoryManager.Instance.Infected;
        if (!attackAction)
            return;

        Debug.Log("Unkonw weapon location!");
        var weapons = FindObjectsOfType<Weapon>().Where(x => x.IsFree);
        float minDistance = Mathf.Infinity;
        AttackType attackType = (AttackType)Random.Range(1, 2);
        foreach (var currWeapon in weapons)
        {

            float distance = Vector2.Distance(agent.transform.position, currWeapon.transform.position);
            if (distance < minDistance || currWeapon.environment == agent.Environment)
            {
                this.weapon = currWeapon;
                weaponLocation = currWeapon.transform;
                minDistance = distance;

                if (currWeapon.environment == agent.Environment)
                    break;
            }
                

        }
        // Check if the distance to the closest weapon is lower than the double 
        // of the  distance to the infected otherwise attack with body 
        if (minDistance > 1.5f * Vector2.Distance(agent.transform.position, infected.transform.position))
        {
            attackWithBody = true;
            weapon = null;
            return;
        }
        
    }
}
