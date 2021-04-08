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
    [System.NonSerialized] public State actionState;
    [Range(0, 1)]
    public float detectionLevel;

    public bool attackAction = false;

    public bool CanRepeat = false;

    public virtual void OnActionStart(AIAgent agent)
    {
        agent.DetectionLevel += detectionLevel;
        actionState = state;
    }
    public abstract void OnActionPrepare(AIAgent agent);
    public abstract void Execute(AIAgent agent);
    public abstract bool IsComplete(AIAgent agent);
    public abstract void OnActionFinish(AIAgent agent);

    
}
