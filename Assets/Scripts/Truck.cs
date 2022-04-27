using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Truck : MonoBehaviour
{

    public NavMeshAgent agent;
    public int capacity, maxCapacity;
    private GameObject heroBuilding;
    private bool comingBack, food, stone, crystal;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        heroBuilding = GameObject.FindGameObjectWithTag("HeroBuilding");
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        comingBack = false;
        capacity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If it colides with a sphere collider
        if(other is SphereCollider)
        {
            // If it is a production building
            if (other.gameObject.GetComponent<ProductionBuilding>())
            {
                ProductionBuilding otherScript = other.gameObject.GetComponent<ProductionBuilding>();
                
                if (otherScript.getTruckRecollecting() == this.gameObject)
                {
                    // If it is a farm
                    if (other.gameObject.GetComponent<FoodBuilding>())
                    {
                        otherScript = other.gameObject.GetComponent<FoodBuilding>();
                        food = true;
                        capacity = otherScript.GetCurrentFoodStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addFood(-capacity);
                    }
                    else if (other.gameObject.GetComponent<StoneMiner>())  // If it is a stone miner
                    {
                        otherScript = other.gameObject.GetComponent<StoneMiner>();
                        stone = true;
                        capacity = otherScript.GetCurrentStoneStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addStone(-capacity);
                    }
                    else if (other.gameObject.GetComponent<CrystalMiner>())  // If it is a crystal miner
                    {
                        otherScript = other.gameObject.GetComponent<CrystalMiner>();
                        crystal = true;
                        capacity = otherScript.GetCurrentCrystalStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addCrystal(-capacity);
                    }


                    agent.SetDestination(heroBuilding.transform.position);
                    otherScript.setTruckRecollecting(null);
                    otherScript.setRecollecting(false);
                    comingBack = true;
                }
            }
            else
            {
                // If it is the hero building
                if(other.gameObject.tag == "HeroBuilding" && comingBack)
                {
                    if (food)
                    {
                        food = false;
                        gameManager.addTotalFood(capacity);
                    }else if (stone)
                    {
                        stone = false;
                        gameManager.addTotalStone(capacity);
                    }else if (crystal)
                    {
                        crystal = false;
                        gameManager.addTotalCrystal(capacity);
                    }

                    this.gameObject.SetActive(false);
                    comingBack = false;
                    heroBuilding.GetComponent<StartingConstruction>().makeAvailableTruck(this.gameObject);
                    capacity = 0;
                }
            }
        }
    }

    public void setDestination(Vector3 goal)
    {
        agent.SetDestination(goal);
    }
}
