using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCost : MonoBehaviour
{
    public int width, height;

    //set initial cost of the building type
    public int GoldCost;
    public int FoodCost;
    public int EnergyCost;
    public int StoneCost;
    public int CrystalCost;
    public int PopCost;

    public int MaintenanceGoldCost;
    public int MaintenanceFoodCost;
    public int MaintenanceEnergyCost;
    public int MaintenanceStoneCost;
    public int MaintenanceCrystalCost;
    public int MaintenancePopCost;
    //set time between increases in reasources
    public float timeBtwIncrease;
    public float nextIncreaseTime;

    public GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

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

    
}
