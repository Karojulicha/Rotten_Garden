using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Clips")]
    public AudioClip buttonClick;
    public AudioClip sfxHealing;
    public AudioClip sfxWorms;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonSound()
    {
        PlaySFX(buttonClick);
    }

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("VolumeMusic", Mathf.Log10(volume) * 20);
    }

    public void setSfxVolume(float volume)
    {
        audioMixer.SetFloat("VolumeSfx", Mathf.Log10(volume) * 20);
    }

    public void setUIvolume(float volume)
    {
        audioMixer.SetFloat("VolumeUi", Mathf.Log10(volume) * 20);
    }

}