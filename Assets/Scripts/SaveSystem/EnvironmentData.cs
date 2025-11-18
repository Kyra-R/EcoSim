using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentSaveData
{
    public float worldBorder;

    public float nutritionalValueOfFruits;

    //public int amountOfWatersources;
    public float waterEvaporCoeff;

    public float rainFrequency;

    public float rainStrength;

    public float waterAmount;

    public float soilHostility;

    public float timer;

    public List<WatersourceSaveData> watersources = new List<WatersourceSaveData>();
  
}
