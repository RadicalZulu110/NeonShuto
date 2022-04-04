using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BuildingCost
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    
    public virtual int GetFoodIncrease()
    {
        return 0;
    }

    
    public virtual int GetEnergyIncrease()
    {
        return 0;
    }

    
    public virtual int GetCrystalIncrease()
    {
        return 0;
    }

    
    public virtual int GetStoneIncrease()
    {
        return 0;
    }
}
