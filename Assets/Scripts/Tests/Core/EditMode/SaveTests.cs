using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SaveTests
{
    // Start is called before the first frame update
    [Test]
    public void SaveAndLoad_ShouldRestoreSameData()
    {
        var data = new PopulationData();
        data.animals = new List<AnimalSaveData> {
            new AnimalSaveData { }, new AnimalSaveData { }
        };

        SaveManager.SaveToFile<PopulationData>(data,"TestSave");
        var loaded = SaveManager.LoadFromFile<PopulationData>("TestSave");

        Assert.AreEqual(data.animals.Count, loaded.animals.Count);
    }

}
