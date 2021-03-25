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
public class Evironment : MonoBehaviour
{
    public EnvironmentType environment;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Agent"))
        {
            AIAgent agent = other.GetComponent<AIAgent>();
            agent.Environment = environment;
            if(!EnvironmentManager.Instance.IsEnvironmentAvailable(environment))
            {
                agent.AddLockEnvironment(environment);
                agent.ChangedAction();
            }
        }
           
    }

}
