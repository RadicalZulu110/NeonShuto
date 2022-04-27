using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMiner : MinerBuilding
{
    public int PersonalStoneCapacity;
    public int currentStoneStored;

    // Start is called before the first frame update
    void Start()
    {
        gm.stoneCapacity += PersonalStoneCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            currentStoneStored += StoneIncrease;

            gm.TotalGold -= MaintenanceGoldCost;
            gm.TotalEnergy -= MaintenanceEnergyCost;

            gm.AddStonePersonalCapacity(StoneIncrease);
            currentStoneStored += StoneIncrease;

            if (currentStoneStored > PersonalStoneCapacity)
            {
                currentStoneStored = PersonalStoneCapacity;
            }
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
}
