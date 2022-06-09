using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public static float lastSliderValue;

    /*
    public string Channel;
    public GameObject DataManager;
    public GameObject soundSliderObject;
    Slider slider;
    
    public void Awake()
    {
                DontDestroyOnLoad(this.gameObject);
    }
    public void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();
        slider.v
    }
    */

    private void Update()
    {
        
    }

    public void SetMasterLevel(float sliderValue)
    {
        //Slider slider = this.gameObject.GetComponent<Slider>();
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        //DataManager.GetComponent<DataManager>().masterVolume = Mathf.Log10(sliderValue) * 20;
        //slider.value = Mathf.Log10(sliderValue) * 20;
    }
    public void SetMusicLevel (float sliderValue)
    {
        //Slider slider = this.gameObject.GetComponent<Slider>();
        mixer.SetFloat("MusicVol", Mathf.Log10 (sliderValue) * 20);
        //DataManager.GetComponent<DataManager>().soundVolume = Mathf.Log10(sliderValue) * 20;
        //slider.value = Mathf.Log10(sliderValue) * 20;
    }
    public void SetSFXLevel(float sliderValue)
    {
        //Slider slider = this.gameObject.GetComponent<Slider>();
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        //DataManager.GetComponent<DataManager>().sfxVolume = Mathf.Log10(sliderValue) * 20;
        //slider.value = Mathf.Log10(sliderValue) * 20;
    }

    public void SetSliderValue(float sliderValue)
    {
        lastSliderValue = sliderValue;
    }
}
