using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentType
{
    Any,
    Room,
    Lobby,
    Corridor,
    Garden,
}
public class Evironment : MonoBehaviour
{
    public EnvironmentType environment = EnvironmentType.Any;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Agent")
            other.GetComponent<AIAgent>().Environment = environment;
    }

}
