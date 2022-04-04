using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCost : MonoBehaviour
{
    public int width, height;

    //set initial cost of the building type
    public int GoldCost;
    public int FoodCost;
    public int EnergyCost;
    public int StoneCost;
    public int CrystalCost;
    public int PopCost;

    //set maintenance cost of the building type
    public int MaintenanceGoldCost;
    public int MaintenanceFoodCost;
    public int MaintenanceEnergyCost;
    public int MaintenanceStoneCost;
    public int MaintenanceCrystalCost;
    public int MaintenancePopCost;

    //set amount of reasource to increase
    public int GoldIncrease;
    public int FoodIncrease;
    public int EnergyIncrease;
    public int StoneIncrease;
    public int CrystalIncrease;
    public int PopIncrease;

    //set time between increases in reasources
    public float timeBtwIncrease;
    private float nextIncreaseTime;

    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gm.AddPop(PopIncrease);
        gm.AddGold(GoldIncrease);
        gm.AddFood(FoodIncrease);
        gm.AddEnergy(EnergyIncrease);
        gm.AddStone(StoneIncrease);
        gm.AddCrystal(CrystalIncrease);
    }

    private void Update()
    {
        if (Time.time > nextIncreaseTime)
        {

            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.gold += GoldIncrease;
            gm.energy += EnergyIncrease;
            gm.food += FoodIncrease;
            gm.stone += StoneIncrease;
            gm.crystal += CrystalIncrease;
        }
    }

    public int GetPopulation()
    {
        return PopIncrease;
    }

    public int GetGoldIncrease()
    {
        return GoldIncrease;
    }

    public int GetFoodIncrease()
    {
        return FoodIncrease;
    }

    public int GetEnergyIncrease()
    {
        return EnergyIncrease;
    }

    public int GetCrystalIncrease()
    {
        return CrystalIncrease;
    }

    public int GetStoneIncrease()
    {
        return StoneIncrease;
    }

    public int getGridWidth()
    {
        return width;
    }

    public int getGridHeight()
    {
        return height;
    }

    public void setWH(int w, int h)
    {
        width = w;
        height = h;
    }

    // Change the height with the width and viceversa
    public void changeWH()
    {
        int haux = height;
        height = width;
        width = haux;
    }
}
