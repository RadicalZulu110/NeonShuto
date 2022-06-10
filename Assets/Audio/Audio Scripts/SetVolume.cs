using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;

    public static float lastSliderValue;

    private void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    public void SetMasterLevel(float sliderValue)
    { 
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
    public void SetMusicLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10 (sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void SetSFXLevel(float sliderValue)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void SetSliderValue(float sliderValue)
    {
        lastSliderValue = sliderValue;
    }
}
