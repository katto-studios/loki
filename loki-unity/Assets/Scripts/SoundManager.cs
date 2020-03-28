using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{

    public AudioMixer mixer;
    public Slider sliderMaster;
    public Slider sliderEffect;
    public Slider sliderMusic;

    private void Start()
    {
        if (FindObjectsOfType<SoundManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Object.DontDestroyOnLoad(gameObject);
        }

        SetLevelMaster(PlayerPrefs.GetFloat("MasterVolume", 1.0f));
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        SetLevelEffect(PlayerPrefs.GetFloat("EffectVolume", 1.0f));
        sliderEffect.value = PlayerPrefs.GetFloat("EffectVolume", 1.0f);
        SetLevelMusic(PlayerPrefs.GetFloat("MusicVolume", 1.0f));
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    }

    public void SetLevelMaster(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetLevelEffect(float sliderValue)
    {
        mixer.SetFloat("EffectVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("EffectVolume", sliderValue);
    }

    public void SetLevelMusic(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}
