using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "Actions/EscapeCar")]
public class EscapeCar : Action
{
   
    [System.NonSerialized] Transform carTrf;
    [System.NonSerialized] Car car;
    [System.NonSerialized] bool triedToEscape = false;
    public override void Execute(AIAgent agent)
    {
        agent.Target = carTrf;
        if(Vector2.Distance(agent.transform.position, carTrf.position) < car.distanceToEnter)
        {
            triedToEscape = true;
            car.TurnOn(agent);
        }
    }

    public override bool IsComplete(AIAgent agent)
    {
        return !car || triedToEscape || car.IsUsed;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        car = FindObjectOfType<Car>();
        if(!car.Reserved)
        {
            carTrf = car.doorEntry;
            car.Reserved = true;
        }
        else
            car = null;

    }
}
