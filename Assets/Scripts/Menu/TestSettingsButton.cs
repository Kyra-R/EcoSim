using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestSettingsButton : MonoBehaviour
{

    public Button m_MyButton;

    AnimalController animalController;
    EnvironmentController environmentController;
    PlantController plantController;
    
    GameObject prefab;

    // Start is called before the first frame update

    
    void Awake()
    {
        animalController = GameObject.Find("AnimalController").GetComponent<AnimalController>();
        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();
        plantController = GameObject.Find("PlantController").GetComponent<PlantController>();
        prefab = animalController.creaturePrefab;
    }

    void Start()
    {
        m_MyButton.onClick.AddListener(OnButtonClick);
    }


    void OnButtonClick()
    {
        animalController.initialPopulationPerSex = 2;
        animalController.initialCarnivorePopulationPerSex = 0;
        plantController.initialPopulation = 20;
        environmentController.worldBorder = 50f;
        environmentController.amountOfWatersources = 4;
        environmentController.ChangeWorldBorder();
        Debug.Log("Button");

    }


}
