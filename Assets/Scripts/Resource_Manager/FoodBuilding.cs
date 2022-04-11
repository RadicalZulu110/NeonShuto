using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuilding : ProductionBuilding
{

    

    // Start is called before the first frame update
    void Start()
    {
        gm.AddFood(FoodIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.TotalFood += FoodIncrease;
        }
    }

    public override int GetFoodIncrease()
    {
        return FoodIncrease;
    }
}
