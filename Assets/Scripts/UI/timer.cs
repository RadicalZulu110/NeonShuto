using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    private float seconds;
    private int min;
    public  int hour;
    private int day;
    public float timeMod;

    public Text TimeDisplay;
    public Text SecDisplay;

    public GameObject FailedQuota;

    private void Update()
    {

        seconds += Time.deltaTime * timeMod;
        //sec = tick
        if (seconds >= 60)
        {
            min += (int)(seconds / 60);
            seconds %= 60;
        }
        //min = cycle
        if (min >= 60)
        {
            hour += min / 60;
            min %= 60;  
        }

        /*
        if (hour >= 24)
        {
            day += hour / 24;
            hour %= 24;
        }
        */

        TimeDisplay.text = min.ToString() + " : " + hour.ToString(); //+ " : " + day.ToString();
    }
}

