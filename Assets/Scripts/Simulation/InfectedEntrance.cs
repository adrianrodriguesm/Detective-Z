﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedEntrance : MonoBehaviour
{

    public GameObject beforeEntry;
    public GameObject afterEntry;
    public GameObject exit;
    public WalkingObject walkingObject;
    public bool Entered
    {
        get { return entered; }
    }
    bool entered;
    // Start is called before the first frame update
    void Start()
    {
        entered = false;
        if (beforeEntry)
            beforeEntry = Instantiate(beforeEntry, transform.position, Quaternion.identity);
        
        
    }

    public void Entry()
    {
        if (entered)
            return;

        if (walkingObject)
            StoryManager.Instance.Infected.objectsToInstatiateWalking.Add(walkingObject);

        entered = true;
        if (afterEntry)
            afterEntry = Instantiate(afterEntry, transform.position, Quaternion.identity);

        if (beforeEntry)
            Destroy(beforeEntry);
    }

    public void Exit()
    {
        if (entered)
            return;

        entered = true;

        if (exit)
            afterEntry = Instantiate(exit, transform.position, Quaternion.identity);

        if (beforeEntry)
            Destroy(beforeEntry);
    }
}
