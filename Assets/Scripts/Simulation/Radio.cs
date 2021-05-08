using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public EnvironmentType environment;
    // Turn on radio (audio)
    InfectedAgent infected;
    bool activated = false;
    new AudioSource audio;
    Clue clue;
    bool soundPlaying = false;
    public bool Active
    {
        get { return activated; }
    }
    // Start is called before the first frame update
    void Start()
    {
        infected = StoryManager.Instance.Infected;
        audio = GetComponent<AudioSource>();
        audio.enabled = false;
        activated = false;
        clue = GetComponent<Clue>();
        if (clue)
            clue.Enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!soundPlaying && activated && StoryManager.Instance.IsSimulationEnd())
        {
            audio.enabled = true;
            soundPlaying = true;
        }
    }
           

    public void ActivateRadio()
    {
        // Play audio
        infected.SuspectTarget = transform;
        activated = true;

        if (clue)
            clue.Enable = true;
        //clue.Enable = true;
    }

}
