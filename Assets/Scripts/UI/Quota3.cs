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
        goldQuotaDisplay.text = gameManager.gold.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.food.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.stone.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.crystal.ToString() + "/" + "(" + (thirdQuota).ToString() + ")";

        if (timer.hour >= timeLimit)
        {
            Time.timeScale = 0;
            FailedQuota.SetActive(true); 
        }
    }

    public void OnMouseDown()
    {
        if (gameManager.gold >= thirdQuota && gameManager.food >= thirdQuota && gameManager.stone >= thirdQuota && gameManager.crystal >= thirdQuota)
        {
            gameManager.gold -= thirdQuota;
            gameManager.food -= thirdQuota;
            gameManager.stone -= thirdQuota;
            gameManager.crystal -= thirdQuota;

            QuotaThree.SetActive(false);
            QuotaThreeComplete.SetActive(true);

            Time.timeScale = 0;
        }
    }
}
