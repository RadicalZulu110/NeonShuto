using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }

    public void PauseGame()
    { 
         Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void FastForward()
    {
        Time.timeScale = 2;
    }
}
