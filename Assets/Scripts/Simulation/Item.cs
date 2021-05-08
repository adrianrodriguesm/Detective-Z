using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType
{
    Weapon, Drink, Book, Cigarette, Kitchen, Car, Ball, Pants
}
public class Item : MonoBehaviour
{
    public ItemType type;
    [Header("Storytelling elements")]
    public GameObject itemAdded;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public GameObject itemFall;
    public GameObject itemUse;
    public GameObject itemDeath;
    public float offsetDeadX = 1f;
    public float offsetDeadY = 1f;
    public static List<Vector2> lastInstantion = new List<Vector2>();
    public static float distance = 0.5f;
    public virtual void OnItemAdded(AIAgent agent)
    {
        agent.items.Add(this);
        if (!itemAdded)
            return;

        transform.GetChild(0).gameObject.SetActive(false);
        Instantiate(itemAdded, transform.position, Quaternion.identity);
    }

    public void OnItemUse(AIAgent agent, Vector3 position)
    {
        if (!itemUse)
            return;

        Instantiate(itemUse, position, Quaternion.identity);
    }

    public void OnItemUse(AIAgent agent)
    {
        if (!itemUse)
            return;

        Instantiate(itemUse, transform.position, Quaternion.identity);
    }

    // Call when the agent changed its state from calm to alert/attacked
    public void OnStateChanged(AIAgent agent)
    {
        if (!itemFall)
            return;

        Vector2 position = Vector2.zero;
        do
        {
            float offsetXDelta = Random.Range(-offsetX, offsetX);
            float offsetYDelta = Random.Range(-offsetY, offsetY);
            position.x = agent.transform.position.x + offsetXDelta;
            position.y = agent.transform.position.y + offsetYDelta;
        } while (CheckDistance(position));
        lastInstantion.Add(position);
        Instantiate(itemFall, new Vector3(position.x, position.y, agent.transform.position.z), Quaternion.identity);
    }

    public bool CheckDistance(Vector2 position)
    {
        var objectsClose = lastInstantion.Where(x => Vector2.Distance(position, x) < distance);
        return objectsClose.Count() > 0;
    }

    public void OnDeath(AIAgent agent)
    {
        if (!itemDeath)
            return;

        
        Vector2 position = Vector2.zero;
        do
        {
            float offsetXDelta = Random.Range(-offsetDeadX, offsetDeadX);
            float offsetYDelta = Random.Range(-offsetDeadY, offsetDeadY);
            position.x = agent.transform.position.x + offsetXDelta;
            position.y = agent.transform.position.y + offsetYDelta;
        } while (CheckDistance(position));
        lastInstantion.Add(position);
        Instantiate(itemDeath, new Vector3(position.x, position.y, agent.transform.position.z), Quaternion.identity);
    }

    
    // Cigaratte
    // -- OnActionChanged -- Leave Lit cigarette
    // -- OnDeath         -- Packege of cigarette next to the body
    // Glasses
    // -- OnActionChanged -- Leave glasses
    // -- OnDeath         -- Nothing
    // Wine
    // -- OnActionChanged -- Leave wine glass
    // -- OnDeath         -- Wine stain on clothes
    // Radio -- Action (e.g turn on radio)
    // -- Atract infected
    // Phone
    // -- Send Message if the level of detection of a friend is hight and the owner of the 
    // -- phone is mostly carefull (e.g Stop NameOfTheAction or he will find you)
    // -- If the agent is not careful and recieve a message the phone will fall
    // -- send a maximum of two messages
}
