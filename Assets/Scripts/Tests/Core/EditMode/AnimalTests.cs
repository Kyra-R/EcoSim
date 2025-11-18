using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;

public class AnimalTests
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
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerObject);
        Object.DestroyImmediate(controller.creaturePrefab);
        Object.DestroyImmediate(controller.carnivoreCreaturePrefab);
    }

    [UnityTest]
    public IEnumerator FromNewGame_ShouldCreateInitialPopulation()
    {
        controller.initialPopulationPerSex = 2;
        controller.initialCarnivorePopulationPerSex = 1;

        controller.FromNewGame();
        yield return null;

        Assert.AreEqual(4, controller.population.Count);
        Assert.AreEqual(2, controller.carnivorePopulation.Count);
    }

    /*[UnityTest]
    public IEnumerator InstantiateOffspring_ShouldAddToCorrectPopulation()
    {
        controller.creaturePrefab.GetComponent<NeatNN>().ModifyBaseStructureHerbivore(trackerGO.GetComponent<InnovationTracker>());
        var dummyStats = controller.creaturePrefab.GetComponent<DNAstats>();
        var dummyNN = controller.creaturePrefab.GetComponent<NeatNN>();
       // Debug.Log(dummyNN == null)

        int before = controller.population.Count;
        controller.InstantiateOffspring(Vector2.zero, "Baby", 1, dummyStats, dummyStats, dummyNN, dummyNN, true);

        yield return null;

        Assert.AreEqual(before + 1, controller.population.Count);
    }*/

    [Test]
    public void GetPopulation_ShouldReturnCorrectCount()
    {
        var creature = Object.Instantiate(controller.creaturePrefab);
        controller.population.Add(creature);

        Assert.AreEqual(1, controller.GetPopulation());
    }

    [Test]
    public void GetAverageEnergyDrain_ShouldReturnPositive()
    {
        var creature = Object.Instantiate(controller.creaturePrefab);
        creature.GetComponent<DNAstats>().energyDrainMod = 1.0f;
        controller.population.Add(creature);

        float averageDrain = controller.GetAverageEnergyDrain();
        Assert.Greater(averageDrain, 0);
    }
}
