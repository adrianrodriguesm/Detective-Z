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
    bool m_First = false;
    AnimationState m_CurrentAnimationState;
    float m_MainSoundStartVolume;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioPlayer = GetComponent<AudioPlayer>();
        m_Player = FindObjectOfType<PlayerController>();
        PlayInsideAmbientSound();
       
    }

    public void DestroyAudioListener()
    {
        if(transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }

    public void PlayClickSound()
    {
        m_AudioPlayer.PlayOnce("Click");
    }

    public void PlayDialogueSound()
    {
        m_AudioPlayer.PlayOnce("DialagueTyping");
    }

    private void Update()
    {
        if (GameplayManager.Instance.GameplayStarted)
        {
            if(!m_First)
            {
                m_Player = FindObjectOfType<PlayerController>();
                m_AudioPlayer.PlayLoop("MainMusic");
                m_MainSoundStartVolume = m_AudioPlayer.GetLoopVolumeScale("MainMusic");
                if (m_Player.environment.Equals(EnvironmentType.Garden))
                    PlayOutsideAmbientSound();
                else
                    PlayInsideAmbientSound();

                
                m_First = true;
            }

            AnimationState currAnimState = StoryManager.Instance.AnimationState;
            if(!m_CurrentAnimationState.Equals(currAnimState) && !currAnimState.Equals(AnimationState.Stop))
            {
                if(m_AudioPlayer.IsLoopPlaying("MainMusic"))
                    m_AudioPlayer.StopLoop("MainMusic");

                StopAmbientSound();
            }
            else if(!m_CurrentAnimationState.Equals(currAnimState) && currAnimState.Equals(AnimationState.Stop))
            {
                StopSimulation();
                m_AudioPlayer.PlayLoop("MainMusic");
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
        if (m_AudioPlayer.IsLoopPlaying("MainMusic"))
            m_AudioPlayer.SetLoopVolumeScale("MainMusic", m_MainSoundStartVolume / 4, 0.35f);

        if(m_AudioPlayer.IsLoopPlaying("AmbientWindInside"))
        {
            m_AudioPlayer.StopLoop("AmbientWindInside");
            m_AudioPlayer.StopLoop("AmbientWindInsideExtras");
        }

        m_AudioPlayer.PlayLoop("AmbientWindOutside");
        m_AudioPlayer.PlayLoop("AmbientWindOutsideExtras");

    }

    public void StopAmbientSound()
    {
        if (m_AudioPlayer.IsLoopPlaying("AmbientWindOutside"))
        {
            m_AudioPlayer.StopLoop("AmbientWindOutside");
            m_AudioPlayer.StopLoop("AmbientWindOutsideExtras");
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
        {
            m_AudioPlayer.StopLoop("AmbientWindOutsideExtras");
            m_AudioPlayer.StopLoop("AmbientWindOutside");
        }

        if (m_AudioPlayer.IsLoopPlaying("MainMusic"))
            m_AudioPlayer.SetLoopVolumeScale("MainMusic", m_MainSoundStartVolume, 0.35f);

        m_AudioPlayer.PlayLoop("AmbientWindInside");
        m_AudioPlayer.PlayLoop("AmbientWindInsideExtras");
    }
}
