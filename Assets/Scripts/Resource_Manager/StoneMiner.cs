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

        ResourceNode[] resources = GameObject.FindObjectsOfType<ResourceNode>();
        float dist = float.MaxValue;

        for(int i = 0; i < resources.Length; i++)
        {
            if(resources[i].StoneNodeAmount > 0)
            {
                if(Vector3.Distance(this.gameObject.transform.position, resources[i].gameObject.transform.position) < dist)
                {
                    dist = Vector3.Distance(this.gameObject.transform.position, resources[i].gameObject.transform.position);
                    resourceNode = resources[i];
                }
            }
        }
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
            
            if(resourceNode != null)
            {
                gm.TotalGold -= MaintenanceGoldCost;
                gm.TotalEnergy -= MaintenanceEnergyCost;
                gm.PayFoodRent(MaintenanceFoodCost);
                gm.PayRentStone(MaintenanceStoneCost);
                gm.PayRentCrystal(MaintenanceCrystalCost);

                if (currentStoneStored + StoneIncrease > PersonalStoneCapacity)
                {
                    resourceNode.StoneNodeAmount -= PersonalStoneCapacity - currentStoneStored;
                    gm.AddStonePersonalCapacity(PersonalStoneCapacity - StoneIncrease);
                    currentStoneStored = PersonalStoneCapacity;
                }
                else
                {
                    resourceNode.StoneNodeAmount -= StoneIncrease;
                    currentStoneStored += StoneIncrease;
                    gm.AddStonePersonalCapacity(StoneIncrease);
                }
            }
            else
            {
                gm.TotalGold -= MaintenanceGoldCost/2;
                gm.TotalEnergy -= MaintenanceEnergyCost/2;
                gm.PayFoodRent(MaintenanceFoodCost/2);
                gm.PayRentStone(MaintenanceStoneCost/2);
                gm.PayRentCrystal(MaintenanceCrystalCost/2);
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
