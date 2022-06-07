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

    //set time between increases in reasources
    public float timeBtwIncrease;
    public float nextIncreaseTime;

    public int StoneIncrease;
    public int CrystalIncrease;
    public int EnergyIncrease;
    public int FoodIncrease;
    public int GoldIncreasePerPerson;
    public int PopIncrease;

    public float speedAnimation, amountAnimation;

    public int tier;

    public GameManager gm;

    public float T3TreeLife;

    private List<GameObject> nodes;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        nodes = new List<GameObject>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * speedAnimation) * amountAnimation, transform.position.y + Mathf.Sin(Time.time * speedAnimation) * amountAnimation, transform.position.z + Mathf.Sin(Time.time * speedAnimation) * amountAnimation);
    }
    
    public void RotateBuilding()
    {
        int aux = width;
        width = height;
        height = aux;
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

    public virtual int GetFoodIncrease()
    {
        return FoodIncrease;
    }

    // Get the nodes that the building is occuping
    public List<GameObject> getNodes()
    {
        return nodes;
    }

    // Set the nodes that the building is occuping
    public void setNodes(List<GameObject> n)
    {
        nodes = n;
    }

    public int getTier()
    {
        return tier;
    }

}
