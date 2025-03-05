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

    [Header("Master Audio")]
    [SerializeField] AudioMixer masterAudio;
    [Range(-5, 5)]

    [Header("Audio Clips")]
    public AudioClip buttonClick;
    public AudioClip sfxHealing;
    public AudioClip sfxWorms;
    public AudioClip musicClip;

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
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
        uiSource = transform.GetChild(2).GetComponent<AudioSource>();
        InitialPlayMusic(musicClip);
    }


    void InitialPlayMusic(AudioClip audioClip)
    {
        musicSource.Stop();
        musicSource.clip = audioClip;
        musicSource.Play();
        musicSource.loop = true;

    }


    // public void MuteAll()
    // {
    //     isMute = !isMute;
    //     if (isMute)
    //     {
    //         master.SetFloat("Master", -80f);
    //     }
    //     else
    //     {
    //         master.SetFloat("Master", 0f);
    //     }
    // }

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
        masterAudio.SetFloat("VolumeMusic", Mathf.Log10(volume) * 20);
    }

    public void setSfxVolume(float volume)
    {
        masterAudio.SetFloat("VolumeSfx", Mathf.Log10(volume) * 20);
    }

    public void setUIvolume(float volume)
    {
        masterAudio.SetFloat("VolumeUi", Mathf.Log10(volume) * 20);
    }

}