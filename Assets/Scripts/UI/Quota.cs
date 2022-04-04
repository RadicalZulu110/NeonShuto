using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quota : MonoBehaviour
{
    public int firstQuota;

    public int timeLimit;

    public Text goldQuotaDisplay;
    public Text foodQuotaDisplay;
    public Text stoneQuotaDisplay;
    public Text crystalQuotaDisplay;

    public GameManager gameManager;
    public timer timer;
    public GameObject QuotaOne;
    public GameObject QuotaTwo;
    public GameObject QuotaOneComplete;
    public GameObject FailedQuota;


    void Update()
    {
        goldQuotaDisplay.text = gameManager.gold.ToString() + "/" + "(" + (firstQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.food.ToString() + "/" + "(" + (firstQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.stone.ToString() + "/" + "(" + (firstQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.crystal.ToString() + "/" + "(" + (firstQuota).ToString() + ")";

        if (timer.hour >= timeLimit)
        {
            Time.timeScale = 0;
            FailedQuota.SetActive(true);
        }
    }

    public void OnMouseDown()
    {
        if (gameManager.gold >= firstQuota && gameManager.food >= firstQuota && gameManager.stone >= firstQuota && gameManager.crystal >= firstQuota)
        {
            gameManager.gold -= firstQuota;
            gameManager.food -= firstQuota;
            gameManager.stone -= firstQuota;
            gameManager.crystal -= firstQuota;

            QuotaOne.SetActive(false);
            QuotaTwo.SetActive(true);
            QuotaOneComplete.SetActive(true);

            Time.timeScale = 0;
        }
    }
}
