using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightTriggerVolume : MonoBehaviour
{
    [Header("Parameters")]
    public bool OnlyPlayOnce = true;
    public Light2D Source;
    public float TimeToDestroy = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (!Source)
            Source = GetComponent<Light2D>();

        Source.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Source.enabled = true;
            if (OnlyPlayOnce)
                StartCoroutine(DestroyComponent());
        }
    }

    IEnumerator DestroyComponent()
    {
        yield return new WaitForSeconds(TimeToDestroy);
        Destroy(Source);
        Destroy(this);
    }
}
