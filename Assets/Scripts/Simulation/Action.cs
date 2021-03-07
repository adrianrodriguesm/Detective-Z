using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action : ScriptableObject
{
    // The evironment of execution
    public EnvironmentType environment;
    // State of execution of action
    public State state;

    // Behavior
    [Range(0, 1)]
    public float courage;
    [Range(0, 1)]
    public float fearfull;
    [Range(0, 1)]
    public float carefull;
    [Range(0, 1)]
    public float detectionLevel;

    public abstract void Execute(AIAgent a);
    public abstract bool IsComplete();
    public abstract void OnActionFinish();

    
}
