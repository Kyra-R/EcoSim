using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetAnimalButton : MonoBehaviour
{
    public AnimalController animalController;

    public Button m_MyButton;

    public Button toMutations;
    public Button toParameters;
    public Button carnivores;
    public Button herbivores;

    public Slider speedSlider; //TODO: add fruit nutritiousness
    public Slider visionSlider;
    public Slider maxAgeSlider;
    public Slider energyLossSlider;
    public Slider hungerLossSlider;
    public Slider thirstLossSlider;
    public Slider maxHealthSlider;
    public Slider populationSlider;
    public Slider maxPopulationSlider;

    public Slider neuronMutationSlider;
    public Slider connectionMutationSlider;
    public Slider biasAndWeightsMutationSlider;

    public Slider mutationRateSlider;
    public Slider maxMutationIntensitySlider;
    public Slider percentOfIntenseMutationsSlider;

    public bool simulationActive = false;
    public bool isHerbivore = true;

    public bool onParameters = true;
    //public InputField worldBorderInput;

    void Awake()
    {
        animalController = GameObject.Find("AnimalController").GetComponent<AnimalController>();
    }

    void Start()
    {
        
        // Заполняем начальные значения
       /* waterAmountSlider.value = environmentController.GetComponent<EnvironmentController>().waterAmount;
        waterAmountSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterAmountSlider.value * 100f) / 100f;
        soilHostilitySlider.value = environmentController.GetComponent<EnvironmentController>().soilHostility;
        soilHostilitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(soilHostilitySlider.value * 100f) / 100f;

        fruitNutritionSlider.value = environmentController.GetComponent<EnvironmentController>().nutritionalValueOfFruits;
        fruitNutritionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + fruitNutritionSlider.value;
        waterInFruitsSlider.value = environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits;
        waterInFruitsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterInFruitsSlider.value;

        watersourceSlider.value = environmentController.GetComponent<EnvironmentController>().amountOfWatersources;
        watersourceSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + watersourceSlider.value;
        worldBorderSlider.value = environmentController.GetComponent<EnvironmentController>().worldBorder;
        worldBorderSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + worldBorderSlider.value;*/

        ChangeAndUpdateIsHerbivore(isHerbivore);

        maxPopulationSlider.value = animalController.maxPopulation / 10f;
        maxPopulationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxPopulationSlider.value * 10f);

        
        mutationRateSlider.value = animalController.mutationRate;
        mutationRateSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(mutationRateSlider.value * 100f) / 100f;
        mutationRateSlider.gameObject.SetActive(false);

        maxMutationIntensitySlider.value = animalController.maxMutationIntensity;
        maxMutationIntensitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(maxMutationIntensitySlider.value * 100f) / 100f;
        maxMutationIntensitySlider.gameObject.SetActive(false);

        percentOfIntenseMutationsSlider.value = animalController.percentOfIntenseMutations;
        percentOfIntenseMutationsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(percentOfIntenseMutationsSlider.value * 100f);
        percentOfIntenseMutationsSlider.gameObject.SetActive(false);


        neuronMutationSlider.value = animalController.neuronMutationRate;
        neuronMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(neuronMutationSlider.value * 100f) / 100f;
        neuronMutationSlider.gameObject.SetActive(false);

        connectionMutationSlider.value = animalController.connectionMutationRate;
        connectionMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(connectionMutationSlider.value * 100f) / 100f;
        connectionMutationSlider.gameObject.SetActive(false);

        biasAndWeightsMutationSlider.value = animalController.biasAndWeightsMutationRate;
        biasAndWeightsMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(biasAndWeightsMutationSlider.value * 100f) / 100f;
        biasAndWeightsMutationSlider.gameObject.SetActive(false);


        /*populationSlider.value = animalController.initialPopulationPerSex;
        populationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  populationSlider.value;

        visionSlider.value = animalController.creaturePrefab.GetComponent<Animal>().viewRadius;
        visionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  animalController.creaturePrefab.GetComponent<Animal>().viewRadius;

        maxAgeSlider.value = prefab.GetComponent<DNAstats>().maxAge;
        maxAgeSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxAgeSlider.value * 10f);

        //prefab.GetComponent<DNAstats>().energyDrainMod = Mathf.Round(energyLossSlider.value * 100f) / 100f;
        energyLossSlider.value = Mathf.Round(prefab.GetComponent<DNAstats>().energyDrainMod * 100f) / 100f;
        energyLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  energyLossSlider.value;
    
        hungerLossSlider.value = Mathf.Round(prefab.GetComponent<DNAstats>().hungerDrainMod * 100f) / 100f;
        hungerLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  hungerLossSlider.value;

        thirstLossSlider.value = Mathf.Round(prefab.GetComponent<DNAstats>().thirstDrainMod * 100f) / 100f;
        thirstLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  thirstLossSlider.value;
    
        maxHealthSlider.value = Mathf.Round((prefab.GetComponent<DNAstats>().healthMod * animalController.baseStatValue) * 100f) / 100f;
        maxHealthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  maxHealthSlider.value;*/
          
        m_MyButton.onClick.AddListener(OnButtonClick);
    }

    public void SwitchPage()
    {
        onParameters = !onParameters;

        toMutations.gameObject.SetActive(onParameters);
        toParameters.gameObject.SetActive(!onParameters);

        herbivores.gameObject.SetActive(onParameters);
        carnivores.gameObject.SetActive(onParameters);

        speedSlider.gameObject.SetActive(onParameters); //TODO: add fruit nutritiousness
        visionSlider.gameObject.SetActive(onParameters);
        maxAgeSlider.gameObject.SetActive(onParameters);
        energyLossSlider.gameObject.SetActive(onParameters);
        hungerLossSlider.gameObject.SetActive(onParameters);
        thirstLossSlider.gameObject.SetActive(onParameters);
        maxHealthSlider.gameObject.SetActive(onParameters);
        populationSlider.gameObject.SetActive(onParameters);
        maxPopulationSlider.gameObject.SetActive(onParameters);

        mutationRateSlider.gameObject.SetActive(!onParameters);
        maxMutationIntensitySlider.gameObject.SetActive(!onParameters);
        percentOfIntenseMutationsSlider.gameObject.SetActive(!onParameters);

        neuronMutationSlider.gameObject.SetActive(!onParameters);
        connectionMutationSlider.gameObject.SetActive(!onParameters);
        biasAndWeightsMutationSlider.gameObject.SetActive(!onParameters);

    }

    void AlterOriginalPopulation(bool herbivores){
        GameObject prefab;
        if(onParameters){
        if(herbivores){
            prefab = animalController.creaturePrefab;
            animalController.initialPopulationPerSex = (int) populationSlider.value;

            //prefab.GetComponent<Animal>().speed = Mathf.Round(speedSlider.value * 100f) / 100f;
            //speedSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(speedSlider.value * 100f) / 100f;
            
        } else {
            prefab = animalController.carnivoreCreaturePrefab;
            animalController.initialCarnivorePopulationPerSex = (int) populationSlider.value;
        }
        populationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  populationSlider.value;

        animalController.maxPopulation = (int) (maxPopulationSlider.value * 10f);
        maxPopulationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxPopulationSlider.value * 10f);

        prefab.GetComponent<Animal>().speed = Mathf.Round(speedSlider.value * 100f) / 100f;
        speedSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(speedSlider.value * 100f) / 100f;

        prefab.GetComponent<Animal>().viewRadius = visionSlider.value;
        visionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  visionSlider.value;

        prefab.GetComponent<DNAstats>().maxAge = maxAgeSlider.value * 10f;
        maxAgeSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxAgeSlider.value * 10f);

        prefab.GetComponent<DNAstats>().energyDrainMod = Mathf.Round(energyLossSlider.value * 100f) / 100f;
        energyLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(energyLossSlider.value * 100f) / 100f;
    
        prefab.GetComponent<DNAstats>().hungerDrainMod = Mathf.Round(hungerLossSlider.value * 100f) / 100f;
        hungerLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(hungerLossSlider.value * 100f) / 100f;

        prefab.GetComponent<DNAstats>().thirstDrainMod = Mathf.Round(thirstLossSlider.value * 100f) / 100f;
        thirstLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(thirstLossSlider.value * 100f) / 100f;
    
        prefab.GetComponent<DNAstats>().healthMod = Mathf.Round((maxHealthSlider.value / animalController.baseStatValue) * 100f) / 100f;
        maxHealthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  maxHealthSlider.value;
        } else {

        animalController.mutationRate = Mathf.Round(mutationRateSlider.value * 100f) / 100f;
        mutationRateSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(mutationRateSlider.value * 100f) / 100f;

        animalController.maxMutationIntensity = Mathf.Round(maxMutationIntensitySlider.value * 100f) / 100f;
        maxMutationIntensitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(maxMutationIntensitySlider.value * 100f) / 100f;
        
        animalController.percentOfIntenseMutations = Mathf.Round(percentOfIntenseMutationsSlider.value * 100f) / 100f;
        percentOfIntenseMutationsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(percentOfIntenseMutationsSlider.value * 100f);
        
        
        animalController.neuronMutationRate = Mathf.Round(neuronMutationSlider.value * 100f) / 100f;;
        neuronMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(neuronMutationSlider.value * 100f) / 100f;

        animalController.connectionMutationRate = Mathf.Round(connectionMutationSlider.value * 100f) / 100f;
        connectionMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(connectionMutationSlider.value * 100f) / 100f;

        animalController.biasAndWeightsMutationRate = Mathf.Round(biasAndWeightsMutationSlider.value * 100f) / 100f;
        biasAndWeightsMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(biasAndWeightsMutationSlider.value * 100f) / 100f;

        }
    }

    void UpdateMutationText(){
        
        mutationRateSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  animalController.mutationRate;

        maxMutationIntensitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  animalController.maxMutationIntensity;
        
        percentOfIntenseMutationsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  animalController.percentOfIntenseMutations * 100f;
        
        neuronMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  animalController.neuronMutationRate;

        connectionMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + animalController.connectionMutationRate;

        biasAndWeightsMutationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  animalController.biasAndWeightsMutationRate;
    }

    public void ChangeAndUpdateIsHerbivore(bool herbivores)
    {
        isHerbivore = herbivores;
        GameObject prefab;

        if(isHerbivore){
            prefab = animalController.creaturePrefab;
            populationSlider.value = animalController.initialPopulationPerSex;

        } else {
            prefab = animalController.carnivoreCreaturePrefab;
            populationSlider.value = animalController.initialCarnivorePopulationPerSex;
        }
        populationSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  populationSlider.value;

        speedSlider.value = Mathf.Round(prefab.GetComponent<Animal>().speed * 100f) / 100f;
        speedSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(speedSlider.value * 100f) / 100f;

        visionSlider.value = prefab.GetComponent<Animal>().viewRadius;
        visionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  prefab.GetComponent<Animal>().viewRadius;

        maxAgeSlider.value = prefab.GetComponent<DNAstats>().maxAge / 10f;
        maxAgeSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  (maxAgeSlider.value * 10f);

        energyLossSlider.value = Mathf.Round(prefab.GetComponent<DNAstats>().energyDrainMod * 100f) / 100f;
        energyLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(energyLossSlider.value * 100f) / 100f;

        hungerLossSlider.value = Mathf.Round(prefab.GetComponent<DNAstats>().hungerDrainMod * 100f) / 100f;
        hungerLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(hungerLossSlider.value * 100f) / 100f;

        thirstLossSlider.value = Mathf.Round(prefab.GetComponent<DNAstats>().thirstDrainMod * 100f) / 100f;
        thirstLossSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  Mathf.Round(thirstLossSlider.value * 100f) / 100f;

        maxHealthSlider.value = prefab.GetComponent<DNAstats>().healthMod * animalController.baseStatValue;
        maxHealthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " +  maxHealthSlider.value;
    }

    public void GameStarted()
    {
        Debug.Log("started");
        simulationActive = true;
        speedSlider.interactable = false; //TODO: add fruit nutritiousness
        visionSlider.interactable = false;
        maxAgeSlider.interactable = false;
        energyLossSlider.interactable = false;
        hungerLossSlider.interactable = false;
        thirstLossSlider.interactable = false;
        maxHealthSlider.interactable = false;
        populationSlider.interactable = false;

        UpdateMutationText();
        //worldBorderSlider.interactable = false;
        //watersourceSlider.interactable = false;
    }

    public void GameEnded()
    {
        simulationActive = false;
        speedSlider.interactable = true; //TODO: add fruit nutritiousness
        visionSlider.interactable = true;
        maxAgeSlider.interactable = true;
        energyLossSlider.interactable = true;
        hungerLossSlider.interactable = true;
        thirstLossSlider.interactable = true;
        maxHealthSlider.interactable = true;
        populationSlider.interactable = true;
        //worldBorderSlider.interactable = true;
        //watersourceSlider.interactable = true;
    }

    void OnButtonClick()
    {
        Debug.Log("New settings");

        AlterOriginalPopulation(isHerbivore);
    /* environmentController.GetComponent<EnvironmentController>().waterAmount = waterAmountSlider.value;
        waterAmountSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterAmountSlider.value * 100f) / 100f;
        environmentController.GetComponent<EnvironmentController>().soilHostility = soilHostilitySlider.value;
        soilHostilitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(soilHostilitySlider.value * 100f) / 100f;
    
        //fruitNutritionSlider.value = environmentController.GetComponent<EnvironmentController>().nutritionalValueOfFruits;
        environmentController.GetComponent<EnvironmentController>().nutritionalValueOfFruits = fruitNutritionSlider.value;
        fruitNutritionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + fruitNutritionSlider.value;
        //waterInFruitsSlider.value = environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits;
        environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits = waterInFruitsSlider.value;
        waterInFruitsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterInFruitsSlider.value;

        //waterInFruitsSlider.value = environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits;
        if(!simulationActive){
            environmentController.GetComponent<EnvironmentController>().amountOfWatersources = (int) watersourceSlider.value;
            watersourceSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + watersourceSlider.value;

            environmentController.GetComponent<EnvironmentController>().worldBorder = worldBorderSlider.value;
            worldBorderSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + worldBorderSlider.value;

            environmentController.GetComponent<EnvironmentController>().ChangeWorldBorder();
        }*/
    }



}