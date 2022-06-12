using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] Stages;
    private int popUpIndex;

    public Button HeroButton;
    public Button HousingTab;
    public Button T1_HouseButton;
    public Button ResTab;
    public Button FarmButton;
    public Button PowerButton;
    public Button MinerTab;
    public Button CrystalMinerButton;
    public Button StoneMinerButton;
    public Button StorageTab;
    public Button ResStorageButton;
    public Button FoodStorageButton;
    public Button RoadButton;
    public Button DeleteButton;
    public Button WaterButton;


    public GameObject Close;

    public Color ActiveColor;
    public Color CompleteColor;


    private void Start()
    {
        popUpIndex = 0;
    }

    private void Update()
    {
        for (int i = 0; i < Stages.Length; i++)
        {

            if (i == popUpIndex)
            {
                Stages[popUpIndex].gameObject.SetActive(true);
            }
            else
            {
                Stages[i].SetActive(false);
            }
        }
        //cba = ColorBlockActive
        //cbc = ColorBlockComplete
        if(popUpIndex == 0)
        {
            ColorBlock cba = HeroButton.colors;
            cba.normalColor = ActiveColor;
            HeroButton.colors = cba;

            if (GameObject.FindGameObjectWithTag("HeroBuilding") != null)
            {
                ColorBlock cbc = HeroButton.colors;
                cbc.normalColor = CompleteColor;
                HeroButton.colors = cbc;
                popUpIndex++;
            }
        }
        else if(popUpIndex == 1)
        {
            ColorBlock cba = HousingTab.colors;
            cba.normalColor = ActiveColor;
            HousingTab.colors = cba;

            ColorBlock cba1 = T1_HouseButton.colors;
            cba1.normalColor = ActiveColor;
            T1_HouseButton.colors = cba1;

            if (GameObject.FindGameObjectWithTag("PopulationBuilding") != null)
            {
                ColorBlock cbc = HousingTab.colors;
                cbc.normalColor = CompleteColor;
                HousingTab.colors = cbc;

                ColorBlock cbc1 = T1_HouseButton.colors;
                cbc1.normalColor = CompleteColor;
                T1_HouseButton.colors = cbc1;
                popUpIndex++;
            }
        }
        else if(popUpIndex == 2)
        {
            ColorBlock cba = RoadButton.colors;
            cba.normalColor = ActiveColor;
            RoadButton.colors = cba;

            if (GameObject.FindGameObjectWithTag("Road") != null)
            {
                ColorBlock cbc = RoadButton.colors;
                cbc.normalColor = CompleteColor;
                RoadButton.colors = cbc;
                popUpIndex++;
            }
        }
        else if(popUpIndex == 3)
        {
            ColorBlock cba = ResTab.colors;
            cba.normalColor = ActiveColor;
            ResTab.colors = cba;

            ColorBlock cba1 = FarmButton.colors;
            cba1.normalColor = ActiveColor;
            FarmButton.colors = cba1;

            if (GameObject.Find("SM_FoodT1") != null)
            {
                ColorBlock cbc = ResTab.colors;
                cbc.normalColor = CompleteColor;
                ResTab.colors = cbc;

                ColorBlock cbc1 = FarmButton.colors;
                cbc1.normalColor = CompleteColor;
                FarmButton.colors = cbc1;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 4)
        {
            ColorBlock cba = ResTab.colors;
            cba.normalColor = ActiveColor;
            ResTab.colors = cba;

            ColorBlock cba1 = PowerButton.colors;
            cba1.normalColor = ActiveColor;
            PowerButton.colors = cba1;

            Close.SetActive(true);

            if (GameObject.Find("SM_PowerT1_New") != null)
            {
                ColorBlock cbc = ResTab.colors;
                cbc.normalColor = CompleteColor;
                ResTab.colors = cbc;

                ColorBlock cbc1 = PowerButton.colors;
                cbc1.normalColor = CompleteColor;
                PowerButton.colors = cbc1;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 5)
        {
            ColorBlock cba = StorageTab.colors;
            cba.normalColor = ActiveColor;
            StorageTab.colors = cba;

            ColorBlock cba1 = WaterButton.colors;
            cba1.normalColor = ActiveColor;
            WaterButton.colors = cba1;

            Close.SetActive(true);

            if (GameObject.Find("SM_Water_T1") != null)
            {
                ColorBlock cbc = StorageTab.colors;
                cbc.normalColor = CompleteColor;
                StorageTab.colors = cbc;

                ColorBlock cbc1 = WaterButton.colors;
                cbc1.normalColor = CompleteColor;
                WaterButton.colors = cbc1;
                popUpIndex++;
            }
        }
    }
}