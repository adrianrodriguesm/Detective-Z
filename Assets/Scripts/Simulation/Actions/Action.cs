using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public abstract class Action : ScriptableObject
{
    // The evironment of execution
    public EnvironmentType environment;
    // State of execution of action
    public State state;

    // Behavior
    public Behaviour behaviour;
    [Range(0, 1)]
    public float detectionLevel;
    [Header("Set of tile that this action will change")]
    public TileData tilesToAffect;
    [Header("Storytelling element generated")]
    public TileBase storytellingElementGenerated;

    public abstract void Execute(AIAgent a);
    public abstract bool IsComplete(AIAgent agent);
    public abstract void OnActionFinish(AIAgent agent);

    
}
