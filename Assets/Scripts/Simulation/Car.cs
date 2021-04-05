using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject doorOpen;
    public GameObject doorBrokenAndOpen;
    public float distanceToBreakTheDoor;
    public float distanceToEnter = 2f;
    public float distanceToFix = 1f;
    bool isUsed, isBroken, infectedTargetIsHideAgent;
    AIAgent agent;
    InfectedAgent infected;
    new AudioSource audio;
    public Transform doorEntry;
    public Transform DoorEntry
    {
        get { return doorEntry; }
    }
    public Transform doorBroken;
    public Transform DoorBroken
    {
        get { return doorBroken; }
    }
    public Transform enginePoint;
    public bool IsUsed
    {
        get { return isUsed; }
        set { isUsed = value; }
    }
    public bool IsBroken
    {
        get { return isBroken; }
        set { isBroken = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        infected = StoryManager.Instance.Infected;
        audio = GetComponent<AudioSource>();
        audio.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBroken && isUsed && agent != null && infected.gameObject.activeSelf)  
        {
            if(!infectedTargetIsHideAgent && infected.Action.GetTargetPosition() == agent.transform.position)
            {
                infected.SuspectTarget = doorBroken;
                infectedTargetIsHideAgent = true;
            }
           
            if (Vector2.Distance(infected.transform.position, doorBroken.position) < distanceToBreakTheDoor)
            {
                isBroken = true;
                doorOpen.SetActive(false);
                doorBrokenAndOpen = Instantiate(doorBrokenAndOpen, transform);
            }

        }
    }
    public void TurnOn(AIAgent agent)
    {
        audio.enabled = true;
        transform.GetChild(0).gameObject.SetActive(false);
        doorOpen = Instantiate(doorOpen, transform);
    }
    public void Hide(AIAgent agent)
    {
        isUsed = true;
        this.agent = agent;
        transform.GetChild(0).gameObject.SetActive(false);
        doorOpen = Instantiate(doorOpen, transform);
    }
}
