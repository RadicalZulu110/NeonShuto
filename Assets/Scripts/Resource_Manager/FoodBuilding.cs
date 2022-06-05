using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuilding : ProductionBuilding
{
    public int PersonalFoodCapacity;
    public int currentFoodStored;

    // Start is called before the first frame update
    void Start()
    {
        //gm.foodCapacity += PersonalFoodCapacity;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.TotalGold -= MaintenanceGoldCost;
            gm.AddFoodPersonalCapacity(FoodIncrease);
            currentFoodStored += FoodIncrease;

            if(currentFoodStored > PersonalFoodCapacity)
            {
                currentFoodStored = PersonalFoodCapacity;
            }

            if (getTier() == 3)
            {
                gm.AddTreeLife(-T3TreeLife);
            }
        }
    }

    public override int GetFoodIncrease()
    {
        return FoodIncrease;
    }

    public override int GetCurrentFoodStored()
    {
        return currentFoodStored;
    }

    public override int GetPersonalFoodCapacity()
    {
        return PersonalFoodCapacity;
    }

    public override void addFood(int food)
    {
        currentFoodStored += food;
    }
}
