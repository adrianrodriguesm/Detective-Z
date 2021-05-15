using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public struct ClueSerialize
{
    public string ClueName;
    public int NumberOfInteraction;
    public float Duration;
}
[System.Serializable]
public struct GameplaySerialize
{
    public float GameplayDuration;
    public int NumberOfClueWithInteraction;
    public int NumberOfClueWithoutInteraction;
    [SerializeField]
    public List<ClueSerialize> Clues;
}

public class Serializer
{
    public static void Serialize<T>(T obj)
    {
        string output = JsonUtility.ToJson(obj);
        File.WriteAllText(Application.dataPath + "/Log/Log.json", output);
    }
}
