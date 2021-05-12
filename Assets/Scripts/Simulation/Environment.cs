using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EnvironmentType
{
    Any,
    Room,
    SafeRoom,
    Lobby,
    Corridor,
    Garden,
    Kitchen,
    Bathroom
}
public class Environment : MonoBehaviour
{
    public EnvironmentType environment;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Agent"))
        {
            AIAgent agent = other.GetComponent<AIAgent>();
            agent.Environment = environment;
            if(!EnvironmentManager.Instance.IsEnvironmentAvailable(environment))
            {
                agent.AddLockEnvironment(environment);
                //agent.ChangedAction();
            }
        }
        else if(other.CompareTag("InfectedAgent"))
        {
            InfectedAgent agent = other.GetComponent<InfectedAgent>();
            agent.Environment = environment;
        }
        else if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.environment = environment;
        }
           
    }

}
