using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Truck : MonoBehaviour
{

    public NavMeshAgent agent;
    public int capacity, maxCapacity;
    private GameObject heroBuilding;
    private bool comingBack, food;
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
        if(other is SphereCollider)
        {
            if (other.gameObject.GetComponent<ProductionBuilding>())
            {
                ProductionBuilding otherScript = other.gameObject.GetComponent<ProductionBuilding>();
                
                if (otherScript.getTruckRecollecting() == this.gameObject)
                {
                    if (other.gameObject.GetComponent<FoodBuilding>())
                    {
                        otherScript = other.gameObject.GetComponent<FoodBuilding>();
                        food = true;
                        capacity = otherScript.GetCurrentFoodStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addFood(-capacity);
                    }

                    agent.SetDestination(heroBuilding.transform.position);
                    otherScript.setTruckRecollecting(null);
                    otherScript.setRecollecting(false);
                    comingBack = true;
                }
            }
            else
            {
                if(other.gameObject.tag == "HeroBuilding" && comingBack)
                {
                    if (food)
                    {
                        food = false;
                        gameManager.addTotalFood(capacity);
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
        Debug.Log(goal.x);
    }
}
