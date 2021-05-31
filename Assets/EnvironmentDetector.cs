using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDetector : MonoBehaviour
{
    public EnvironmentType environment;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player.environment.Equals(environment))
                player.ChangedEnvironment(environment);
        }
    }
}
