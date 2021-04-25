using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public SpriteRenderer radio;
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
        clue = GetComponent<Clue>();
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
        
        clue.Enable = true;
    }

}
