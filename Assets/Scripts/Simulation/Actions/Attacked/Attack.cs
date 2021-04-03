﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/Attack")]
public class Attack : Action
{
    InfectedAgent infected;
    public GameObject weaponObject;
    [System.NonSerialized] Weapon weapon;
    Transform weaponLocation;
   

   [Header("Paremeter of body attack (in case of weapon not being assing or weapon impossible to use)")]
    public int damage = 3;
    public float radiusAttack = 1f;
    public LayerMask layer;
    [Header("Clue placement offset")]
    public float offsetRadiusX = 1f;
    public float offsetRadiusY = 1f;
    public GameObject deadCharacter;
    [System.NonSerialized] bool attackWithBody = false;
    [System.NonSerialized] bool foundWeapon = false;

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
                if (agent.HasWeapon(weapon))
                {
                    // Updated target transform
                    agent.target = infected.transform;
                    AttackAndClueGeneration(weapon.radiusAttack, weapon.attackType, agent);
                }
                // Othewise find a weapon in environment
                else
                {
                    if (!foundWeapon)
                    {
                        Debug.Log("Unkonw weapon location!");
                        var weapons = FindObjectsOfType<Weapon>().Where(x => x.isFree);
                        //List<Weapon> weapons = new List<Weapon>(FindObjectsOfType<Weapon>()).Where(x => x.attackType == weapon.attackType);
                        float minDistance = Mathf.Infinity;

                        foreach (var currWeapon in weapons)
                        {
                            if (currWeapon.attackType == this.weapon.attackType)
                            {
                                float distance = Vector2.Distance(agent.transform.position, currWeapon.transform.position);
                                if (distance < minDistance)
                                {
                                    this.weapon = currWeapon;
                                    weaponLocation = currWeapon.transform;
                                    minDistance = distance;

                                }
                            }

                        }
                        foundWeapon = true;
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
            else if (!weapon && !attackAction && agent.HasWeapons())
            {
                foundWeapon = true;
                weapon = agent.TryGetWeapon();
                // Updated target transform
                agent.target = infected.transform;
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
            agent.target = infected.transform;
            AttackAndClueGeneration(radiusAttack, AttackType.Body, agent);
        }

        
    }

    private void AttackAndClueGeneration(float radius, AttackType type, AIAgent agent)
    {
        actionState = State.Attacked;
        agent.State = State.Attacked;
        Collider2D hit = Physics2D.OverlapCircle(agent.transform.position, radius, layer);
        if (hit)
        {
            /** /
            float offsetX = Random.Range(-offsetRadiusX, offsetRadiusX);
            float offsetY = Random.Range(-offsetRadiusY, offsetRadiusY);
            Instantiate(storytellingElement, new Vector3(agent.transform.position.x + offsetX, agent.transform.position.y + offsetY, agent.transform.position.z), Quaternion.identity);
            /**/
            // If the agent is mainly fearfull it won't attack the infected

            infected.TakeDamage(type, agent);
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
        infected = StoryManager.Instance.Infected;
        foundWeapon = false;
    }
}