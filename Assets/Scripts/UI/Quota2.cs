using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quota2 : MonoBehaviour
{
    public int secondQuota;

    public int timeLimit;

    public Text goldQuotaDisplay;
    public Text foodQuotaDisplay;
    public Text stoneQuotaDisplay;
    public Text crystalQuotaDisplay;

    public GameManager gameManager;
    public timer timer;
    public GameObject QuotaTwo;
    public GameObject QuotaThree;
    public GameObject QuotaTwoComplete;
    public GameObject FailedQuota;

    void Update()
    {
        goldQuotaDisplay.text = gameManager.TotalGold.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.TotalFood.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.TotalStone.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.TotalCrystal.ToString() + "/" + "(" + (secondQuota).ToString() + ")";

        if (timer.hour >= timeLimit)
        {
            Time.timeScale = 0;
            FailedQuota.SetActive(true);
        }
    }

    public void OnMouseDown()
    {
        if (gameManager.TotalGold >= secondQuota && gameManager.TotalFood >= secondQuota && gameManager.TotalStone >= secondQuota && gameManager.TotalCrystal >= secondQuota)
        {
            gameManager.TotalGold -= secondQuota;
            gameManager.TotalFood -= secondQuota;
            gameManager.TotalStone -= secondQuota;
            gameManager.TotalCrystal -= secondQuota;

            QuotaTwo.SetActive(false);
            QuotaThree.SetActive(true);
            QuotaTwoComplete.SetActive(true);

            Time.timeScale = 0;
        }
    }
}
