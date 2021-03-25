using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Behaviour 
{    // Behavior
    [Range(0, 1)]
    public float courage;
    [Range(0, 1)]
    public float fearfull;
    [Range(0, 1)]
    public float carefull;

    public static float CalculateWellfare(Behaviour b0, Behaviour b1)
    {
        float courageDif =  Mathf.Abs(b0.courage - b1.courage);
        float fearfullDif = Mathf.Abs(b0.fearfull - b1.fearfull);
        float carefullDif = Mathf.Abs(b0.carefull - b1.carefull);
        // Calculate the average proximity between the behaviour parameters
        return (courageDif + fearfullDif + carefullDif) / 3f;
    }
}
