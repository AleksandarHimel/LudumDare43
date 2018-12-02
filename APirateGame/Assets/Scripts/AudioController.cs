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
    private IEnumerator fadeInEnumerator;

    // Use this for initialization
    void Start()
    {
        GameObject audioSourceObject = new GameObject();
        BackgroundAudioSource = audioSourceObject.AddComponent<AudioSource>();

        SfxAudioSource = audioSourceObject.AddComponent<AudioSource>();

        BackgroundMusic = Resources.Load<AudioClip>("Audio/MusicTheme");
        BackgroundAudioSource.clip = BackgroundMusic;
        BackgroundAudioSource.loop = true;
    }

    public void FadeInBackgroundMusic()
    {
        // Stop backround fadeout if running
        StopCoroutine(fadeOutEnumerator);
        // Start fade in coroutine
        fadeInEnumerator = GetBackgroundMusicFadeInEnumerator();
        StartCoroutine(fadeInEnumerator);
    }


    public void FadeOutBackgroundMusic()
    {
        // Stop backround fadeout if running
        StopCoroutine(fadeInEnumerator);
        // Stop backround music
        fadeOutEnumerator = GetBackgroundMusicFadeOutEnumerator();
        StartCoroutine(fadeOutEnumerator);
    }

    private IEnumerator GetBackgroundMusicFadeOutEnumerator()
    {
        float FadeTime = 3.0f;
        float startVolume = BackgroundAudioSource.volume;

        while (BackgroundAudioSource.volume > 0)
        {
            BackgroundAudioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        BackgroundAudioSource.Stop();
        BackgroundAudioSource.volume = startVolume;
    }

    private IEnumerator GetBackgroundMusicFadeInEnumerator()
    {
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
    }
}
