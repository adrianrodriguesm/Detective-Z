using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioTriggerVolume : MonoBehaviour
{
    [Header("Parameters")]
    public bool OnlyPlayOnce = true;
    public AudioSource Source;
    // Start is called before the first frame update
    void Start()
    {
        if(Source == null)
            Source = GetComponent<AudioSource>();

        Source.enabled = false;
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
