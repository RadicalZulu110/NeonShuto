using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
	[SerializeField]
	private StartingConstruction heroBuilding;
	private List<FoodStorageBuilding> foodStorageBuildings;
	private List<ResourceStorageBuilding> resourceStorageBuildings;

	private int NoBuildings;
	public int TotalGold;
	public int goldIncome;

	private int NoBatterys;
	public int TotalEnergy;
	public int energyIncome;

	private int NoFarms;
	public int TotalFood;
	public int foodIncome;
	[HideInInspector] public int foodCapacity;
	[HideInInspector] public int foodStored;
	private int NoFoodStorage;

	public int NoStoneMines;
	public int TotalStone;
	public int stoneIncome;
	[HideInInspector] public int stoneCapacity;
	[HideInInspector] public int stoneStored;

	public int NoCrystalMines;
	public int TotalCrystal;
	public int crystalIncome;
	[HideInInspector] public int crystalCapacity;
	[HideInInspector] public int crystalStored;
	private int NoResourceStorage;
	
	public int pop;
	public int futurePop;
	[HideInInspector] public int PopCapacity;
	[HideInInspector] public int TotalPop;

	

	public Text PlayerGoldDisplay;
	public Text GoldProduced;

	public Text PlayerEnergyDisplay;
	public Text EnergyProduced;

	public Text PlayerFoodDisplay;
	public Text FoodConsumed;
	public Text FoodStored;
	public Text FoodCapacity;

	public Text PlayerStoneDisplay;
	public Text StoneConsumed;
	public Text StoneStored;
	public Text StoneCapacity;

	public Text PlayerCrystalDisplay;
	public Text CrystalConsumed;
	public Text CrystalStored;
	public Text CrystalCapacity;


	public Text popDisplay;

	public CustomCursor customCursor;

	private List<GameObject> foodBuildings, stoneMiners, crystalMiners;

	private void Start()
	{
		NoBuildings = 0;
		NoFarms = 0;
		NoBatterys = 0;
		NoCrystalMines = 0;
		NoStoneMines = 0;
		foodBuildings = new List<GameObject>();
		stoneMiners = new List<GameObject>();
		crystalMiners = new List<GameObject>();
		NoFoodStorage = 0;
		NoResourceStorage = 0;
		foodBuildings = new List<GameObject>();
		foodStorageBuildings = new List<FoodStorageBuilding>();
		resourceStorageBuildings = new List<ResourceStorageBuilding>();
		TotalFood = 0;
		TotalStone = 0;
		TotalCrystal = 0;
	}

	private void Update()
	{
        if (heroBuilding)
		{ 
			// Food
			TotalFood = heroBuilding.GetComponent<StartingConstruction>().GetFoodStored();
			foreach (FoodStorageBuilding foodStorage in foodStorageBuildings)
            {
				TotalFood += foodStorage.GetFoodStored();
			}

			// Stone
			TotalStone = heroBuilding.GetStoneStored();
			foreach (ResourceStorageBuilding resourceStorage in resourceStorageBuildings)
				TotalStone += resourceStorage.GetStoneStored();

			// Crystal
			TotalCrystal = heroBuilding.GetCrystalStored();
			foreach (ResourceStorageBuilding resourceStorage in resourceStorageBuildings)
				TotalCrystal += resourceStorage.GetCrystalStored();
		}
		

		PlayerGoldDisplay.text = (TotalGold).ToString();
		GoldProduced.text = (goldIncome).ToString();

		PlayerEnergyDisplay.text = (TotalEnergy).ToString();
		EnergyProduced.text = (energyIncome).ToString();

		PlayerFoodDisplay.text = (TotalFood).ToString();
		FoodConsumed.text = (foodIncome).ToString();
		FoodStored.text = (foodStored).ToString();
		FoodCapacity.text = (foodCapacity).ToString();

		PlayerStoneDisplay.text = (TotalStone).ToString();
		StoneConsumed.text = (stoneIncome).ToString();
		StoneStored.text = (stoneStored).ToString();
		StoneCapacity.text = (stoneCapacity).ToString();

		PlayerCrystalDisplay.text = (TotalCrystal).ToString();
		CrystalConsumed.text = (crystalIncome).ToString();
		CrystalStored.text = (crystalStored).ToString();
		CrystalCapacity.text = (crystalCapacity).ToString();

		popDisplay.text = (TotalPop).ToString() + "/" + "[" + (PopCapacity).ToString() + "]";

	}

	//deduction of Reasources
	public void BuyBuilding(BuildingCost building)
	{
		goldIncome -= building.MaintenanceGoldCost;
		energyIncome -= building.MaintenanceEnergyCost;
		foodIncome -= building.MaintenanceFoodCost;
		stoneIncome -= building.MaintenanceStoneCost;
		crystalIncome -= building.MaintenanceCrystalCost;


		//initial gold cost deduction
		if (TotalGold >= building.GoldCost)
		{
			TotalGold -= building.GoldCost;
		}

		//initial energy cost deduction
		if (TotalEnergy >= building.EnergyCost)
		{
			TotalEnergy -= building.EnergyCost;
		}

		// FOOD CRYSTAL AND STONE
		float percent = 0;
		FoodStorageBuilding foodStorBuild = null;
		ResourceStorageBuilding resoStorBuild = null;

        if (heroBuilding)
        {
			if(TotalFood > building.FoodCost)
            {
				// Get the food storage with more percentage
				percent = heroBuilding.GetFoodPercentage();
				foodStorBuild = getMaxFoodStoragePercetnage();
				if (foodStorBuild && foodStorBuild.GetFoodPercentage() > percent)
				{
					foodStorBuild.addFood(-building.FoodCost);
				}
				else
				{
					heroBuilding.addFood(-building.FoodCost);
				}

				percent = 0;
				foodStorBuild = null;
			}
			

			if(TotalStone > building.StoneCost)
            {
				// Get the stone storage with more percentage
				percent = heroBuilding.GetStonePercentage();
				resoStorBuild = getMaxStoneStoragePercetnage();
				if (resoStorBuild && resoStorBuild.GetStonePercentage() > percent)
				{
					resoStorBuild.addStone(-building.StoneCost);
				}
				else
				{
					heroBuilding.addStone(-building.StoneCost);
				}

				percent = 0;
				resoStorBuild = null;
			}
			

			if(TotalCrystal > building.CrystalCost)
            {
				// Get the crystal storage with more percentage
				percent = heroBuilding.GetCrystalPercentage();
				resoStorBuild = getMaxCrystalStoragePercetnage();
				if (resoStorBuild && resoStorBuild.GetCrystalPercentage() > percent)
				{
					resoStorBuild.addCrystal(-building.CrystalCost);
				}
				else
				{
					heroBuilding.addCrystal(-building.CrystalCost);
				}

				percent = 0;
				resoStorBuild = null;
			}
			
		}


		if (pop >= building.PopCost)
		{
			pop -= building.PopCost;
		}
	}

	public int GetNoBuildings()
	{
		return NoBuildings;
	}

	public void SetNoBuilding(int BuildingNo)
	{
		NoBuildings = BuildingNo;
	}

	public int GetNoFarms()
	{
		return NoFarms;
	}

	public void SetNoFarms(int FarmNo)
    {
		NoFarms = FarmNo;
    }

	public int GetNoBatterys()
    {
		return NoBatterys;
    }

	public void SetNoBatterys(int BatteryNo)
    {
		NoBatterys = BatteryNo;
    }

	public int GetNoFoodStorage()
    {
		return NoFoodStorage;
    }

	public void SetNoFoodStorage(int FoodStorageNo)
    {
		NoFoodStorage = FoodStorageNo;
    }

	public int GetNoResourceStorage()
    {
		return NoResourceStorage;
    }

	public void SetNoResourceStorage(int ResourceStorageNo)
    {
		NoResourceStorage = ResourceStorageNo;
    }

	public int GetNoStoneMines()
    {
		return NoStoneMines;
    }

	public void SetNoStoneMines(int StoneMineNo)
    {
		NoStoneMines = StoneMineNo;
    }

	public int GetNoCrystalMines()
    {
		return NoCrystalMines;
    }

	public void SetNoCrystalMines(int CrystalMineNo)
    {
		NoCrystalMines = CrystalMineNo;
    }

	public int GetPop()
    {
		return futurePop;
    }

	public void SetFuturePop(int futurePopulation)
    {
		futurePop = futurePopulation;
    }

	public int GetTotalGold()
    {
		return TotalGold;
    }

	public int GetTotalEnergy()
    {
		return TotalEnergy;
    }
	
	public int GetTotalFood()
    {
		return TotalFood;
    }

	public int GetTotalStone()
    {
		return TotalStone;
    }

	public int GetTotalCrystal()
    {
		return TotalCrystal;
    }
	
	public void AddGold(int gold)
    {
		goldIncome += gold;
    }

	public void AddFood(int food)
    {
		foodIncome += food;
    }

	public void AddEnergy(int Energy)
    {
		energyIncome += Energy;
    }

	public void AddCrystal(int Crystal)
    {
		crystalIncome += Crystal;
    }

	public void AddStone(int Stone)
    {
		stoneIncome += Stone;
    }

	public void AddPop(int pop)
    {
		PopCapacity += pop;
    }

	public void AddTotalPop(int ExpectedPop)
    {
		TotalPop += ExpectedPop;

		if (TotalPop > PopCapacity)
        {
			TotalPop = PopCapacity;
		}
    }

	public void AddFoodPersonalCapacity(int FoodIncrease)
	{
		foodStored += FoodIncrease;

		if(foodStored > foodCapacity)
		{
			foodStored = foodCapacity;
		}
	}

	public void AddStonePersonalCapacity(int StoneIncrease)
    {
		stoneStored += StoneIncrease;

		if(stoneStored > stoneCapacity)
        {
			stoneStored = stoneCapacity;
        }
    }

	public void AddCrystalPersonalCapacity(int CrystalIncrease)
    {
		crystalStored += CrystalIncrease;

		if(crystalStored > crystalCapacity)
        {
			crystalStored = crystalCapacity;
        }
    }

	// Add a new farm to the list
	public void addFarm(GameObject farm)
    {
		foodBuildings.Add(farm);
    }

	// Delete a farm from the list
	public void deleteFarm(GameObject farm)
    {
		foodBuildings.Remove(farm);
    }

	public List<GameObject> getFarms()
    {
		return foodBuildings;
    }

	public void addTotalFood(int food)
    {
		TotalFood += food;
    }

	// Add a new stone miner to the list
	public void addStoneMiner(GameObject stoneMiner)
	{
		stoneMiners.Add(stoneMiner);
	}

	// Delete a stone miner from the list
	public void deleteStoneMiner(GameObject stoneMiner)
	{
		stoneMiners.Remove(stoneMiner);
	}

	public List<GameObject> getStoneMiners()
	{
		return stoneMiners;
	}

	public void addTotalStone(int stone)
	{
		TotalStone += stone;
	}

	// Add a new crystal miner to the list
	public void addCrystalMiner(GameObject crystalMiner)
	{
		crystalMiners.Add(crystalMiner);
	}

	// Delete a farm from the list
	public void deleteCrystalMiner(GameObject crystalMiner)
	{
		crystalMiners.Remove(crystalMiner);
	}

	public List<GameObject> getCrystalMiners()
	{
		return crystalMiners;
	}

	public void addTotalCrystal(int crystal)
	{
		TotalCrystal += crystal;
	}

	public void AddHeroBuilding(StartingConstruction hB)
    {
		heroBuilding = hB;
    }

	public void AddFoodStorageBuilding(FoodStorageBuilding fSB)
    {
		foodStorageBuildings.Add(fSB);
    }

	public void DeleteFoodStorageBuilding(FoodStorageBuilding fsb)
    {
		foodStorageBuildings.Remove(fsb);
    }

	public void AddResourceStorageBuilding(ResourceStorageBuilding rSB)
	{
		resourceStorageBuildings.Add(rSB);
	}

	public void DeleteResourceStorageBuilding(ResourceStorageBuilding rsb)
	{
		resourceStorageBuildings.Remove(rsb);
	}

	// Get the food storage with more percentage
	private FoodStorageBuilding getMaxFoodStoragePercetnage()
    {
		if (foodStorageBuildings.Count == 0)
			return null;

		float max = 0;
		FoodStorageBuilding maxObject = null;

		for(int i=0; i<foodStorageBuildings.Count; i++)
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