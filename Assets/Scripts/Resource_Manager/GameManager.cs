using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
	private int NoBuildings;
	public int gold;
	public int currentGold;

	private int NoBatterys;
	public int energy;
	public int currentEnergy;

	private int NoFarms;
	public int food;
	public int currentFood;

	public int NoStoneMines;
	public int stone;
	public int currentStone;

	public int NoCrystalMines;
	public int crystal;
	public int currentCrystal;
	
	public int pop;
	public int futurePop;
	public int currentPop;


	public Text goldDisplay;
	public Text energyDisplay;
	public Text foodDisplay;
	public Text StoneDisplay;
	public Text CrystalDisplay;
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
		goldDisplay.text = (gold).ToString() + "(" + (currentGold).ToString() + ")";
		energyDisplay.text = (energy).ToString() + "(" + (currentEnergy).ToString() + ")";
		foodDisplay.text = (food).ToString() + "(" + (currentFood).ToString() + ")";
		StoneDisplay.text = (stone).ToString() + "(" + (currentStone).ToString() + ")";
		CrystalDisplay.text = (crystal).ToString() + "(" + (currentCrystal).ToString() + ")";
		popDisplay.text = (currentPop).ToString();
	}

	//deduction of reasources
	public void BuyBuilding(BuildingCost building)
	{
		if (gold >= building.GoldCost)
		{
			gold -= building.GoldCost;
		}

		if (energy >= building.EnergyCost)
		{
			energy -= building.EnergyCost;
		}

		if (food >= building.FoodCost)
		{
			food -= building.FoodCost;
		}

        if (stone >= building.StoneCost)
        {
			stone -= building.StoneCost;
        }

        if (crystal >= building.CrystalCost)
        {
			crystal -= building.CrystalCost;
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

	public int GetGold()
    {
		return gold;
    }

	public int GetEnergy()
    {
		return energy;
    }
	
	public int GetFood()
    {
		return food;
    }

	public int GetStone()
    {
		return stone;
    }

	public int GetCrystal()
    {
		return crystal;
    }
	
	public void AddGold(int gold)
    {
		currentGold += gold;
    }

	public void AddFood(int food)
    {
		currentFood += food;
    }

	public void AddEnergy(int Energy)
    {
		currentEnergy += Energy;
    }

	public void AddCrystal(int Crystal)
    {
		currentCrystal += Crystal;
    }

	public void AddStone(int Stone)
    {
		currentStone += Stone;
    }

	public void AddPop(int pop)
    {
		currentPop += pop;
    }
}
