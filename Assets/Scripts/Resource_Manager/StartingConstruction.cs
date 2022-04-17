using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StartingConstruction : BuildingCost
{
    public int MaxPop;
    public int MinPop;
    public int ExpectedPop;
    private int nextPopIncreaseTime;
    public int timeBtwPopIncrease;

    void Update()
    {
        if (Time.time > nextPopIncreaseTime)
        {
            nextPopIncreaseTime = (int)(Time.time + timeBtwPopIncrease);
            ExpectedPop = Random.Range(MinPop, MaxPop);

            gm.AddTotalPop(ExpectedPop);
            gm.goldIncome = gm.TotalPop * GoldIncreasePerPerson;
            gm.TotalGold += gm.goldIncome;
        }
    }

    public int GetExpectedPop()
    {
        return ExpectedPop;
    }
}
