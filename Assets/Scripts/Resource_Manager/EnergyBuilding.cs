using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBuilding : ProductionBuilding
{

    

    // Start is called before the first frame update
    void Start()
    {
        gm.AddEnergy(EnergyIncrease);
    }

    // Update is called once per frame
    
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.TotalEnergy += EnergyIncrease;
            gm.TotalGold -= MaintenanceGoldCost;
            gm.PayFoodRent(MaintenanceFoodCost);
            gm.PayRentStone(MaintenanceStoneCost);
            gm.PayRentCrystal(MaintenanceCrystalCost);

            if (getTier() == 3)
            {
                gm.AddTreeLife(-T3TreeLife);
            }
        }

        
    }
    

    public override int GetEnergyIncrease()
    {
        return EnergyIncrease;
    }
}
