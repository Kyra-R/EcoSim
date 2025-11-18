

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;

public class FullGameCycleE2ETest
{
   private GameObject controllerObject;
   private GameObject trackerGO;
   private AnimalController controller;


    [SetUp]
    public void SetUp()
    {
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

        controller.creaturePrefab.GetComponent<NeatNN>().InitializeBaseStructure(trackerGO.GetComponent<InnovationTracker>());

        //controller.FromNewGame();
        //yield return null;
    }

    [UnityTest]
    public IEnumerator FullCycle_ShouldRestoreSameNumberOfAnimals()
    {
        Debug.Log(controller == null);
        controller.initialPopulationPerSex = 2;
        controller.initialCarnivorePopulationPerSex = 1;

        controller.FromNewGame();
        yield return null;

        Assert.AreEqual(4, controller.population.Count);
        Assert.AreEqual(2, controller.carnivorePopulation.Count);


        int originalCount = controller.GetPopulation();

        Debug.Log(controller.GetPopulation());

        Assert.Greater(originalCount, 0, "Популяция должна быть создана");

        // Сохраняем состояние
        var data = new PopulationData
        {
            animals =  controller.GetPopulationToSave().ConvertAll(go =>
                go.GetComponent<Animal>().ToData())
        };

        SaveManager.SaveToFile<PopulationData>(data,"TestSave");
        // Очищаем сцену
        foreach (var animal in controller.population)
        {
            GameObject.DestroyImmediate(animal);
        }
        controller.population.Clear();

        yield return null;
        Assert.AreEqual(0, controller.GetPopulation(), "Популяция должна быть очищена");

        // Загружаем сохранение с сервера
        var loaded = SaveManager.LoadFromFile<PopulationData>("TestSave");

        Assert.AreEqual(data.animals.Count, loaded.animals.Count);

        List<AnimalSaveData> animalDataList = loaded.animals;

        foreach(var animal in animalDataList){
            controller.FromSave(animal);
        }
        int restoredCount = controller.GetPopulation();
        Assert.AreEqual(originalCount, restoredCount, "Популяция должна быть восстановлена");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.DestroyImmediate(controllerObject);
        GameObject.DestroyImmediate(trackerGO);
        yield return null;
    }
}