using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuilding : ProductionBuilding
{
    public int PersonalFoodCapacity;
    public int currentFoodStored;
    private List<GameObject> roadsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        //gm.foodCapacity += PersonalFoodCapacity;
        roadsToSpawn = new List<GameObject>();
        noRoadAccessIcon = transform.Find("NoRoadAccess").gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (roadsToSpawn.Count == 0)
        {
            noRoadAccessIcon.SetActive(true);
        }
        else
        {
            noRoadAccessIcon.SetActive(false);
        }

        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.TotalGold -= MaintenanceGoldCost;
            gm.PayFoodRent(MaintenanceFoodCost);
            gm.PayRentStone(MaintenanceStoneCost);
            gm.PayRentCrystal(MaintenanceCrystalCost);
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

    private void OnTriggerEnter(Collider other)
    {
        // Add the roads available to spawn 
        if (other.gameObject.tag == "Road")
        {
            roadsToSpawn.Add(other.gameObject);
        }
    }

    // Check if all the roads to spawn are not null. If null, delete it
    public void CheckAdyacentRoads()
    {
        for (int i = 0; i < roadsToSpawn.Count; i++)
        {
            if (roadsToSpawn[i] == null)
            {
                roadsToSpawn.RemoveAt(i);
                i--;
            }
        }
    }

    // Remove a road from the adyacent roads
    public void RemoveRoad(GameObject road)
    {
        for (int i = 0; i < roadsToSpawn.Count; i++)
        {
            if (roadsToSpawn[i] == road)
            {
                roadsToSpawn.RemoveAt(i);
                break;
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

    public int GetNumberRoads()
    {
        return roadsToSpawn.Count;
    }
}
