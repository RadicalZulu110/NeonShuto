using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quota : MonoBehaviour
{
	[SerializeField]
	private StartingConstruction heroBuilding;
	private List<FoodStorageBuilding> foodStorageBuildings;
	private List<ResourceStorageBuilding> resourceStorageBuildings;
	private BuildingCost building;

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

	private void Start()
	{
		foodStorageBuildings = new List<FoodStorageBuilding>();
		resourceStorageBuildings = new List<ResourceStorageBuilding>();
	}


	void Update()
    {
		if(heroBuilding == null)
        {
			heroBuilding = GameObject.FindGameObjectWithTag("HeroBuilding").GetComponent<StartingConstruction>();
        }

        goldQuotaDisplay.text = gameManager.TotalGold.ToString() + "/" + "(" + (firstQuota).ToString() + ")";
        foodQuotaDisplay.text = gameManager.TotalFood.ToString() + "/" + "(" + (firstQuota).ToString() + ")";
        stoneQuotaDisplay.text = gameManager.TotalStone.ToString() + "/" + "(" + (firstQuota).ToString() + ")";
        crystalQuotaDisplay.text = gameManager.TotalCrystal.ToString() + "/" + "(" + (firstQuota).ToString() + ")";

        if (timer.hour >= timeLimit)
        {
            Time.timeScale = 0;
            FailedQuota.SetActive(true);
        }
    }

    public void OnMouseDown()
    {
        if (gameManager.TotalGold >= firstQuota && gameManager.TotalFood >= firstQuota && gameManager.TotalStone >= firstQuota && gameManager.TotalCrystal >= firstQuota)
        {
			// FOOD CRYSTAL AND STONE
			float percent = 0;
			FoodStorageBuilding foodStorBuild = null;
			ResourceStorageBuilding resoStorBuild = null;

			if (heroBuilding)
			{
				if (gameManager.TotalFood >= firstQuota)
				{
					// Get the food storage with more percentage
					percent = heroBuilding.GetFoodPercentage();
					foodStorBuild = getMaxFoodStoragePercetnage();
					if (foodStorBuild && foodStorBuild.GetFoodPercentage() > percent)
					{
						foodStorBuild.addFood(-firstQuota);
					}
					else
					{
						heroBuilding.addFood(-firstQuota);
					}

					percent = 0;
					foodStorBuild = null;
				}


				if (gameManager.TotalStone >= firstQuota)
				{
					// Get the stone storage with more percentage
					percent = heroBuilding.GetStonePercentage();
					resoStorBuild = getMaxStoneStoragePercetnage();
					if (resoStorBuild && resoStorBuild.GetStonePercentage() > percent)
					{
						resoStorBuild.addStone(-firstQuota);
					}
					else
					{
						heroBuilding.addStone(-firstQuota);
					}

					percent = 0;
					resoStorBuild = null;
				}


				if (gameManager.TotalCrystal >= firstQuota)
				{
					// Get the crystal storage with more percentage
					percent = heroBuilding.GetCrystalPercentage();
					resoStorBuild = getMaxCrystalStoragePercetnage();
					if (resoStorBuild && resoStorBuild.GetCrystalPercentage() > percent)
					{
						resoStorBuild.addCrystal(-firstQuota);
					}
					else
					{
						heroBuilding.addCrystal(-firstQuota);
					}

					percent = 0;
					resoStorBuild = null;
				}

			}
			
			gameManager.TotalGold -= firstQuota;
            //gameManager.TotalFood -= firstQuota;
            //gameManager.TotalStone -= firstQuota;
            //gameManager.TotalCrystal -= firstQuota;

            QuotaOne.SetActive(false);
            QuotaTwo.SetActive(true);
            QuotaOneComplete.SetActive(true);

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
}
