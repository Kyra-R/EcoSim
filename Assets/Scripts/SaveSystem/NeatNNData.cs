using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeatNNData
{
    public List<NeuronData> neurons = new List<NeuronData>();
    public List<ConnectionData> connections = new List<ConnectionData>();
    public int[] inputIds;
    public int[] outputIds;
}

[System.Serializable]
public class NeuronData
{
    public int id;
    public NodeType type;
    public float bias;
}

[System.Serializable]
public class ConnectionData
{
    public int from;
    public int to;
    public float weight;
    public bool enabled;
    public int innovation;
}
