using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public Slider slider;
    public GameObject HalfAlert;
    public GameObject QuarterAlert;

    public void Update()
    {
        HalfHealth();
        QuarterHealth();
    }

    public void HalfHealth()
    {
        if(slider.value <= 200 && slider.value > 150)
        {
            HalfAlert.SetActive(true);
        }
        else
        {
            HalfAlert.SetActive(false);
        }
    }

    public void QuarterHealth()
    {
        if(slider.value <= 150)
        {
            QuarterAlert.SetActive(true);
        }
        else
        {
            QuarterAlert.SetActive(false);
        }
    }
}
