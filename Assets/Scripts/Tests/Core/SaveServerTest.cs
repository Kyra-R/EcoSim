using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SaveServerTest
{

    private GameObject saveControllerObject;
    private SaveController saveController;
    private GameObject envObject;
    private GameObject animalObject;
    private GameObject plantObject;
    private GameObject graphObject;
    private GameObject trackerGO;
    private GameObject squareField;
    private GameObject mainCamera;

    

    [SetUp]
    public void SetUp()
    {

        squareField = new GameObject("Square");
        //sqareObject.AddComponent<SpriteRenderer>();
        squareField.AddComponent<SpriteRenderer>();

        var canvasGO = new GameObject("Canvas", typeof(Canvas));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        
        var buttonGO = new GameObject("SomeButton", typeof(RectTransform), typeof(Button), typeof(Image));
        buttonGO.transform.SetParent(canvasGO.transform);
        var button = buttonGO.GetComponent<Button>();

        
        envObject = new GameObject("EnvironmentController");
        envObject.AddComponent<EnvironmentController>();

        mainCamera = new GameObject("Main Camera");
        Camera cam = mainCamera.AddComponent<Camera>();
        mainCamera.AddComponent<CameraController2D>();
        mainCamera.GetComponent<CameraController2D>().cam = cam;

        envObject.GetComponent<EnvironmentController>().camera = mainCamera;

        trackerGO = new GameObject("InnovationTracker");
        trackerGO.AddComponent<InnovationTracker>();
       // controller.globalInnovationTracker = trackerGO.GetComponent<InnovationTracker>();

        animalObject = new GameObject("AnimalController");
        animalObject.AddComponent<AnimalController>();
        animalObject.GetComponent<AnimalController>().environmentController = envObject.GetComponent<EnvironmentController>();

        plantObject = new GameObject("PlantController");
        plantObject.AddComponent<PlantController>();
        plantObject.GetComponent<PlantController>().environmentController = envObject.GetComponent<EnvironmentController>();

        var buttonGO2 = new GameObject("SomeButton2", typeof(RectTransform), typeof(Button), typeof(Image));
        buttonGO2.transform.SetParent(canvasGO.transform);
        var graph = new GameObject("graph");
        graph.transform.SetParent(canvasGO.transform);

        graphObject = new GameObject("MenuController");
        graphObject.AddComponent<GraphLinesController>();

        saveControllerObject = new GameObject("SaveController");
        saveController = saveControllerObject.AddComponent<SaveController>();
        saveController.graphController = graph;
        saveController.newGameButton = buttonGO2.GetComponent<Button>();
        saveController.loadGameButton = buttonGO2.GetComponent<Button>();
        saveController.saveButton = buttonGO2.GetComponent<Button>();
        saveController.clearButton = buttonGO2.GetComponent<Button>();
        saveController.loadFromServerButton = buttonGO.GetComponent<Button>();
        saveController.saveToServerButton = buttonGO.GetComponent<Button>();

        saveControllerObject.GetComponent<SaveController>().loadFromServerButton = button;
        saveControllerObject.GetComponent<SaveController>().saveToServerButton = button;

   
        saveController.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(envObject);
        Object.DestroyImmediate(animalObject);
        Object.DestroyImmediate(saveControllerObject);
    }

    [UnityTest]
    public IEnumerator UploadJson_ShouldReturnSuccess()
    {
        var data = new PopulationData();
        data.animals = new List<AnimalSaveData> {
            new AnimalSaveData { }, new AnimalSaveData { }
        };

        yield return saveController.UploadJson<PopulationData>(data);

    }


}
