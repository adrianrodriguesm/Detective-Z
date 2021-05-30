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
    AudioListener m_Listener;
    bool m_First = false;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioPlayer = GetComponent<AudioPlayer>();

        PlayInsideAmbientSound();
    }

    private void Update()
    {
        if (GameplayManager.Instance.GameplayStarted && !m_First)
        {
            m_Listener.enabled = false;
            m_First = true;
        }
            
    }

    public void PlayOutsideAmbientSound()
    {
        m_AudioPlayer.StopLoop("AmbientWindInside");
        m_AudioPlayer.StopLoop("AmbientWindInsideExtras");

        m_AudioPlayer.PlayLoop("AmbientWindOutside");
       
    }

    
    public void PlayInsideAmbientSound()
    {
        if(m_AudioPlayer.IsLoopPlaying("AmbientWindOutside"))
            m_AudioPlayer.StopLoop("AmbientWindOutside");

        m_AudioPlayer.PlayLoop("AmbientWindInside");
        m_AudioPlayer.PlayLoop("AmbientWindInsideExtras");
    }
}
