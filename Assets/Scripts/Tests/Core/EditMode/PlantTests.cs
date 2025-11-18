using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;

public class PlantTests
{
    private GameObject controllerObject;
    private PlantController controller;

    [SetUp]
    public void SetUp()
    {
        controllerObject = new GameObject("PlantController");
        controller = controllerObject.AddComponent<PlantController>();

        // Настройка заглушки prefab
        controller.plantPrefab = new GameObject("PlantPrefab");
        controller.plantPrefab.AddComponent<Plant>();
        controller.plantPrefab.AddComponent<PlantDNAstats>();

        controller.population = new List<GameObject>();

        
        var envGO = new GameObject("EnvironmentController");
        var env = envGO.AddComponent<EnvironmentController>();
        env.worldBorder = 20f; 
        controller.environmentController = env;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerObject);
        Object.DestroyImmediate(controller.plantPrefab);
    }

    [UnityTest]
    public IEnumerator NewGame_ShouldCreateInitialPopulation()
    {
        controller.initialPopulation = 3;

        controller.FromNewGame();
        yield return null;

        Assert.AreEqual(3, controller.population.Count);
        foreach (var plant in controller.population)
        {
            Assert.IsNotNull(plant);
            Assert.AreEqual("FirstGenPlant", plant.name);
        }
    }

    [UnityTest]
    public IEnumerator InstantiateOffspring_ShouldAddPlantToPopulation()
    {
        var dummyStats = controller.plantPrefab.GetComponent<PlantDNAstats>();
        int beforeCount = controller.population.Count;

        controller.InstantiateOffspring(Vector2.zero, "BabyPlant", 2, dummyStats, dummyStats);
        yield return null;

        Assert.AreEqual(beforeCount + 1, controller.population.Count);
        GameObject newPlant = controller.population[^1]; // последний элемент
        Assert.IsTrue(newPlant.name.Contains("BabyPlant"));
        Assert.AreEqual(2, newPlant.GetComponent<Plant>().generation);
    }
}