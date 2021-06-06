using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioTriggerVolume : MonoBehaviour
{
    [Header("Parameters")]
    public bool OnlyPlayOnce = true;
    public AudioSource Source;
    StoryManager m_StoryManager;
    // Start is called before the first frame update
    void Start()
    {
        if(Source == null)
            Source = GetComponent<AudioSource>();

        Source.enabled = false;
        m_StoryManager = StoryManager.Instance;
    }

    private void Update()
    {
        if(Source != null && Source.enabled)
        {
            if (!m_StoryManager.AnimationState.Equals(AnimationState.Stop) && Source.isPlaying)
                Source.Pause();
            else if(m_StoryManager.AnimationState.Equals(AnimationState.Stop) && !Source.isPlaying)
                Source.Play();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Source.enabled = true;
            if (OnlyPlayOnce)
                StartCoroutine(DestroyComponent());
        }
    }
    
    IEnumerator DestroyComponent()
    {
        yield return new WaitForSeconds(Source.clip.length);
        Destroy(Source);
        Destroy(this);
    }
}
