using System.Collections;
using System.Linq;
using UnityEngine;



[System.Serializable]
[CreateAssetMenu(menuName = "Actions/SearchWeapon")]
public class SearchWeapon : Action
{
    [Header("NOTE: This action needs the storytelling element of making the fact the weapon was found")]
    public GameObject weaponObj;
    [System.NonSerialized] Weapon weapon;
    [System.NonSerialized] Transform weaponPosition;
    [System.NonSerialized] bool found = false;
    [System.NonSerialized] bool exitsInTheEnvironment = false;
    [Header("Distance in which the agent will be able to catch the item")]
    [Range(0, 5)]
    public float distanceToAdd = 3f;
    public override void Execute(AIAgent agent)
    {
        if (!exitsInTheEnvironment || found)
            return;

        agent.Target = weaponPosition;

        if (Vector2.Distance(agent.transform.position, weaponPosition.position) <= distanceToAdd)
        {
            found = true;
            weapon.OnItemAdded(agent);
        }
            
    }

    public override bool IsComplete(AIAgent agent)
    {
        if (!exitsInTheEnvironment || found || !weapon.IsFree)
            return true;
        else
            return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        if(!weaponObj)
        {
            exitsInTheEnvironment = false;
            return;
        }
        weapon = weaponObj.GetComponent<Weapon>();
        var weapons = FindObjectsOfType<Weapon>().Where(x => x.attackType == weapon.attackType && x.IsFree);
        float minDistance = Mathf.Infinity;
        foreach(Weapon weaponInEnv in weapons)
        {
     
            float distance = Vector2.Distance(weaponInEnv.transform.position, agent.transform.position);
            if (distance < minDistance)
            {
                weapon = weaponInEnv;
                minDistance = distance;
                weaponPosition = weaponInEnv.transform;
                exitsInTheEnvironment = true; 
            }
                
            
        }
    }
}
