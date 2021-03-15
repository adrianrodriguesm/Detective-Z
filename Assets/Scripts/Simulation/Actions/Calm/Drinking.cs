using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/Drinking")]
public class Drinking : Action
{

    public GameObject drink;
   
    public override void Execute(AIAgent agent)
    {
        agent.target = agent.transform;
    }



    public override bool IsComplete(AIAgent agent)
    {
        return agent.State != State.Calm;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        Instantiate(drink, agent.transform.position, Quaternion.identity);
        //EnvironmentManager.Instance.SetTile(storytellingElementGenerated, environment, agent.transform.position);
    }
}
