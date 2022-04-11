using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBuilding : ProductionBuilding
{

    

    // Start is called before the first frame update
    void Start()
    {
        gm.AddStone(StoneIncrease);
        gm.AddCrystal(CrystalIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.TotalStone += StoneIncrease;
            gm.TotalCrystal += CrystalIncrease;
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
}
