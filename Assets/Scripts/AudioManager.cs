using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    private AudioSource musicSource, musicSource2;
    private AudioSource ambientSource;
    private AudioSource [] sfxSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource2 = gameObject.AddComponent<AudioSource>();
            ambientSource = gameObject.AddComponent<AudioSource>();
            sfxSource = new AudioSource[10];
            for (int i = 0; i < sfxSource.Length; i++)
            {
                sfxSource[i] = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip _clip, bool _fade = false, float _fadeDuration = 1)
    {
        if (_fade == true)
        {
            if (musicSource.isPlaying)
            {
                musicSource2.clip = _clip;
                musicSource2.loop = true;
                musicSource2.Play();
                StartCoroutine(FadeMusic(musicSource, musicSource2, _fadeDuration));
            }
            else
            {
                musicSource.clip = _clip;
                musicSource.loop = true;
                musicSource.Play();
                StartCoroutine(FadeMusic(musicSource2, musicSource, _fadeDuration));
            }
        }
        else
        {
            musicSource.clip = _clip;
            musicSource.loop = true;
            musicSource.Play();
            musicSource2.Stop();
        }
    }

    public void PlayAmbient(AudioClip _clip)
    {
        ambientSource.clip = _clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void PlaySfx(AudioClip _clip)
    {
        for (int i = 0; i < sfxSource.Length; i++)
        {
            if (sfxSource[i].isPlaying == false)
            {
                sfxSource[i].clip = _clip;
                sfxSource[i].loop = true;
                sfxSource[i].Play();
                break;
            }
        }
    }

    IEnumerator FadeMusic(AudioSource _current, AudioSource _next, float _fadeDuration)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _next.volume = Mathf.Lerp(0, 1, t);
            _current.volume = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }
}
