using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMiner : MinerBuilding
{

    public int PersonalCrystalCapacity;
    public int currentCrystalStored;

    // Start is called before the first frame update
    void Start()
    {
        gm.crystalCapacity += PersonalCrystalCapacity;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            currentCrystalStored += CrystalIncrease;

            gm.TotalGold -= MaintenanceGoldCost;
            gm.TotalEnergy -= MaintenanceEnergyCost;

            gm.AddCrystalPersonalCapacity(CrystalIncrease);
            currentCrystalStored += CrystalIncrease;

            if (currentCrystalStored > PersonalCrystalCapacity)
            {
                currentCrystalStored = PersonalCrystalCapacity;
            }
        }
    }

    public override int GetCrystalIncrease()
    {
        return CrystalIncrease;
    }

    public override int GetCurrentCrystalStored()
    {
        return currentCrystalStored;
    }

    public override void addCrystal(int c)
    {
        currentCrystalStored += c;
    }
}
