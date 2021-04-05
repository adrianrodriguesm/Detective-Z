using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/GoToEnvironmentAndUseItem")]
public class GoToEnvironmentAndUseItem : Action
{
    [Header("Agent need to have the item")]
    public GameObject itemObj;
    [System.NonSerialized] Item item;
    public Transform targetTrf;
    public Vector2 instatiationOffset = Vector2.zero;
    public float distanceToUse = 1f;
    [System.NonSerialized] bool isFinish = false;

    private void OnEnable()
    {

    }

    public override void Execute(AIAgent agent)
    {
        agent.target = targetTrf;
        if(Vector2.Distance(agent.transform.position, targetTrf.position) <= distanceToUse)
        {
            item.OnItemUse(agent, new Vector3(targetTrf.position.x + instatiationOffset.x, targetTrf.position.y + instatiationOffset.y));
            isFinish = true;
        }

    }



    public override bool IsComplete(AIAgent agent)
    {
        return isFinish;
    }

    public override void OnActionFinish(AIAgent agent)
    {

    }

    public override void OnActionPrepare(AIAgent agent)
    {
        if(!itemObj)
        {
            isFinish = true;
            return;
        }

        item = itemObj.GetComponent<Item>();
        
       foreach(var currItem in agent.items)
       {
            if(currItem.type == item.type)
            {
                item = currItem;
                break;
            }

       }
     
    }
}
