using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InnovationTrackerData
{
    public int nextNodeId;
    public int currentInnovation;

    // Инновации: ключ = (from, to), значение = innovation number
    public List<MutationRecordData> innovations = new List<MutationRecordData>();
}

[System.Serializable]
public class MutationRecordData
{
    public int from;
    public int to;
    public int innovation;

    public MutationRecordData(int from, int to, int innovation)
    {
        this.from = from;
        this.to = to;
        this.innovation = innovation;
    }
}