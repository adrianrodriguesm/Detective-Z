using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public GameObject item;

    public abstract bool OnDeath();

    
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
