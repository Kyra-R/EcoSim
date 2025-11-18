using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class NeatTest : MonoBehaviour
{

    private GameObject controllerObject;
    private GameObject squareField;
    private GameObject mainCamera;
    private GameObject tracker;
    private AnimalController controller;
    // Start is called before the first frame update
[Test]
    public void Crossover_CreatesChildWithGenesFromParents()
    {
        squareField = new GameObject("Square");
        //sqareObject.AddComponent<SpriteRenderer>();
        squareField.AddComponent<SpriteRenderer>();

        tracker = new GameObject("InnovationTracker");
        tracker.AddComponent<InnovationTracker>();

        var envGO = new GameObject("EnvironmentController");
        envGO.AddComponent<EnvironmentController>();

        mainCamera = new GameObject("Main Camera");
        Camera cam = mainCamera.AddComponent<Camera>();
        mainCamera.AddComponent<CameraController2D>();
        mainCamera.GetComponent<CameraController2D>().cam = cam;

        envGO.GetComponent<EnvironmentController>().camera = mainCamera;


        controllerObject = new GameObject("AnimalController");
        controller = controllerObject.AddComponent<AnimalController>();
        controller.globalInnovationTracker = tracker.GetComponent<InnovationTracker>();
        controller.environmentController = envGO.GetComponent<EnvironmentController>();

        var animal1 = new GameObject();
        animal1.AddComponent<Animal>();
        animal1.AddComponent<NeatNN>();
        var animal2 = new GameObject();
        animal2.AddComponent<Animal>();
        animal2.AddComponent<NeatNN>();

        // Arrange

        NeatNN parent1 = animal1.GetComponent<NeatNN>();
        NeatNN parent2 = animal2.GetComponent<NeatNN>();



        for (int i = 0; i < 3; i++)
        {
            parent1.AddNeuron(new Neuron(i, NodeType.Input, 0.0f));
            parent2.AddNeuron(new Neuron(i, NodeType.Input, 0.0f));
        }

        parent1.AddNeuron(new Neuron(100, NodeType.Output, 0.0f));
        parent2.AddNeuron(new Neuron(100, NodeType.Output, 0.0f));

      
        parent1.AddConnection(new Connection(0, 100, 1.0f, true, tracker.GetComponent<InnovationTracker>().GetInnovation(0, 100)));
        parent1.AddConnection(new Connection(1, 100, 0.5f, true, tracker.GetComponent<InnovationTracker>().GetInnovation(1, 100)));
        
        parent2.AddConnection(new Connection(0, 100, -1.0f, true, tracker.GetComponent<InnovationTracker>().GetInnovation(0, 100))); // Разный вес
        parent2.AddConnection(new Connection(2, 100, 0.8f, true, tracker.GetComponent<InnovationTracker>().GetInnovation(2, 100)));  // Уникальное соединение


        NeatNN child = NeatNN.Crossover(parent1, parent2, true);

        Assert.IsTrue(child.GetConnections().Count >= 2); 
        Assert.IsTrue(child.GetNeurons().ContainsKey(100)); 

        
        HashSet<int> innovations = new HashSet<int>();
        foreach (var c in child.GetConnections())
        {
            Assert.IsFalse(innovations.Contains(c.Innovation), "Дублирующий Innovation!");
            innovations.Add(c.Innovation);
        }
    }
}
