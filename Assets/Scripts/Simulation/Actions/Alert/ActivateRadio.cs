﻿using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/ActivateRadio")]
public class ActivateRadio : Action
{
    [System.NonSerialized] Radio radio;
    [System.NonSerialized] bool activated = false;
    public override void Execute(AIAgent agent)
    {
        if (!radio)
            return;

        agent.Target = radio.transform;
        if (Vector2.Distance(agent.transform.position, radio.transform.position) < 2f)
        {
            radio.ActivateRadio();
            activated = true;
        }
    }

    public override bool IsComplete(AIAgent agent)
    {
        // If there is not radio in the environment | Is activated | The environment of the radio is not available
        return !radio || activated || (!EnvironmentManager.Instance.IsEnvironmentAvailable(radio.environment)); 
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        var radios = FindObjectsOfType<Radio>().Where(x => !x.Active && !x.Reserved && EnvironmentManager.Instance.IsEnvironmentAvailable(x.environment));
        float minDistance = Mathf.Infinity;
        InfectedAgent infected = StoryManager.Instance.Infected;
        foreach (var radio in radios)
        {
            float distance = Vector2.Distance(agent.transform.position, radio.transform.position);
            float distanceToInfected = Vector2.Distance(infected.transform.position, radio.transform.position);
            if (distance < minDistance && distanceToInfected > 2f)
            {
                this.radio = radio;
                minDistance = distance;
                // If the radio is in the same enviornment of the agent is good enougth
                if (agent.Environment == radio.environment) break;
            }
        }
        if(radio)
            radio.Reserved = true;
    }
}
