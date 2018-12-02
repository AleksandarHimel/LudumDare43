using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public List<AudioClip> ErrorAudio;
    public List<AudioClip> OkAudio;
    public List<AudioClip> SfxAudio;
    public AudioClip BackgroundMusic;

    public AudioSource BackgroundAudioSource;
    public AudioSource SfxAudioSource;

    private IEnumerator fadeOutEnumerator;
    private bool fadeOutInProgress = false;
    private IEnumerator fadeInEnumerator;
    private bool fadeInInProgress = false;

    private void Awake()
    {
        GameObject audioSourceObject = new GameObject("_audioController");
        BackgroundAudioSource = audioSourceObject.AddComponent<AudioSource>();
        SfxAudioSource = audioSourceObject.AddComponent<AudioSource>();
        BackgroundMusic = Resources.Load<AudioClip>("Audio/MusicTheme");
        BackgroundAudioSource.clip = BackgroundMusic;
        BackgroundAudioSource.loop = true;
        BackgroundAudioSource.clip = BackgroundMusic;

        fadeInEnumerator = GetBackgroundMusicFadeInEnumerator();
        fadeOutEnumerator = GetBackgroundMusicFadeOutEnumerator();
    }

    public void FadeInBackgroundMusic()
    {
        // Stop backround fadeout if running
        if (fadeOutInProgress)
            StopCoroutine(fadeOutEnumerator);

        // Start fade in coroutine
        fadeInEnumerator = GetBackgroundMusicFadeInEnumerator();
        StartCoroutine(fadeInEnumerator);
    }

    public void FadeOutBackgroundMusic()
    {
        // Stop backround fadeout if running
        if (fadeInInProgress)
            StopCoroutine(fadeInEnumerator);
        // Stop backround music
        fadeOutEnumerator = GetBackgroundMusicFadeOutEnumerator();
        StartCoroutine(fadeOutEnumerator);
    }

    private IEnumerator GetBackgroundMusicFadeOutEnumerator()
    {
        fadeOutInProgress = true;

        float FadeTime = 3.0f;
        float startVolume = BackgroundAudioSource.volume;

        while (BackgroundAudioSource.volume > 0)
        {
            BackgroundAudioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        BackgroundAudioSource.Stop();
        BackgroundAudioSource.volume = startVolume;
        fadeOutInProgress = false;
    }

    private IEnumerator GetBackgroundMusicFadeInEnumerator()
    {
        fadeInInProgress = true;

        float FadeTime = 3.0f;
        float startVolume = 0.2f;

        BackgroundAudioSource.volume = 0;
        BackgroundAudioSource.Play();

        while (BackgroundAudioSource.volume < 1.0f)
        {
            BackgroundAudioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        BackgroundAudioSource.volume = 1f;
        fadeInInProgress = false;
    }
}
