using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quota2 : MonoBehaviour
{
    public int secondQuota;

    public Text goldQuotaDisplay;
    public Text foodQuotaDisplay;
    public Text stoneQuotaDisplay;
    public Text crystalQuotaDisplay;

    public GameManager gameManager;
    public GameObject QuotaTwo;
    public GameObject QuotaThree;
    public GameObject QuotaTwoComplete;

    void Update()
    {
        goldQuotaDisplay.text = gameManager.gold.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.food.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.stone.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.crystal.ToString() + "/" + "(" + (secondQuota).ToString() + ")";
    }

    public void OnMouseDown()
    {
        if (gameManager.gold >= secondQuota && gameManager.food >= secondQuota && gameManager.stone >= secondQuota && gameManager.crystal >= secondQuota)
        {
            gameManager.gold -= secondQuota;
            gameManager.food -= secondQuota;
            gameManager.stone -= secondQuota;
            gameManager.crystal -= secondQuota;

            QuotaTwo.SetActive(false);
            QuotaThree.SetActive(true);
            QuotaTwoComplete.SetActive(true);
        }
    }
}
