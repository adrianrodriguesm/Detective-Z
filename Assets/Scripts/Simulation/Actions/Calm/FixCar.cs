using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/FixCar")]

public class FixCar : Action
{
    public Item instruments;
    public WalkingObject oil;
    [System.NonSerialized] bool fixing = false;
    [System.NonSerialized] Car car;
    [System.NonSerialized] Transform cartrf;


    public override void Execute(AIAgent agent)
    {
        if (fixing)
            return;

        agent.Target = cartrf;
        if(Vector2.Distance(agent.transform.position, cartrf.position) < car.distanceToFix)
        {
            fixing = true;
            agent.items.Add(instruments);
            agent.objectsToInstatiateWalking.Add(oil);
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
        car = FindObjectOfType<Car>();
        if (car && !car.Reserved)
        {
            cartrf = car.enginePoint;
            car.Reserved = true;
        }
        else
        {
            car = null;
            block = true;
        }
            
    }
}
