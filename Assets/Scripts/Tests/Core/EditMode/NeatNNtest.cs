using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NeatNNtest
{
    [Test]
    public void AddNeuron_ShouldAddNeuronWithCorrectId()
    {
        var animal = new GameObject();
        animal.AddComponent<NeatNN>();
        var nn = animal.GetComponent<NeatNN>();
        int id = nn.AddNeuron(new Neuron(1, NodeType.Input, 0.0f));
        Assert.IsTrue(nn.GetNeurons().ContainsKey(id));
    }

    [Test]
    public void AddConnection_ShouldAddConnectionBetweenNeurons()
    {
        var animal = new GameObject();
        animal.AddComponent<NeatNN>();
        var nn =  animal.GetComponent<NeatNN>();
        int from = nn.AddNeuron(new Neuron(1, NodeType.Input, 0.0f));
        int to = nn.AddNeuron(new Neuron(2, NodeType.Output, 0.0f));
        nn.AddConnection(new Connection(from, to, 0.1f, true, 1));

        Assert.IsTrue(nn.GetConnections().Exists(c => c.From == from && c.To == to));
    }

    [Test]
    public void Evaluate_ShouldReturnValidOutput()
    {
        var animal = new GameObject();
        var tracker = new GameObject();
        animal.AddComponent<Animal>();
        animal.AddComponent<NeatNN>();
        tracker.AddComponent<InnovationTracker>();

        animal.GetComponent<NeatNN>().InitializeBaseStructure(tracker.GetComponent<InnovationTracker>());
        var inputs = new float[] { 0.1f, 0.5f, 0.3f, 0.7f, 1, 0.2f, 0.4f, 0.3f };
        var outputs = animal.GetComponent<NeatNN>().FeedForward(inputs);

        Assert.AreEqual(6, outputs.Length);
        foreach (var o in outputs)
            Assert.IsTrue(o >= 0f && o <= 1f); 
    }


    /*[Test]
    public void Crossover_CreatesChildWithGenesFromParents()
    {
        var animal1 = new GameObject();
        animal1.AddComponent<Animal>();
        animal1.AddComponent<NeatNN>();
        var animal2 = new GameObject();
        animal2.AddComponent<Animal>();
        animal2.AddComponent<NeatNN>();

        var tracker = new GameObject();
        tracker.AddComponent<InnovationTracker>();
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
    }*/ //moved to PlayMode test

}
