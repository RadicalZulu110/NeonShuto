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
	
	public int pop;
	public int futurePop;
	[HideInInspector] public int PopCapacity;
	[HideInInspector] public int TotalPop;


	public Text goldDisplay;
	public Text energyDisplay;
	public Text foodDisplay;
	public Text foodStorage;
	public Text StoneDisplay;
	public Text stoneStorage;
	public Text CrystalDisplay;
	public Text crystalStorage;
	public Text popDisplay;

	public CustomCursor customCursor;

	private void Start()
	{
		NoBuildings = 0;
		NoFarms = 0;
		NoBatterys = 0;
		NoCrystalMines = 0;
		NoStoneMines = 0;
	}

	private void Update()
	{
		goldDisplay.text = (TotalGold).ToString() + "(" + (goldIncome).ToString() + ")";
		energyDisplay.text = (TotalEnergy).ToString() + "(" + (energyIncome).ToString() + ")";
		foodDisplay.text = (TotalFood).ToString() + "(" + (foodIncome).ToString() + ")";
		StoneDisplay.text = (TotalStone).ToString() + "(" + (stoneIncome).ToString() + ")";
		CrystalDisplay.text = (TotalCrystal).ToString() + "(" + (crystalIncome).ToString() + ")";
		popDisplay.text = (TotalPop).ToString() + "/" + "[" + (PopCapacity).ToString() + "]";

		foodStorage.text = (foodStored).ToString() + "[" + (foodCapacity).ToString() + "]";
		stoneStorage.text = (stoneStored).ToString() + "[" + (stoneCapacity).ToString() + "]";
		crystalStorage.text = (crystalStored).ToString() + "[" + (crystalCapacity).ToString() + "]";
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

	//deduction of reasources new work in progress kind apointless might be useful 

	/*
	public void BuyBuilding(BuildingCost building)
	{
		if (gold >= building.GoldCost && currentGold >= building.MaintenanceGoldCost)
		{
			gold -= building.GoldCost;
			currentGold -= building.MaintenanceGoldCost;
		}

		if (energy >= building.EnergyCost && currentEnergy >= building.MaintenanceEnergyCost)
		{
			energy -= building.EnergyCost;
			currentEnergy -= building.MaintenanceEnergyCost;
		}

		if (food >= building.FoodCost && currentFood >= building.MaintenanceFoodCost)
		{
			food -= building.FoodCost;
			currentFood -= building.MaintenanceFoodCost;
		}

        if (stone >= building.StoneCost && currentStone >= building.MaintenanceStoneCost)
        {
			stone -= building.StoneCost;
			currentStone -= building.MaintenanceStoneCost;
        }

        if (crystal >= building.CrystalCost && currentCrystal >= building.MaintenanceCrystalCost)
        {
			crystal -= building.CrystalCost;
			currentCrystal -= building.MaintenanceCrystalCost;
        }

		if (pop >= building.PopCost)
		{
			pop -= building.PopCost;
		}
	}
	*/

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
}