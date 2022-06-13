using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationBuilding : BuildingCost
{

    

    // Start is called before the first frame update
    void Start()
    {
        gm.AddPop(PopIncrease);
        
    }

    // Update is called once per frame
    void Update()
    {  
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.TotalEnergy -= MaintenanceEnergyCost;
            gm.PayFoodRent(MaintenanceFoodCost);
            gm.PayRentStone(MaintenanceStoneCost);
            gm.PayRentCrystal(MaintenanceCrystalCost);

            if(getTier() == 3)
            {
                gm.AddTreeLife(-T3TreeLife);
            }
        }
    }

    
    public int GetPopulation()
    {
        return PopIncrease;
    }

    
    public int GetGoldIncrease()
    {
        return GoldIncreasePerPerson;
    }

    public int GetMaintenanceEnergyCost()
    {
        return MaintenanceEnergyCost;
    }

    public int GetMaintenanceFoodCost()
    {
        return MaintenanceFoodCost;
    }
}
