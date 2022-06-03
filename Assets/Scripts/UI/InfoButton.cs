using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : MonoBehaviour
{
    public void Info()
    {
        Time.timeScale = 0;
    }

    public void exit()
    {
        Time.timeScale = 1;
    }
}
