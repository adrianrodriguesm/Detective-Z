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
    public GameObject clue;
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
        clue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRadio()
    {
        // Play audio
        infected.SuspectTarget = transform;
        activated = true;
        audio.enabled = true;
        clue.SetActive(true);
    }

}
