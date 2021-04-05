using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/HideInCar")]
public class HideInCar : Action
{
    public float distanceToHide = 2f;
    [System.NonSerialized] Car car;
    [System.NonSerialized] Transform targetTrf;
    [System.NonSerialized] InfectedAgent infected;
    public override void Execute(AIAgent agent)
    {
        if(!car.IsUsed)
        {
            agent.target = targetTrf;
            if (Vector2.Distance(targetTrf.position, agent.transform.position) < distanceToHide)
            { 
                car.Hide(agent);
            }
        }
        else if(car.IsBroken)
        {
            agent.target = infected.transform;   
        }

    }

    public override bool IsComplete(AIAgent agent)
    {
        return !car;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        car = FindObjectOfType<Car>();
        if(car)
        {
            infected = StoryManager.Instance.Infected;
            targetTrf = car.doorEntry;
        }
    }
}
