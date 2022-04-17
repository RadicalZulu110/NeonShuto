using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBuilding : ProductionBuilding
{
    public int PersonalStoneCapacity;
    public int PersonalCrystalCapacity;
    [HideInInspector] public int currentStoneStored;
    [HideInInspector] public int currentCrystalStored;

    // Start is called before the first frame update
    void Start()
    {
        gm.stoneCapacity += PersonalStoneCapacity;
        gm.crystalCapacity += PersonalCrystalCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            currentStoneStored += StoneIncrease;
            currentCrystalStored += CrystalIncrease;

            gm.TotalGold -= MaintenanceGoldCost;
            gm.TotalEnergy -= MaintenanceEnergyCost;

            gm.AddCrystalPersonalCapacity(CrystalIncrease);
            gm.AddStonePersonalCapacity(StoneIncrease);

            if(currentCrystalStored > PersonalCrystalCapacity)
            {
                currentCrystalStored = PersonalCrystalCapacity;
            }
            if(currentStoneStored > PersonalStoneCapacity)
            {
                currentStoneStored = PersonalStoneCapacity;
            }
        }
    }

    public override int GetCrystalIncrease()
    {
        return CrystalIncrease;
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

    public override int GetCurrentCrystalStored()
    {
        return currentCrystalStored;
    }
}
