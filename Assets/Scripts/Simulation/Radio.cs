using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public EnvironmentType environment;
    bool isReserved = false;
    public bool Reserved
    {
        get { return isReserved; }
        set { isReserved = value; }
    }
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
    StoryManager m_StoryManager;
    // Start is called before the first frame update
    void Start()
    {
        m_StoryManager = StoryManager.Instance;
        infected = m_StoryManager.Infected;
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
        if(GameplayManager.Instance.GameplayStarted)
        {
            if (!soundPlaying && activated && m_StoryManager.AnimationState.Equals(AnimationState.Stop))
            {
                audio.enabled = true;
                soundPlaying = true;
            }
            else if(!m_StoryManager.AnimationState.Equals(AnimationState.Stop))
            {
                audio.enabled = false;
                soundPlaying = false;
            }
        }



    }
           

    public void ActivateRadio()
    {
        // Play audio
        infected.AddSuspectTarget(transform);
        activated = true;

        if (clue)
            clue.Enable = true;
        //clue.Enable = true;
    }

}
