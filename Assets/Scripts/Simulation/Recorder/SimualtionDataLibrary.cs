using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SimualtionData/Data")]
public class SimualtionDataLibrary : ScriptableObject
{
    public List<Sprite> simulations;
}
