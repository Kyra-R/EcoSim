
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;


public class FullGameCycleE2ETestGameMode
{
   private GameObject controllerObject;
   private GameObject trackerGO;
   private AnimalController controller;
    private GameObject squareField;
    private GameObject mainCamera;


    [UnitySetUp]
    public IEnumerator SetUp()
    {
     /*// Создаем объекты-контейнеры
        controllerObject = new GameObject("AnimalController");
        controller = controllerObject.AddComponent<AnimalController>();

        // Заглушки prefab'ов
        controller.creaturePrefab = new GameObject("HerbivorePrefab");
        controller.creaturePrefab.AddComponent<SpriteRenderer>();
        controller.creaturePrefab.AddComponent<Animal>();
        controller.creaturePrefab.AddComponent<NeatNN>();
    
        controller.creaturePrefab.AddComponent<DNA>();
        controller.creaturePrefab.AddComponent<DNAstats>();
        controller.creaturePrefab.AddComponent<CreatureMover>();

        controller.carnivoreCreaturePrefab = GameObject.Instantiate(controller.creaturePrefab);
        controller.population = new List<GameObject>();
        controller.carnivorePopulation = new List<GameObject>();

        // Глобальный InnovationTracker
        trackerGO = new GameObject("InnovationTracker");
        trackerGO.AddComponent<InnovationTracker>();
        controller.globalInnovationTracker = trackerGO.GetComponent<InnovationTracker>();

        var envGO = new GameObject("EnvironmentController");
        envGO.AddComponent<EnvironmentController>();
        controller.environmentController = envGO.GetComponent<EnvironmentController>();

        controller.creaturePrefab.GetComponent<NeatNN>().InitializeBaseStructure(trackerGO.GetComponent<InnovationTracker>());*/
        

        squareField = new GameObject("Square");
        //sqareObject.AddComponent<SpriteRenderer>();
        squareField.AddComponent<SpriteRenderer>();

        trackerGO = new GameObject("InnovationTracker");
        trackerGO.AddComponent<InnovationTracker>();

        var envGO = new GameObject("EnvironmentController");
        envGO.AddComponent<EnvironmentController>();

        mainCamera = new GameObject("Main Camera");
        Camera cam = mainCamera.AddComponent<Camera>();
        mainCamera.AddComponent<CameraController2D>();
        mainCamera.GetComponent<CameraController2D>().cam = cam;

        envGO.GetComponent<EnvironmentController>().camera = mainCamera;


        controllerObject = new GameObject("AnimalController");
        controller = controllerObject.AddComponent<AnimalController>();
        controller.globalInnovationTracker = trackerGO.GetComponent<InnovationTracker>();
        controller.environmentController = envGO.GetComponent<EnvironmentController>();


        // Заглушки prefab'ов
        controller.creaturePrefab = new GameObject("HerbivorePrefab");
        controller.creaturePrefab.AddComponent<SpriteRenderer>();
        controller.creaturePrefab.AddComponent<NeatNN>();
    
        controller.creaturePrefab.AddComponent<DNA>();
        controller.creaturePrefab.AddComponent<DNAstats>();
        controller.creaturePrefab.AddComponent<CreatureMover>();

        controller.creaturePrefab.AddComponent<Animal>();

        controller.carnivoreCreaturePrefab = GameObject.Instantiate(controller.creaturePrefab);
        controller.population = new List<GameObject>();
        controller.carnivorePopulation = new List<GameObject>();

        controller.creaturePrefab.GetComponent<NeatNN>().InitializeBaseStructure(trackerGO.GetComponent<InnovationTracker>());

        controller.initialPopulationPerSex = 2;
        controller.initialCarnivorePopulationPerSex = 1;

        controller.FromNewGame();
        yield return null;

        Assert.AreEqual(4, controller.population.Count);
        Assert.AreEqual(2, controller.carnivorePopulation.Count);
    }

    [UnityTest]
    public IEnumerator FullCycle_ShouldRestoreSameNumberOfAnimals()
    {


        int originalCount = controller.GetPopulation();

        Assert.Greater(originalCount, 0, "Популяция должна быть создана");

        // 2. Сохраняем состояние
        var data = new PopulationData
        {
            animals =  controller.GetPopulationToSave().ConvertAll(go =>
                go.GetComponent<Animal>().ToData())
        };

        SaveManager.SaveToFile<PopulationData>(data,"TestSave");
        // 3. Очищаем сцену
        foreach (var animal in controller.population)
        {
            GameObject.Destroy(animal);
        }
        controller.population.Clear();

        yield return null;
        Assert.AreEqual(0, controller.GetPopulation(), "Популяция должна быть очищена");

        // 4. Загружаем сохранение с сервера
        var loaded = SaveManager.LoadFromFile<PopulationData>("TestSave");


        Assert.AreEqual(data.animals.Count, loaded.animals.Count);

        //controller.FromData(loaded.animalControllerData);]
        foreach(var animal in loaded.animals){
            controller.FromSave(animal);
        }
        


        int restoredCount = controller.GetPopulation();
        Assert.AreEqual(originalCount, restoredCount, "Популяция должна быть восстановлена");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(controllerObject);
        GameObject.Destroy(trackerGO);
        yield return null;
    }
}