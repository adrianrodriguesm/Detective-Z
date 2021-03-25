using System.Collections;
using System.Linq;
using UnityEngine;



[System.Serializable]
[CreateAssetMenu(menuName = "Actions/SearchWeapon")]
public class SearchWeapon : Action
{
    [Header("NOTE: This action needs the storytelling element of making the fact the weapon was found")]
    public Weapon item;
    [System.NonSerialized] Transform itemPosition;
    [System.NonSerialized] bool found = false;
    [System.NonSerialized] bool exitsInTheEnvironment = false;

    public override void Execute(AIAgent agent)
    {
        if (!exitsInTheEnvironment)
            return;

        agent.target = itemPosition;

        if (Vector2.Distance(agent.transform.position, itemPosition.position) < 1f)
            found = true;
    }

    public override bool IsComplete(AIAgent agent)
    {
        if (!exitsInTheEnvironment)
            return true;
        else if (found)
            return true;
        else
            return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        if(!item)
        {
            exitsInTheEnvironment = false;
            return;
        }

        var items = FindObjectsOfType<Weapon>().Where(x => x.attackType == item.attackType);
        float minDistance = Mathf.Infinity;
        foreach(Item itemInEnv in items)
        {
     
            float distance = Vector2.Distance(itemInEnv.transform.position, agent.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                itemPosition = itemInEnv.transform;
                exitsInTheEnvironment = true;
                
            }
                
            
        }
    }
}
