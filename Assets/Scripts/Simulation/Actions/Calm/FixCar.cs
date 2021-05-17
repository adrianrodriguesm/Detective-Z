using System.Collections;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/FixCar")]

public class FixCar : Action
{
    [Header("Item to find")]
    public GameObject itemObj;
    public float distanceToAdd = 3f;
    Item item;
    [System.NonSerialized] Transform itemTrf;
    [System.NonSerialized] bool exitsInTheEnvironment = false;
    [System.NonSerialized] bool isFound = false;
    public WalkingObject oil;
    [System.NonSerialized] bool fixing = false;
    [System.NonSerialized] Car car;
    [System.NonSerialized] Transform cartrf;


    public override void Execute(AIAgent agent)
    {
        if(isFound)
        {
            if (fixing)
                return;

            agent.Target = cartrf;
            if (Vector2.Distance(agent.transform.position, cartrf.position) < car.distanceToFix)
            {
                fixing = true;
                //agent.items.Add(instruments);
                agent.objectsToInstatiateWalking.Add(oil);
            }
        }
        else
        {
            FoundItem(agent);
        }
  
    }

    void FoundItem(AIAgent agent)
    {
        if (!exitsInTheEnvironment)
            return;

        if (!isFound)
        {
            agent.Target = itemTrf;

            if (Vector2.Distance(agent.transform.position, itemTrf.position) <= distanceToAdd)
            {
                isFound = true;
                item.OnItemAdded(agent);
            }
        }
    }

    public override bool IsComplete(AIAgent agent)
    {
        return fixing;
    }

    public override void OnActionFinish(AIAgent agent)
    {
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        // Found item
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
        // Get car
        car = FindObjectOfType<Car>();
        if (car && !car.Reserved)
        {
            cartrf = car.enginePoint;
            //car.Reserved = true;
        }
        else
        {
            car = null;
            block = true;
        }
            
    }
}
