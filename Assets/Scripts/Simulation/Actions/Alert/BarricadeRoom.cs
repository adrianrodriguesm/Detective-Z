using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/BarricadeRoom")]
public class BarricadeRoom : Action
{
    [Range(0, 4)]
    public float distanceToMoveObject;
    public EnvironmentType environmentOfTheRoom;
    [System.NonSerialized] List<BarricadeObject> objectsToMove;
    [System.NonSerialized] List<BarricadeObject> objectsMoved;
    [System.NonSerialized] BarricadeObject currObject;
    [System.NonSerialized] bool fistObjectMoved = true;

    public override void Execute(AIAgent agent)
    {
        // If the environment is lock and the moved objects are 0 that means that this
        // environment is already lock so this action will finish
        if(!EnvironmentManager.Instance.IsEnvironmentAvailable(environment) && objectsMoved.Count == 0)
        {
            objectsToMove.Clear();
            return;
        }
        // If reach the point barricades the door
        if (!currObject && objectsToMove.Count > 0)
        {
            // Found the near object in the room
            float nearDistance = Mathf.Infinity;
            foreach (var barricadeObject in objectsToMove)
            {
                float distance = Vector2.Distance(agent.transform.position, barricadeObject.transformObj.position);
                if (distance < nearDistance)
                {
                    currObject = barricadeObject;
                    nearDistance = distance;
                }
            }
        }
        else
        {
            if (currObject)
            {
                float distance = Vector2.Distance(agent.transform.position, currObject.transformObj.position);
                if (distance <= distanceToMoveObject)
                {
                    currObject.MoveObject();
                    objectsMoved.Add(currObject);
                    objectsToMove.Remove(currObject);
                    currObject = null;
                    if (fistObjectMoved)
                    {
                        EnvironmentManager.Instance.LockEnvironment(environmentOfTheRoom, false);
                        fistObjectMoved = false;
                    }
                }
                else
                {
                    agent.target = currObject.transformObj;
                }

            }
            
        }
    }

    public override bool IsComplete(AIAgent agent)
    {
        return objectsToMove.Count == 0;
    }

    public override void OnActionFinish(AIAgent agent)
    {
        
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        objectsToMove = EnvironmentManager.Instance.GetBarricadeObjects(agent.Environment);
        objectsMoved = new List<BarricadeObject>();
    }
}
