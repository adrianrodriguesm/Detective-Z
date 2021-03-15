using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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
    public EnvironmentType environment;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Agent"))
            other.GetComponent<AIAgent>().Environment = environment;
    }

}
