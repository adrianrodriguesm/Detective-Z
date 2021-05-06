using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("If waypoint is null is going to be used the transform of the prefab")]
    public Transform waypoint;
    public EnvironmentType environment;
}
