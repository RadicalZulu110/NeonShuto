using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quota3 : MonoBehaviour
{
	[SerializeField]
	private StartingConstruction heroBuilding;
	private List<FoodStorageBuilding> foodStorageBuildings;
	private List<ResourceStorageBuilding> resourceStorageBuildings;
	private BuildingCost building;

	public int minQuota, maxQuota;

	public int timeLimit;

	public int goldQuota, foodQuota, stoneQuota, crystalQuota;

	public Text goldQuotaDisplay;
    public Text foodQuotaDisplay;
    public Text stoneQuotaDisplay;
    public Text crystalQuotaDisplay;

    public GameManager gameManager;
    public timer timer;
    public GameObject QuotaThree;
    public GameObject QuotaThreeComplete;
    public GameObject FailedQuota;

    private void Start()
    {
		foodStorageBuildings = new List<FoodStorageBuilding>();
		resourceStorageBuildings = new List<ResourceStorageBuilding>();
		GenerateQuotas();
	}

    void Update()
    {
		if (heroBuilding == null)
		{
			heroBuilding = GameObject.FindGameObjectWithTag("HeroBuilding").GetComponent<StartingConstruction>();
		}

		goldQuotaDisplay.text = gameManager.TotalGold.ToString() + "/" + "(" + (goldQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.TotalFood.ToString() + "/" + "(" + (foodQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.TotalStone.ToString() + "/" + "(" + (stoneQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.TotalCrystal.ToString() + "/" + "(" + (crystalQuota).ToString() + ")";

        if (timer.hour >= timeLimit)
        {
            Time.timeScale = 0;
            FailedQuota.SetActive(true); 
        }
    }

    public void OnMouseDown()
    {
        if (gameManager.TotalGold >= goldQuota && gameManager.TotalFood >= foodQuota && gameManager.TotalStone >= stoneQuota && gameManager.TotalCrystal >= crystalQuota)
        {
			// FOOD CRYSTAL AND STONE
			float percent = 0;
			FoodStorageBuilding foodStorBuild = null;
			ResourceStorageBuilding resoStorBuild = null;

			if (heroBuilding)
			{
				if (gameManager.TotalFood >= foodQuota)
				{
					// Get the food storage with more percentage
					percent = heroBuilding.GetFoodPercentage();
					foodStorBuild = getMaxFoodStoragePercetnage();
					if (foodStorBuild && foodStorBuild.GetFoodPercentage() > percent)
					{
						foodStorBuild.addFood(-foodQuota);
					}
					else
					{
						heroBuilding.addFood(-foodQuota);
					}

					percent = 0;
					foodStorBuild = null;
				}


				if (gameManager.TotalStone >= stoneQuota)
				{
					// Get the stone storage with more percentage
					percent = heroBuilding.GetStonePercentage();
					resoStorBuild = getMaxStoneStoragePercetnage();
					if (resoStorBuild && resoStorBuild.GetStonePercentage() > percent)
					{
						resoStorBuild.addStone(-stoneQuota);
					}
					else
					{
						heroBuilding.addStone(-stoneQuota);
					}

					percent = 0;
					resoStorBuild = null;
				}


				if (gameManager.TotalCrystal >= crystalQuota)
				{
					// Get the crystal storage with more percentage
					percent = heroBuilding.GetCrystalPercentage();
					resoStorBuild = getMaxCrystalStoragePercetnage();
					if (resoStorBuild && resoStorBuild.GetCrystalPercentage() > percent)
					{
						resoStorBuild.addCrystal(-crystalQuota);
					}
					else
					{
						heroBuilding.addCrystal(-crystalQuota);
					}

					percent = 0;
					resoStorBuild = null;
				}

			}

			gameManager.TotalGold -= goldQuota;
            //gameManager.TotalFood -= thirdQuota;
            //gameManager.TotalStone -= thirdQuota;
            //gameManager.TotalCrystal -= thirdQuota;

            QuotaThree.SetActive(false);
            QuotaThreeComplete.SetActive(true);

            Time.timeScale = 0;
        }
    }

	// Get the food storage with more percentage
	private FoodStorageBuilding getMaxFoodStoragePercetnage()
	{
		if (foodStorageBuildings.Count == 0)
			return null;

		float max = 0;
		FoodStorageBuilding maxObject = null;

		for (int i = 0; i < foodStorageBuildings.Count; i++)
		{
			if (foodStorageBuildings[i].GetFoodPercentage() > max || max == 0)
			{
				maxObject = foodStorageBuildings[i];
				max = maxObject.GetFoodPercentage();
			}

			if (max == 100)
				break;

		}

		return maxObject;
	}

	// Get the stone storage with more percentage
	private ResourceStorageBuilding getMaxStoneStoragePercetnage()
	{
		if (resourceStorageBuildings.Count == 0)
			return null;

		float max = 0;
		ResourceStorageBuilding maxObject = null;

		for (int i = 0; i < resourceStorageBuildings.Count; i++)
		{
			if (resourceStorageBuildings[i].GetStonePercentage() > max || max == 0)
			{
				maxObject = resourceStorageBuildings[i];
				max = maxObject.GetStonePercentage();
			}

			if (max == 100)
				break;

		}

		return maxObject;
	}

	// Get the crystal storage with more percentage
	private ResourceStorageBuilding getMaxCrystalStoragePercetnage()
	{
		if (resourceStorageBuildings.Count == 0)
			return null;

		float max = 0;
		ResourceStorageBuilding maxObject = null;

		for (int i = 0; i < resourceStorageBuildings.Count; i++)
		{
			if (resourceStorageBuildings[i].GetCrystalPercentage() > max || max == 0)
			{
				maxObject = resourceStorageBuildings[i];
				max = maxObject.GetCrystalPercentage();
			}

			if (max == 100)
				break;

		}

		return maxObject;
	}

	private void GenerateQuotas()
	{
		goldQuota = Random.Range(minQuota, maxQuota);
		foodQuota = Random.Range(minQuota, maxQuota);
		stoneQuota = Random.Range(minQuota, maxQuota);
		crystalQuota = Random.Range(minQuota, maxQuota);
	}
}
