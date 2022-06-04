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
    void Update()
    {
        if(getTier() == 3)
        {
            if (Time.time > nextIncreaseTime)
            {
                nextIncreaseTime = Time.time + timeBtwIncrease;
                gm.AddTreeLife(T3TreeLife);
            }
        }
    }
}
