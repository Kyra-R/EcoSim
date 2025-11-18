using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;


public class AnimalTest
{
    private GameObject controllerObject;
    private GameObject squareField;
    private GameObject mainCamera;
    private GameObject trackerGO;
    private AnimalController controller;
[SetUp]
    public void SetUp()
    {

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
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerObject);
        Object.DestroyImmediate(controller.creaturePrefab);
        Object.DestroyImmediate(controller.carnivoreCreaturePrefab);
    }

    [UnityTest]
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
    }

    
}
