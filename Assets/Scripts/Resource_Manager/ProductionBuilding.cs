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

    public virtual int GetPersonalFoodCapacity()
    {
        return 0;
    }
    public virtual int GetCurrentFoodStored()
    {
        return 0;
    }
    public virtual int GetPersonalStoneCapacity()
    {
        return 0;
    }
    public virtual int GetCurrentStoneStored()
    {
        return 0;
    }

    public virtual int GetPersonalCrystalCapacity()
    {
        return 0;
    }
    public virtual int GetCurrentCrystalStored()
    {
        return 0;
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
