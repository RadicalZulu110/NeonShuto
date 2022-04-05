using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationBuilding : BuildingCost
{
    
    public int GoldIncrease;
    public int PopIncrease;

    // Start is called before the first frame update
    void Start()
    {
        gm.AddPop(PopIncrease);
        gm.AddGold(GoldIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.gold += GoldIncrease;
        }
    }

    
    public int GetPopulation()
    {
        return PopIncrease;
    }

    
    public int GetGoldIncrease()
    {
        return GoldIncrease;
    }
}