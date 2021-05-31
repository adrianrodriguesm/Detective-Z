using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioPlayer m_AudioPlayer;
    public AudioPlayer AudioPlayer
    {
        get { return m_AudioPlayer; }
    }
    PlayerController m_Player;
    AudioListener m_Listener;
    bool m_First = false;
    AnimationState m_CurrentAnimationState;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioPlayer = GetComponent<AudioPlayer>();
        m_Player = FindObjectOfType<PlayerController>();
        PlayInsideAmbientSound();
    }

    private void Update()
    {
        if (GameplayManager.Instance.GameplayStarted)
        {
            if(!m_First)
            {
                m_AudioPlayer.PlayLoop("MainMusic");
                if (m_Player.environment.Equals(EnvironmentType.Garden))
                    PlayOutsideAmbientSound();
                else
                    PlayInsideAmbientSound();

                DestroyImmediate(m_Listener);
                m_First = true;
            }

            AnimationState currAnimState = StoryManager.Instance.AnimationState;
            if(!m_CurrentAnimationState.Equals(currAnimState) && !currAnimState.Equals(AnimationState.Stop))
            {
                StopAmbientSound();
            }
            else if(!m_CurrentAnimationState.Equals(currAnimState) && currAnimState.Equals(AnimationState.Stop))
            {
                StopSimulation();
                if (m_Player.environment.Equals(EnvironmentType.Garden))
                    PlayOutsideAmbientSound();
                else
                    PlayInsideAmbientSound();
            }

            m_CurrentAnimationState = currAnimState;
        }
            
    }

    public void PlayOutsideAmbientSound()
    {
        if(m_AudioPlayer.IsLoopPlaying("AmbientWindInside"))
        {
            m_AudioPlayer.StopLoop("AmbientWindInside");
            m_AudioPlayer.StopLoop("AmbientWindInsideExtras");
        }

        m_AudioPlayer.PlayLoop("AmbientWindOutside");
       
    }

    public void StopAmbientSound()
    {
        if (m_AudioPlayer.IsLoopPlaying("AmbientWindOutside"))
        {
            m_AudioPlayer.StopLoop("AmbientWindOutside");
        }

        if (m_AudioPlayer.IsLoopPlaying("AmbientWindInside"))
        {
            m_AudioPlayer.StopLoop("AmbientWindInside");
            m_AudioPlayer.StopLoop("AmbientWindInsideExtras");
        }
         
    }
    public void StopSimulation()
    {
        if (m_AudioPlayer.IsLoopPlaying("RewindSimulation"))
            m_AudioPlayer.StopLoop("RewindSimulation");

        if (m_AudioPlayer.IsLoopPlaying("PlaySimulation"))
            m_AudioPlayer.StopLoop("PlaySimulation");
    }
    public void PlaySimulation()
    {
        //if(m_AudioPlayer.IsLoopPlaying("StopSimulation"))
        if(m_AudioPlayer.IsLoopPlaying("RewindSimulation"))
            m_AudioPlayer.StopLoop("RewindSimulation");

        m_AudioPlayer.PlayLoop("PlaySimulation");
    }

    public void RewindSimulation()
    {
        if (m_AudioPlayer.IsLoopPlaying("RewindSimulation"))
            m_AudioPlayer.StopLoop("RewindSimulation");

        m_AudioPlayer.PlayLoop("RewindSimulation");
    }
    
    public void PlayInsideAmbientSound()
    {
        if(m_AudioPlayer.IsLoopPlaying("AmbientWindOutside"))
            m_AudioPlayer.StopLoop("AmbientWindOutside");

        m_AudioPlayer.PlayLoop("AmbientWindInside");
        m_AudioPlayer.PlayLoop("AmbientWindInsideExtras");
    }
}
