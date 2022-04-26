using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BuildingCost
{

    public bool recollecting;
    public GameObject truckRecollecting;

    // Start is called before the first frame update
    void Start()
    {
        recollecting = false;
        truckRecollecting = null;
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

    public bool isRecollecting()
    {
        return recollecting;
    }

    public void setRecollecting(bool r)
    {
        recollecting = r;
    }

    public GameObject getTruckRecollecting()
    {
        return truckRecollecting;
    }

    public void setTruckRecollecting(GameObject truck)
    {
        truckRecollecting = truck;
    }

    public virtual void addFood(int food)
    {
    }
}
