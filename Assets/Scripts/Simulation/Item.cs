using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject itemFall;
    public GameObject itemDeath;
    public float offsetX = 1f;
    public float offsetY = 1f;

    // Call when the agent changed its state from calm to alert/attacked
    public void OnStateChanged(AIAgent agent)
    {
        if (!itemFall)
            return;

        float offsetXDelta = Random.Range(-offsetX, offsetX);
        float offsetYDelta = Random.Range(-offsetY, offsetY);
        Instantiate(itemFall, new Vector3(agent.transform.position.x + offsetXDelta, agent.transform.position.y + offsetYDelta, agent.transform.position.z), Quaternion.identity);
    }

    public void OnDeath(AIAgent agent)
    {
        if (!itemDeath)
            return;

        float offsetXDelta = Random.Range(-offsetX, offsetX);
        float offsetYDelta = Random.Range(-offsetY, offsetY);
        Instantiate(itemDeath, new Vector3(agent.transform.position.x + offsetXDelta, agent.transform.position.y + offsetYDelta, agent.transform.position.z), Quaternion.identity);
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
