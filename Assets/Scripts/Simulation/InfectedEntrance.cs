using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedEntrance : MonoBehaviour
{

    public GameObject beforeEntry;
    public GameObject afterEntry;
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

        entered = true;
        if (afterEntry)
            afterEntry = Instantiate(afterEntry, transform.position, Quaternion.identity);

        if (beforeEntry)
            Destroy(beforeEntry);
    }
}
