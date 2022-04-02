using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BuildingCost
{
    //set amount of reasource to increase
    public int FoodIncrease;
    public int EnergyIncrease;
    public int StoneIncrease;
    public int CrystalIncrease;

    // Start is called before the first frame update
    void Start()
    {
        gm.AddFood(FoodIncrease);
        gm.AddEnergy(EnergyIncrease);
        gm.AddStone(StoneIncrease);
        gm.AddCrystal(CrystalIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.energy += EnergyIncrease;
            gm.food += FoodIncrease;
            gm.stone += StoneIncrease;
            gm.crystal += CrystalIncrease;
        }
    }

    override
    public int GetFoodIncrease()
    {
        return FoodIncrease;
    }

    override
    public int GetEnergyIncrease()
    {
        return EnergyIncrease;
    }

    override
    public int GetCrystalIncrease()
    {
        return CrystalIncrease;
    }

    override
    public int GetStoneIncrease()
    {
        return StoneIncrease;
    }
}
