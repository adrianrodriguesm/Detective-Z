using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Actions/WalkingObject")]
public class WalkingObject : ScriptableObject
{
    public List<GameObject> gradientClue;
    public bool loop;
    [Range(0, 3)]
    public float spacing;
    [System.NonSerialized] int currIndex = 0;
    [System.NonSerialized] Vector3 lastIntatiation = Vector3.zero;
    [Range(0,5)]
    public float gradientTimer;
    [System.NonSerialized] float currTimer;
    [System.NonSerialized] bool stoop = false;
    // Called every frame
    public void GenerateStorytellingElement(Vector3 position)
    {
        if (stoop)
            return;

        currTimer += Time.fixedDeltaTime;
        if (currTimer >= gradientTimer)
        {
            currTimer = 0;
            if (!loop && currIndex == gradientClue.Count() - 1)
                stoop = true;
            else
            {
                currIndex++;
                currIndex = Mathf.Clamp(currIndex, 0, gradientClue.Count() - 1);
            }
        }

        if (Vector2.Distance(position, lastIntatiation) >= spacing)
        {
            lastIntatiation = position;
            Instantiate(gradientClue[currIndex], position, Quaternion.identity);
           
        }
    }

    public void Reset()
    {
        currIndex = 0;
        stoop = false;
        lastIntatiation = Vector3.zero;
    }

}
