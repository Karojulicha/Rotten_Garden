using UnityEngine.UI;
using UnityEngine;

public class AudioSettingsUI : MonoBehaviour
{

    public Slider musicSlider;
    public Slider sfxSlider;
 
    void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);

        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSfxVolume;

        // Aplicar los valores cargados al AudioManager
        AudioManager.Instance.setMusicVolume(savedMusicVolume);
        AudioManager.Instance.setSfxVolume(savedSfxVolume);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.Instance.setMusicVolume(volume);
        PlayerPrefs.SetFloat("VolumeMusic", volume);
    }

    void SetSFXVolume(float volume)
    {
        AudioManager.Instance.setSfxVolume(volume);
        PlayerPrefs.SetFloat("VolumeSfx", volume); // save volume
    }
}
