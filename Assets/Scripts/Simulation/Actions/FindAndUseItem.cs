using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/FindAndUseItem")]
public class FindAndUseItem : Action
{
    [Header("Item to find")]
    public GameObject itemObj;
    public bool useItem = true;
    Item item;   
    [System.NonSerialized] Transform itemTrf;
    [System.NonSerialized] bool found = false;
    [System.NonSerialized] bool exitsInTheEnvironment = false;
    [Header("Distance in which the agent will be able to catch the item")]
    [Range(0, 5)]
    [Header("Distance in which the agent will be able to catch the item")]
    public float distanceToAdd = 3f;
    [Header("Point in the enviornment in which the agent will use the item")]
    public List<Transform> pointsToUse;
    [System.NonSerialized] Transform pointToUse;
    [Header("Offset of the generating storytelling element")]
    public Vector2 instatiationUseOffset = Vector2.zero;
    [Header("Distance in which the agent will be able to use the item")]
    [Range(0, 5)]
    public float distanceToUse = 1f;
    [System.NonSerialized] bool isFinish = false;
    public override void Execute(AIAgent agent)
    {
        if (!exitsInTheEnvironment)
            return;

        if(!found)
        {
            agent.Target = itemTrf;

            if (Vector2.Distance(agent.transform.position, itemTrf.position) <= distanceToAdd)
            {
                found = true;
                isFinish = useItem ? false : true;
                item.OnItemAdded(agent);
            }
        }
        else if(useItem)
        {
            agent.Target = pointToUse;
            if (Vector2.Distance(agent.transform.position, pointToUse.position) <= distanceToUse)
            {
                isFinish = true;
                item.OnItemUse(agent, new Vector3(pointToUse.position.x + instatiationUseOffset.x, pointToUse.position.y + instatiationUseOffset.y));
            }
        }


    }

    public override bool IsComplete(AIAgent agent)
    {
        if (!exitsInTheEnvironment || isFinish)
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
        // Select a radom point to use
        pointToUse = pointsToUse[Random.Range(0, pointsToUse.Count())];
    }
}
