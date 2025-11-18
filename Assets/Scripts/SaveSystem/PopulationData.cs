using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PopulationData
{
    public List<AnimalSaveData> animals = new List<AnimalSaveData>();

    public List<PlantSaveData> plants = new List<PlantSaveData>();

    public InnovationTrackerData innovationTrackerData;

    public EnvironmentSaveData environmentSaveData;

    public AnimalControllerData animalControllerData;
    
    public PlantControllerData plantControllerData;
}
