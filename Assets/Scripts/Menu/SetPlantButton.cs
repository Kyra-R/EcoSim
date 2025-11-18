using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPlantButton : MonoBehaviour
{
    public PlantController plantController;

    public Button m_MyButton;

    public Button toMutations;
    public Button toParameters;

    public Slider seedDispersionSlider; //TODO: add fruit nutritiousness
    public Slider pollenDispersionSlider;
    public Slider rootRadiusSlider;

    public Slider waterDrainSlider;
    public Slider biomassSlider;
    public Slider energyRecoverySlider;

    public Slider maxHealthSlider;
    public Slider maxAgeSlider;
    public Slider populationSlider;

    public Slider mutationRateSlider;
    public Slider maxMutationIntensitySlider;
    public Slider percentOfIntenseMutationsSlider;

    public Slider maxPopulationSlider;

    public bool simulationActive = false;

    public bool onParameters = true;

    //public InputField worldBorderInput;

    /*
    seedDispersionRadius
    pollenDispersionRadius
    rootRadius
    waterDrainMod
    healthMod
    biomassMod
    energyRecoveryMod
    maxAge

    */

    void Awake()
    {
        plantController = GameObject.Find("PlantController").GetComponent<PlantController>();
    }


    void Start()
    {
        
        seedDispersionSlider.value = plantController.plantPrefab.GetComponent<Plant>().seedDispersionRadius;
        //seedDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(seedDispersionSlider.value * 100f) / 100f;
        
        pollenDispersionSlider.value = plantController.plantPrefab.GetComponent<Plant>().pollenDispersionRadius;
        //pollenDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(pollenDispersionSlider.value * 100f) / 100f;

        rootRadiusSlider.value = plantController.plantPrefab.GetComponent<Plant>().rootRadius;
        //rootRadiusSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + rootRadiusSlider.value;
        
        waterDrainSlider.value = plantController.plantPrefab.GetComponent<PlantDNAstats>().waterDrainMod;
        //waterDrainSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterDrainSlider.value;

        biomassSlider.value = plantController.baseStatValue * plantController.plantPrefab.GetComponent<PlantDNAstats>().biomassMod / 10f; //in 10
        //biomassSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + biomassSlider.value * 10f;
        
        energyRecoverySlider.value = plantController.plantPrefab.GetComponent<PlantDNAstats>().energyRecoveryMod; //in 10  
        //energyRecoverySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + energyRecoverySlider.value;

        maxHealthSlider.value = plantController.plantPrefab.GetComponent<PlantDNAstats>().healthMod * plantController.baseStatValue / 10f;
        //maxHealthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  maxHealthSlider.value;

        maxAgeSlider.value = plantController.plantPrefab.GetComponent<PlantDNAstats>().maxAge / 10f;
        //maxAgeSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxAgeSlider.value * 10f);

        populationSlider.value = plantController.initialPopulation;

        mutationRateSlider.value = plantController.mutationRate;
        mutationRateSlider.gameObject.SetActive(false);

        maxMutationIntensitySlider.value = plantController.maxMutationIntensity;
        maxMutationIntensitySlider.gameObject.SetActive(false);

        percentOfIntenseMutationsSlider.value = plantController.percentOfIntenseMutations;
        percentOfIntenseMutationsSlider.gameObject.SetActive(false);


        maxPopulationSlider.value = plantController.maxPopulation / 10f;
        //populationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  populationSlider.value;

        for(int i = 0; i < 2; i++){
            UpdateCurrentValueText();
            onParameters = !onParameters;
        }
        
        m_MyButton.onClick.AddListener(OnButtonClick);
    }

    public void SwitchPage()
    {
        onParameters = !onParameters;

        toMutations.gameObject.SetActive(onParameters);
        toParameters.gameObject.SetActive(!onParameters);

        seedDispersionSlider.gameObject.SetActive(onParameters); //TODO: add fruit nutritiousness
        pollenDispersionSlider.gameObject.SetActive(onParameters);
        rootRadiusSlider.gameObject.SetActive(onParameters);

        waterDrainSlider.gameObject.SetActive(onParameters);
        biomassSlider.gameObject.SetActive(onParameters);
        energyRecoverySlider.gameObject.SetActive(onParameters);

        maxHealthSlider.gameObject.SetActive(onParameters);
        maxAgeSlider.gameObject.SetActive(onParameters);
        populationSlider.gameObject.SetActive(onParameters);

        maxPopulationSlider.gameObject.SetActive(onParameters);

        mutationRateSlider.gameObject.SetActive(!onParameters);
        maxMutationIntensitySlider.gameObject.SetActive(!onParameters);
        percentOfIntenseMutationsSlider.gameObject.SetActive(!onParameters);
    }

    public void GameStarted()
    {
        Debug.Log("started2------");
        simulationActive = true;
        seedDispersionSlider.interactable = false; //TODO: add fruit nutritiousness
        pollenDispersionSlider.interactable = false;
        rootRadiusSlider.interactable = false;

        waterDrainSlider.interactable = false;
        biomassSlider.interactable = false;
        energyRecoverySlider.interactable = false;

        maxHealthSlider.interactable = false;
        maxAgeSlider.interactable = false;
        populationSlider.interactable = false;

        UpdateCurrentValueText();
    }

    public void GameEnded()
    {
        simulationActive = false;
        seedDispersionSlider.interactable = true; //TODO: add fruit nutritiousness
        pollenDispersionSlider.interactable = true;
        rootRadiusSlider.interactable = true;

        waterDrainSlider.interactable = true;
        biomassSlider.interactable = true;
        energyRecoverySlider.interactable = true;

        maxHealthSlider.interactable = true;
        maxAgeSlider.interactable = true;
        populationSlider.interactable = true;
    }

    void UpdateCurrentValueText(){

        //if(onParameters){

            seedDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + plantController.plantPrefab.GetComponent<Plant>().seedDispersionRadius;
            

            pollenDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + plantController.plantPrefab.GetComponent<Plant>().pollenDispersionRadius;


            rootRadiusSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + plantController.plantPrefab.GetComponent<Plant>().rootRadius;

            waterDrainSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + plantController.plantPrefab.GetComponent<PlantDNAstats>().waterDrainMod;


            biomassSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + (plantController.baseStatValue * plantController.plantPrefab.GetComponent<PlantDNAstats>().biomassMod);
            

            energyRecoverySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + plantController.plantPrefab.GetComponent<PlantDNAstats>().energyRecoveryMod;


            maxHealthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (plantController.plantPrefab.GetComponent<PlantDNAstats>().healthMod * plantController.baseStatValue);


            maxAgeSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (plantController.plantPrefab.GetComponent<PlantDNAstats>().maxAge);


            populationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  plantController.initialPopulation;

            maxPopulationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (plantController.maxPopulation);
        //} else {
            mutationRateSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  plantController.mutationRate;

            maxMutationIntensitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  plantController.maxMutationIntensity;
        
            percentOfIntenseMutationsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + plantController.percentOfIntenseMutations;
       // }
            Debug.Log("started32333 " + plantController.mutationRate);
    }

    /*

        void UpdateCurrentValueText(){

        if(onParameters){

            seedDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(seedDispersionSlider.value * 100f) / 100f;
            

            pollenDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(pollenDispersionSlider.value * 100f) / 100f;


            rootRadiusSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + rootRadiusSlider.value;
            

            waterDrainSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterDrainSlider.value * 100f) / 100f;


            biomassSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + (biomassSlider.value * 10f);
            

            energyRecoverySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + energyRecoverySlider.value;


            maxHealthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxHealthSlider.value * 10f);


            maxAgeSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxAgeSlider.value * 10f);


            populationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  populationSlider.value;

            maxPopulationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxPopulationSlider.value * 10f);
        } else {
            mutationRateSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(mutationRateSlider.value * 100f) / 100f;

            maxMutationIntensitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(maxMutationIntensitySlider.value * 100f) / 100f;
        
            percentOfIntenseMutationsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(percentOfIntenseMutationsSlider.value * 100f);
        }
    
    }
    */

    void OnButtonClick()
    {
        Debug.Log("New settings");

        if(onParameters){

            plantController.plantPrefab.GetComponent<Plant>().seedDispersionRadius = seedDispersionSlider.value;
            //seedDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(seedDispersionSlider.value * 100f) / 100f;

            plantController.plantPrefab.GetComponent<Plant>().pollenDispersionRadius = pollenDispersionSlider.value;
            //seedDispersionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(seedDispersionSlider.value * 100f) / 100f;

            plantController.plantPrefab.GetComponent<PlantDNAstats>().waterDrainMod = Mathf.Round(waterDrainSlider.value * 100f) / 100f;
            //waterDrainSlider.value = plantController.plantPrefab.GetComponent<PlantDNAstats>().waterDrainMod;

            plantController.plantPrefab.GetComponent<PlantDNAstats>().biomassMod = biomassSlider.value * 10f / plantController.baseStatValue; //in 10

            plantController.plantPrefab.GetComponent<Plant>().rootRadius = rootRadiusSlider.value; //in 10

            plantController.plantPrefab.GetComponent<PlantDNAstats>().energyRecoveryMod = Mathf.Round(energyRecoverySlider.value* 100f) / 100f;

            plantController.plantPrefab.GetComponent<PlantDNAstats>().healthMod = maxHealthSlider.value * 10f / plantController.baseStatValue;

            plantController.plantPrefab.GetComponent<PlantDNAstats>().maxAge = Mathf.Round(maxAgeSlider.value) * 10f;

            plantController.initialPopulation = (int) populationSlider.value;

            plantController.maxPopulation = (int) (maxPopulationSlider.value * 10);

        } else {

            plantController.mutationRate = Mathf.Round(mutationRateSlider.value * 100f) / 100f;

            plantController.maxMutationIntensity = Mathf.Round(maxMutationIntensitySlider.value * 100f) / 100f;

            plantController.percentOfIntenseMutations = Mathf.Round(percentOfIntenseMutationsSlider.value * 100f) / 100f;

        }

        UpdateCurrentValueText();
    }



}