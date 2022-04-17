using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quota3 : MonoBehaviour
{
    public int thirdQuota;

    public int timeLimit;

    public Text goldQuotaDisplay;
    public Text foodQuotaDisplay;
    public Text stoneQuotaDisplay;
    public Text crystalQuotaDisplay;

    public GameManager gameManager;
    public timer timer;
    public GameObject QuotaThree;
    public GameObject QuotaThreeComplete;
    public GameObject FailedQuota;


    void Update()
    {
        goldQuotaDisplay.text = gameManager.TotalGold.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.TotalFood.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.TotalStone.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.TotalCrystal.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";

        if (timer.hour >= timeLimit)
        {
            Time.timeScale = 0;
            FailedQuota.SetActive(true); 
        }
    }

    public void OnMouseDown()
    {
        if (gameManager.TotalGold >= thirdQuota && gameManager.TotalFood >= thirdQuota && gameManager.TotalStone >= thirdQuota && gameManager.TotalCrystal >= thirdQuota)
        {
            gameManager.TotalGold -= thirdQuota;
            gameManager.TotalFood -= thirdQuota;
            gameManager.TotalStone -= thirdQuota;
            gameManager.TotalCrystal -= thirdQuota;

            QuotaThree.SetActive(false);
            QuotaThreeComplete.SetActive(true);

            Time.timeScale = 0;
        }
    }
}
