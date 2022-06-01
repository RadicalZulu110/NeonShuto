using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public float masterVolume;
    public float soundVolume;
    public float sfxVolume;

    public GameObject MasterMixer;
    public GameObject SoundMixer;
    public GameObject SfxMixer;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        masterVolume = MasterMixer.GetComponent<Slider>().value;
        soundVolume = SoundMixer.GetComponent<Slider>().value;
        sfxVolume = SfxMixer.GetComponent<Slider>().value;
    }
}
