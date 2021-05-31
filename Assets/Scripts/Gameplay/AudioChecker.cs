using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class AudioChecker : MonoBehaviour
    {
        StoryManager m_StoryManager;
        AudioSource m_AudioSource;
        AnimationState m_CurrentAnimationState;
        // Use this for initialization
        void Start()
        {
            m_StoryManager = StoryManager.Instance;
            m_AudioSource = GetComponent<AudioSource>();
            m_CurrentAnimationState = AnimationState.Stop;
        }

        // Update is called once per frame
        void Update()
        {
            AnimationState currAnimState = m_StoryManager.AnimationState;
            if (!m_CurrentAnimationState.Equals(currAnimState) && !currAnimState.Equals(AnimationState.Stop))
                m_AudioSource.enabled = false;
            else if (!m_CurrentAnimationState.Equals(currAnimState) && currAnimState.Equals(AnimationState.Stop))
                m_AudioSource.enabled = true;

        }
    }
}