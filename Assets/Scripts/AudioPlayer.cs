using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AudioClipNotFoundException : System.Exception
{
    public AudioClipNotFoundException(string clipName) : base("No clip defined with name \"" + clipName + "\"")
    {
    }
}

[System.Serializable]
public class AudioClipAlreadyExistsException : System.Exception
{
    public AudioClipAlreadyExistsException(string clipName) : base("There already is a clip registered with the name \"" + clipName + "\"")
    {
    }
}

[System.Serializable]
public class AudioClipVolume
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1;

    public AudioClipVolume(string name, AudioClip clip, float volume)
    {
        this.name = name;
        this.clip = clip;
        this.volume = volume;
    }
}

[System.Serializable]
public class RandomAudioClipVolume
{
    public string name;
    public List<AudioClip> clips;
    [Range(0, 1)] public float volume = 1;
    int LastIndex = -1;

    public RandomAudioClipVolume(string name, List<AudioClip> clips, float volume)
    {
        this.name = name;
        this.clips = clips;
        this.volume = volume;
    }
    public AudioClip SelectClipToPlay()
    {
        if(LastIndex < 0 || clips.Count() == 1)
            LastIndex = Random.Range(0, clips.Count());
        else
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, clips.Count());
            } while (newIndex == LastIndex);
            LastIndex = newIndex;
        }
        return clips[LastIndex];
    }
}

public class AudioPlayer : MonoBehaviour
{

    [Header("Settings")] public float FadeTime = 0.25f;

    public List<AudioClipVolume> AudioClips;
    public List<RandomAudioClipVolume> RandomAudioClips;
    private Dictionary<string, RandomAudioClipVolume> randomClips = new Dictionary<string, RandomAudioClipVolume>();
    public float TimeStamp = 0.0f;

    Dictionary<string, AudioClipVolume> clips = new Dictionary<string, AudioClipVolume>();
    Dictionary<string, AudioSource> loopSources = new Dictionary<string, AudioSource>();
    Dictionary<string, float> customVolumes = new Dictionary<string, float>();

    private List<AudioSource> oneShotSources = new List<AudioSource>();

    void Awake()
    {
        foreach (var clip in AudioClips)
        {
            clips[clip.name] = clip;
            customVolumes[clip.name] = 1;
        }
        foreach(var clip in RandomAudioClips)
        {
            randomClips[clip.name] = clip;
            customVolumes[clip.name] = 1;
        }
    }

    void Update()
    {
        //Debug.Log(categoryVolumes[AudioCategory.Music]);
        List<AudioSource> oneShotCopy = new List<AudioSource>(oneShotSources);
        foreach (var source in oneShotCopy)
        {
            if (!source.isPlaying)
            {
                oneShotSources.Remove(source);
                Destroy(source);
            }
        }
    }

    public void AddClip(AudioClipVolume clip)
    {
        if (clips.ContainsKey(clip.name))
        {
            throw new AudioClipAlreadyExistsException(clip.name);
        }
        else
        {
            AudioClips.Add(clip);
            clips[clip.name] = clip;
            customVolumes[clip.name] = 1;
        }
    }

    public bool IsLoopPlaying(string name)
    {
        return loopSources.ContainsKey(name);
    }

    public void PlayOnce(string name)
    {
        PlayOnce(name, 1);
    }

    public void PlayOnce(string name, float volumeScale)
    {
        if (clips.ContainsKey(name))
        {
            AudioClipVolume clip = clips[name];
            var source = gameObject.AddComponent<AudioSource>();
            source.volume = clip.volume * customVolumes[name] * volumeScale;
            source.clip = clip.clip;
            source.loop = false;
            oneShotSources.Add(source);
            source.Play();

        }
        else
        {
            throw new AudioClipNotFoundException(name);
        }
    }

    public void PlayOnceRandomClip(string name)
    {
        if (randomClips.ContainsKey(name))
        {
            RandomAudioClipVolume clip = randomClips[name];
            var source = gameObject.AddComponent<AudioSource>();
            source.volume = clip.volume * customVolumes[name];
            source.clip = clip.SelectClipToPlay();
            source.loop = false;
            oneShotSources.Add(source);
            source.Play();

        }
        else
        {
            throw new AudioClipNotFoundException(name);
        }
    }

    public void PauseLoop(string name)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            source.Pause();
        }
        else throw new AudioClipNotFoundException(name);
    }

    public void ContinueLoop(string name)
    {
        if (clips.ContainsKey(name))
        {
            if (!loopSources[name].isPlaying)
            {
                var source = loopSources[name];
                source.Play();
            }

        }
        else throw new AudioClipNotFoundException(name);
    }

    public void PlayLoop(string name)
    {
        PlayLoop(name, FadeTime);
    }

    public void PlayLoop(string name, float FadeTime)
    {
        if (clips.ContainsKey(name))
        {
            if (!loopSources.ContainsKey(name))
            {
                AudioClipVolume clip = clips[name];
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = clip.clip;
                source.loop = true;

                loopSources[name] = source;
                source.time = TimeStamp;
                source.Play();

                StartCoroutine(FadeIn(source, clip.volume * customVolumes[name], FadeTime));
            }
        }
        else
        {
            throw new AudioClipNotFoundException(name);
        }
    }


    public void StopLoop(string name)
    {
        StopLoop(name, FadeTime);
    }

    public void StopLoop(string name, float FadeTime)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            loopSources.Remove(name);
            StartCoroutine(FadeOut(source, FadeTime));
        }
        else throw new AudioClipNotFoundException(name);
    }

    public void SetLoopTime(string name, float time)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            source.time = time;
        }
        else throw new AudioClipNotFoundException(name);
    }

    public float GetLoopTime(string name)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            return source.time;
        }
        else throw new AudioClipNotFoundException(name);
    }

    public void SetLoopVolumeScale(string name, float volume)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            customVolumes[name] = volume;
            if(source.volume > volume)
                StartCoroutine(FadeIn(source, volume * clips[name].volume, FadeTime));
            else
                StartCoroutine(FadeOutWithoutDestroy(source, volume * clips[name].volume, FadeTime));
        }
        else throw new AudioClipNotFoundException(name);
    }
    public void SetLoopVolumeScale(string name, float volume, float fadeTime)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            customVolumes[name] = volume;
            if (source.volume > volume)
                StartCoroutine(FadeIn(source, volume * clips[name].volume, FadeTime));
            else
                StartCoroutine(FadeOutWithoutDestroy(source, volume * clips[name].volume, fadeTime));
        }
        else throw new AudioClipNotFoundException(name);
    }
    public float GetLoopVolumeScale(string name)
    {
        if (clips.ContainsKey(name))
        {
            return customVolumes[name];
        }
        else throw new AudioClipNotFoundException(name);
    }

    public void DoFadeIn(string name)
    {
        if (clips.ContainsKey(name))
        {
            AudioClipVolume clip = clips[name];
            var source = loopSources[name];
            StartCoroutine(FadeIn(source, clip.volume * customVolumes[clip.name] , FadeTime));
        }
        else throw new AudioClipNotFoundException(name);
    }

    public void DoFadeOut(string name)
    {
        if (clips.ContainsKey(name))
        {
            var source = loopSources[name];
            StartCoroutine(FadeOutWithoutDestroy(source, FadeTime));
        }
        else throw new AudioClipNotFoundException(name);
    }
    IEnumerator FadeOutWithoutDestroy(AudioSource source, float FadeTime)
    {
        float startVol = source.volume;
        float t = 0;
        while (t < FadeTime)
        {
            source.volume = Mathf.Lerp(startVol, 0, t / FadeTime);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
    }
    IEnumerator FadeOutWithoutDestroy(AudioSource source, float volume, float FadeTime)
    {
        float startVol = source.volume;
        float t = 0;
        while (t < FadeTime)
        {
            source.volume = Mathf.Lerp(startVol, volume, t / FadeTime);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
    }
    IEnumerator FadeOut(AudioSource source, float FadeTime)
    {
        float startVol = source.volume;
        float t = 0;
        while (t < FadeTime)
        {
            source.volume = Mathf.Lerp(startVol, 0, t / FadeTime);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        Destroy(source);
    }

    IEnumerator FadeIn(AudioSource source, float volume, float FadeTime)
    {
        float t = 0;
        source.volume = 0;
        while (t < FadeTime)
        {
            source.volume = Mathf.Lerp(0, volume, t / FadeTime);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
    }

    public float GetAudioClipLength(string name)
    {
        if (clips.ContainsKey(name))
        {
            if (!loopSources.ContainsKey(name))
            {
                AudioClipVolume clip = clips[name];
                return clip.clip.length;
            }

        }

        return -1f;
    }
}