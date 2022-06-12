using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBuilding : ProductionBuilding 
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
        if (Time.time > nextIncreaseTime)
        {
            if(getTier() == 3)
            {
                nextIncreaseTime = Time.time + timeBtwIncrease;
                gm.AddTreeLife(T3TreeLife);
            }
            gm.PayFoodRent(MaintenanceFoodCost);
            gm.PayRentStone(MaintenanceStoneCost);
            gm.PayRentCrystal(MaintenanceCrystalCost);
        }

        
    }
}
