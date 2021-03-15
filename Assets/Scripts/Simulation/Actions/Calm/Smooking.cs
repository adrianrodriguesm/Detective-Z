using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/Smooking")]
public class Smooking : Action
{

    public GameObject cigarette;
   
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
        Instantiate(cigarette, agent.transform.position, Quaternion.identity);
        //EnvironmentManager.Instance.SetTile(storytellingElementGenerated, environment, agent.transform.position);
    }
}
