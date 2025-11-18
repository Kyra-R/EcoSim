using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetEnvButton : MonoBehaviour
{
    public GameObject environmentController;

    public Button m_MyButton;

    public Slider waterAmountSlider; 
    public Slider soilHostilitySlider;
    public Slider fruitNutritionSlider;
    public Slider waterInFruitsSlider;
    public Slider watersourceSlider;
    public Slider worldBorderSlider;

    public Slider rainFrequencySlider;
    public Slider rainStrengthSlider;
    public Slider waterEvaporCoeffSlider;


    public bool simulationActive = false;
 

    void Awake()
    {
        environmentController = GameObject.Find("EnvironmentController");
    }

    void Start()
    {
        
        waterAmountSlider.value = environmentController.GetComponent<EnvironmentController>().waterAmount;
        //waterAmountSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterAmountSlider.value * 100f) / 100f;
        soilHostilitySlider.value = environmentController.GetComponent<EnvironmentController>().soilHostility;
        //soilHostilitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(soilHostilitySlider.value * 100f) / 100f;

        fruitNutritionSlider.value = environmentController.GetComponent<EnvironmentController>().nutritionalValueOfFruits;
        //fruitNutritionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + fruitNutritionSlider.value;
        waterInFruitsSlider.value = environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits;
        //waterInFruitsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterInFruitsSlider.value;

        watersourceSlider.value = environmentController.GetComponent<EnvironmentController>().amountOfWatersources;
       // watersourceSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + watersourceSlider.value;
        worldBorderSlider.value = environmentController.GetComponent<EnvironmentController>().worldBorder;
        //worldBorderSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + worldBorderSlider.value;

        rainFrequencySlider.value = environmentController.GetComponent<EnvironmentController>().rainFrequency;
        //rainFrequencySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + rainFrequencySlider.value;
        rainStrengthSlider.value = environmentController.GetComponent<EnvironmentController>().rainStrength;
       // rainStrengthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + rainStrengthSlider.value;
        waterEvaporCoeffSlider.value = environmentController.GetComponent<EnvironmentController>().waterEvaporCoeff;
        //waterEvaporCoeffSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterEvaporCoeffSlider.value;
        UpdateText();

        m_MyButton.onClick.AddListener(OnButtonClick);
    }

    public void GameStarted()
    {
        Debug.Log("started");
        simulationActive = true;
        worldBorderSlider.interactable = false;
        watersourceSlider.interactable = false;
        UpdateText();
    }

    public void GameEnded()
    {
        simulationActive = false;
        worldBorderSlider.interactable = true;
        watersourceSlider.interactable = true;
    }

    public void UpdateText()
    {
        waterAmountSlider.value = environmentController.GetComponent<EnvironmentController>().waterAmount;
        waterAmountSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().waterAmount;

        soilHostilitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().soilHostility;
    
        fruitNutritionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().nutritionalValueOfFruits;
        waterInFruitsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits;

        watersourceSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().amountOfWatersources;
        worldBorderSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().worldBorder;

        rainFrequencySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().rainFrequency;
        rainStrengthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().rainStrength;
        waterEvaporCoeffSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + environmentController.GetComponent<EnvironmentController>().waterEvaporCoeff;
    }

    /*    public void UpdateText()
    {
        waterAmountSlider.value = environmentController.GetComponent<EnvironmentController>().waterAmount;
        waterAmountSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterAmountSlider.value * 100f) / 100f;

        soilHostilitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(soilHostilitySlider.value * 100f) / 100f;
    
        fruitNutritionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + fruitNutritionSlider.value;
        waterInFruitsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterInFruitsSlider.value;

        watersourceSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + watersourceSlider.value;
        worldBorderSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + worldBorderSlider.value;

        rainFrequencySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(rainFrequencySlider.value * 100f) / 100f;
        rainStrengthSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(rainStrengthSlider.value * 100f) / 100f;
        waterEvaporCoeffSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterEvaporCoeffSlider.value * 100f) / 100f;
    }*/

    void OnButtonClick()
    {
        Debug.Log("New settings");
        environmentController.GetComponent<EnvironmentController>().waterAmount = Mathf.Round(waterAmountSlider.value * 100f) / 100f;
        //waterAmountSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(waterAmountSlider.value * 100f) / 100f;
        environmentController.GetComponent<EnvironmentController>().soilHostility = Mathf.Round(soilHostilitySlider.value * 100f) / 100f;
        //soilHostilitySlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + Mathf.Round(soilHostilitySlider.value * 100f) / 100f;
    
        environmentController.GetComponent<EnvironmentController>().nutritionalValueOfFruits = fruitNutritionSlider.value;
        //fruitNutritionSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + fruitNutritionSlider.value;

        environmentController.GetComponent<EnvironmentController>().amountOfWaterInFruits = waterInFruitsSlider.value;
        //waterInFruitsSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + waterInFruitsSlider.value;

        environmentController.GetComponent<EnvironmentController>().rainFrequency = Mathf.Round(rainFrequencySlider.value * 100f) / 100f;

        environmentController.GetComponent<EnvironmentController>().rainStrength = Mathf.Round(rainStrengthSlider.value * 100f) / 100f;

        environmentController.GetComponent<EnvironmentController>().waterEvaporCoeff = Mathf.Round(waterEvaporCoeffSlider.value * 100f) / 100f;

        if(!simulationActive){
            environmentController.GetComponent<EnvironmentController>().amountOfWatersources = (int) watersourceSlider.value;
            //watersourceSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + watersourceSlider.value;

            environmentController.GetComponent<EnvironmentController>().worldBorder = worldBorderSlider.value;
            //worldBorderSlider.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Текущее значение: " + worldBorderSlider.value;

            environmentController.GetComponent<EnvironmentController>().ChangeWorldBorder();
        }

        UpdateText();
    }

}