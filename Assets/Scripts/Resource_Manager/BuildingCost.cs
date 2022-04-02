using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCost : MonoBehaviour
{
    public int width, height;

    //set cost of the building type
    public int GoldCost;
    public int FoodCost;
    public int EnergyCost;
    public int StoneCost;
    public int CrystalCost;
    public int PopCost;

    //set time between increases in reasources
    public float timeBtwIncrease;
    public float nextIncreaseTime;

    public GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    //Doing somthing wrong here 
    private void Update()
    {
        
    }
    

    public int getGridWidth()
    {
        return width;
    }

    public int getGridHeight()
    {
        return height;
    }

    public void setWH(int w, int h)
    {
        width = w;
        height = h;
    }

    // Change the height with the width and viceversa
    public void changeWH()
    {
        int haux = height;
        height = width;
        width = haux;
    }

    // Functions in the child
    virtual public int GetPopulation()
    {
        return 0;
    }

    virtual public int GetGoldIncrease()
    {
        return 0;
    }

    virtual public int GetFoodIncrease()
    {
        return 0;
    }

    virtual public int GetEnergyIncrease()
    {
        return 0;
    }

    virtual public int GetCrystalIncrease()
    {
        return 0;
    }

    virtual public int GetStoneIncrease()
    {
        return 0;
    }
}
