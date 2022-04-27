using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
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

	private List<GameObject> foodBuildings;

	private void Start()
	{
		NoBuildings = 0;
		NoFarms = 0;
		NoBatterys = 0;
		NoCrystalMines = 0;
		NoStoneMines = 0;
		NoFoodStorage = 0;
		NoResourceStorage = 0;
	}

	private void Update()
	{
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

		if (TotalFood >= building.FoodCost)
		{
			TotalFood -= building.FoodCost;
		}

		if (TotalStone >= building.StoneCost)
		{
			TotalStone -= building.StoneCost;
		}

		if (TotalCrystal >= building.CrystalCost)
		{
			TotalCrystal -= building.CrystalCost;
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
}