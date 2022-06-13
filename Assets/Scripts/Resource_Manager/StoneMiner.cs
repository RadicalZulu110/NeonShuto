using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMiner : MinerBuilding
{
    public int PersonalStoneCapacity;
    public int currentStoneStored;
    private List<GameObject> roadsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        //gm.stoneCapacity += PersonalStoneCapacity;
        roadsToSpawn = new List<GameObject>();
        noRoadAccessIcon = transform.Find("NoRoadAccess").gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Debug.Log(roadsToSpawn.Count);

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
            currentStoneStored += StoneIncrease;

            gm.TotalGold -= MaintenanceGoldCost;
            gm.TotalEnergy -= MaintenanceEnergyCost;
            gm.PayFoodRent(MaintenanceFoodCost);
            gm.PayRentStone(MaintenanceStoneCost);
            gm.PayRentCrystal(MaintenanceCrystalCost);

            gm.AddStonePersonalCapacity(StoneIncrease);
            currentStoneStored += StoneIncrease;

            if (currentStoneStored > PersonalStoneCapacity)
            {
                currentStoneStored = PersonalStoneCapacity;
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
    public override int GetStoneIncrease()
    {
        return StoneIncrease;
    }

    public override int GetCurrentStoneStored()
    {
        return currentStoneStored;
    }

    public override int GetPersonalStoneCapacity()
    {
        return PersonalStoneCapacity;
    }

    public override void addStone(int s)
    {
        currentStoneStored += s;
    }

    public override int GetMaintenanceEnergyCost()
    {
        return MaintenanceEnergyCost;
    }

    public override int GetMaintenanceFoodCost()
    {
        return MaintenanceFoodCost;
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
                i--;
            }
        }
    }

    public int GetNumberRoads()
    {
        return roadsToSpawn.Count;
    }
}
