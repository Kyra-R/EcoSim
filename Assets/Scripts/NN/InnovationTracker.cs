using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationTracker: MonoBehaviour
{
    private int currentInnovation = 0;
    private int nextNodeId = 0;
    private List<MutationRecord> records = new List<MutationRecord>();

    public int GetInnovation(int from, int to)
    {
        for (int i = 0; i < records.Count; i++)
        {
            if (records[i].From == from && records[i].To == to)
                return records[i].Innovation;
        }

        // Новая мутация
        int innovation = currentInnovation++;
        records.Add(new MutationRecord(from, to, innovation));
        return innovation;
    }

    public void EndGame()
    {
        records.Clear();
        nextNodeId = 0;
        currentInnovation = 0;
    }

    public int GetNextNodeId()
    {
//        Debug.Log(nextNodeId);
        return nextNodeId++;
    }

    public int GetNextNodeIdWithoutIncrease()
    {
        return nextNodeId;
    }

    public void IncreaseNextNodeId()
    {
        nextNodeId++;
    }


    public InnovationTrackerData ToData()
    {
        InnovationTrackerData data = new InnovationTrackerData();
        data.nextNodeId = nextNodeId;
        data.currentInnovation = currentInnovation;

        foreach(var mutation in records)
        {
            data.innovations.Add(new MutationRecordData(mutation.From, mutation.To, mutation.Innovation));
        }

        return data;
    }

    public void FromData(InnovationTrackerData data)
    {
        this.nextNodeId = data.nextNodeId;
        this.currentInnovation = data.currentInnovation;

        foreach(var mutation in data.innovations)
        {
            records.Add(new MutationRecord(mutation.from, mutation.to, mutation.innovation));
        }
    }
}

class MutationRecord
{
    public int From;
    public int To;
    public int Innovation;

    public MutationRecord(int from, int to, int innovation)
    {
        From = from;
        To = to;
        Innovation = innovation;
        }
}