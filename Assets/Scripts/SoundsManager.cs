using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Serializable]
    public class AudioClipInfo
    {
        public string name;
        public AudioClip clip;
    }

    public AudioClipInfo[] musicClips;
    public AudioClipInfo[] sfxClips;

    private Dictionary<string, AudioClip> musicDict;
    private Dictionary<string, AudioClip> sfxDict;

    public string currentlyMusicName;

    void Awake()
    {
        if(instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);

            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializeDictionaries()
    {
        musicDict = new Dictionary<string, AudioClip>();
        sfxDict = new Dictionary<string, AudioClip>();

        foreach (var item in musicClips)
        {
            musicDict[item.name] = item.clip;
        }
        foreach (var item in sfxClips)
        {
            sfxDict[item.name] = item.clip;
        }
    }
    public void PlayMusic(string name)
    {
        if (musicDict.ContainsKey(name))
        {
            musicSource.clip = musicDict[name];
            currentlyMusicName = name;
            StartCoroutine(FadeIn(musicSource, 1f));
        }
        else
        {
            Debug.LogWarning("Musique non trouvée : " + name);
        }
    }
    public void StopMusic(string name)
    {
        if (musicDict.ContainsKey(name))
        {
            musicSource.clip = musicDict[name];
            StartCoroutine(FadeOut(musicSource, 1f));
        }
        else
        {
            Debug.LogWarning("Musique à stopper non trouvée : " + name);
        }
    }
    public void PlaySFX(string name, float volume)
    {
        if (sfxDict.ContainsKey(name))
        {
            sfxSource.volume = volume;
            sfxSource.PlayOneShot(sfxDict[name]);
        }
        else
        {
            Debug.LogWarning("SFX non trouvé : " + name);
        }
    }
     public void PauseAllAudio()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
        if (sfxSource.isPlaying)
        {
            sfxSource.Pause();
        }
    }
    public void ResumeAllAudio()
    {
        if (!musicSource.isPlaying && currentlyMusicName != null)
        {
            musicSource.UnPause();
        }
        if (!sfxSource.isPlaying)
        {
            sfxSource.UnPause();
        }
    }
    IEnumerator FadeIn(AudioSource music, float duration)
    {
        music.volume = 0.0f;
        musicSource.Play();
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(0, 0.25f, timer/duration);
            yield return null;
        }
        music.volume = 0.25f;
    }
    IEnumerator FadeOut(AudioSource music, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(0.25f, 0.0f, timer / duration);
            yield return null;
        }
        music.volume = 0.0f;
        music.Stop();
    }
}
