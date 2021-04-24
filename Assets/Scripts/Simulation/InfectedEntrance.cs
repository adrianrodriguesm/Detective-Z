using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedEntrance : MonoBehaviour
{

    public GameObject beforeEntry;
    public GameObject afterEntry;
    // Start is called before the first frame update
    void Start()
    {
        if(afterEntry)
            afterEntry.SetActive(false);
    }

    public void Entry()
    {
        if(afterEntry)
            afterEntry.SetActive(true);
        
        if(beforeEntry)
            beforeEntry.SetActive(false);
    }
}
