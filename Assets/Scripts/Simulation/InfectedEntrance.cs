using System.Collections;
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

        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        


        if (beforeEntry)
            beforeEntry = Instantiate(beforeEntry, transform.position, Quaternion.identity);
        
        
    }

    public void Entry()
    {
        if (entered)
            return;

        if (walkingObject)
            StoryManager.Instance.Infected.objectsToInstatiateWalking.Add(walkingObject);

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name != "ZombieClip")
                transform.GetChild(i).gameObject.SetActive(true);
        }
           

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

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);

        if (exit)
            afterEntry = Instantiate(exit, transform.position, Quaternion.identity);

        if (beforeEntry)
            Destroy(beforeEntry);
    }
}
