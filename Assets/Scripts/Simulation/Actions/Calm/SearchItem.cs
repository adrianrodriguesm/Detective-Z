using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/SearchItem")]
public class SearchItem : Action
{
    [Header("NOTE: This action needs the storytelling element of making the fact the weapon was found")]
    public GameObject itemObj;
    Item item;
    [System.NonSerialized] Transform itemTrf;
    [System.NonSerialized] bool found = false;
    [System.NonSerialized] bool exitsInTheEnvironment = false;
    [Header("Distance in which the agent will be able to catch the item")]
    [Range(0, 5)]
    public float distanceToAdd = 3f;
    public override void Execute(AIAgent agent)
    {
        if (!exitsInTheEnvironment)
            return;

        agent.target = itemTrf;

        if (Vector2.Distance(agent.transform.position, itemTrf.position) < distanceToAdd)
        {
            found = true;
            item.OnItemAdded(agent);
        }
           
    }

    public override bool IsComplete(AIAgent agent)
    {
        if (!exitsInTheEnvironment || found)
            return true;
        else
            return false;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        if (!itemObj)
        {
            exitsInTheEnvironment = false;
            return;
        }
        item = itemObj.GetComponent<Item>();
        var items = FindObjectsOfType<Item>().Where(x => x.type == item.type);
        float minDistance = Mathf.Infinity;
        foreach (Item itemInEnv in items)
        {

            float distance = Vector2.Distance(itemInEnv.transform.position, agent.transform.position);
            if (distance < minDistance)
            {
                item = itemInEnv;
                minDistance = distance;
                itemTrf = itemInEnv.transform;
                exitsInTheEnvironment = true;

            }


        }
    }
}
