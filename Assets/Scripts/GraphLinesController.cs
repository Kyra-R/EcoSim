using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphLinesController : MonoBehaviour //TODO: amke updates at save and called from outher class
{
    // Start is called before the first frame update
    public GameObject canvas;
    public GameObject animalController;
    public PlantController plantController;
    private DD_DataDiagram ddDataDiagram;

    List <GameObject> lines = new List<GameObject>();

    List <GameObject> carnivoreLines = new List<GameObject>();

    List <GameObject> plantLines = new List<GameObject>();

    private float timeBeforeUpdateGraph = 30f;

    private float counter = 0;

    float updateTime = 30f;

    bool activated = false;

    void Start() //
    {
        animalController = GameObject.Find("AnimalController");
        plantController = GameObject.Find("PlantController").GetComponent<PlantController>();
        canvas = GameObject.Find("Canvas");
        ddDataDiagram = canvas.transform.GetChild(2).GetComponent<DD_DataDiagram>();

    }

    void Update(){
        if(activated){
            timeBeforeUpdateGraph -= Time.deltaTime;
            if(timeBeforeUpdateGraph <= 0){
                timeBeforeUpdateGraph = updateTime;
                counter += updateTime; //do just counter
                //UpdateGraph();
                ProcessHerbivoreData();
                ProcessCarnivoreData();
                ProcessPlantData();
            }
        }
    }

    public void EndGame()
    {
        foreach(var line in lines){
            ddDataDiagram.DestroyLine(line);
        }
        foreach(var line in carnivoreLines){
            ddDataDiagram.DestroyLine(line);
        }
        foreach(var line in plantLines){
            ddDataDiagram.DestroyLine(line);
        }

        lines.Clear();
        carnivoreLines.Clear();
        plantLines.Clear();

        activated = false;
    }

    void UpdateGraph(){
        UpdateHerbivorePopulation();
        UpdateHerbivoreEnergyDrain(); //do via Invoke(func, time)
        UpdateHerbivoreVisionRadius();
        UpdateHerbivoreSpeed();
        UpdateHerbivoreMaxHealth();
        UpdateHerbivoreThirstDrain();
        UpdateHerbivoreHungerDrain();
        UpdateHerbivoreMaxAge();
    }

    public void TurnOffHerbivoreLines()
    {
        foreach(var line in lines)
        {
            line.GetComponent<DD_Lines>().IsShow = false;
        }
    }

    public void TurnOnHerbivoreLines()
    {
        foreach(var line in lines)
        {
            line.GetComponent<DD_Lines>().IsShow = true;
        }
    }

    public void TurnOffCarnivoreLines()
    {
        foreach(var line in carnivoreLines)
        {
            line.GetComponent<DD_Lines>().IsShow = false;
        }
    }

    public void TurnOnCarnivoreLines()
    {
        foreach(var line in carnivoreLines)
        {
            line.GetComponent<DD_Lines>().IsShow = true;
        }
    }


    public void TurnOffPlantLines()
    {
        foreach(var line in plantLines)
        {
            line.GetComponent<DD_Lines>().IsShow = false;
        }
    }

    public void TurnOnPlantLines()
    {
        foreach(var line in plantLines)
        {
            line.GetComponent<DD_Lines>().IsShow = true;
        }
    }

    void AddHervbivoreData(){
        GameObject line = ddDataDiagram.AddLine("herbivorePopulation", new Color(0, 1f, 0)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivorePopulation", 0.1f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreVision", new Color(0, 0, 1f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreVisionRadius", 0.15f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreEnergyDrain"); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreEnergyDrain", 0.2f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreSpeed", new Color(0.33f, 0.50f, 0.85f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreSpeed", 0.25f, updateTime);


        line = ddDataDiagram.AddLine("herbivoreMaxHealth", new Color(1f, 0, 0)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreMaxHealth", 0.3f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreThirstDrain", new Color(0, 1f, 1f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreThirstDrain", 0.4f, updateTime);

        
        line = ddDataDiagram.AddLine("herbivoreHungerDrain", new Color(1f, 0.5f, 0)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreHungerDrain", 0.35f, updateTime);


        line = ddDataDiagram.AddLine("herbivoreMaxAge", new Color(0.48f, 0.3f, 0.3f)); 
        lines.Add(line);
    }


    void AddCarnivoreData()
    {
        GameObject line = ddDataDiagram.AddLine("carnivorePopulation", new Color(0.2f, 0.4f, 0.1f)); 
        carnivoreLines.Add(line);
        Debug.Log(carnivoreLines.Count + "carnivores");
        //InvokeRepeating("UpdateHerbivorePopulation", 0.1f, updateTime)
        line = ddDataDiagram.AddLine("carnivoreVision", new Color(0, 0.1f, 0.95f)); 
        carnivoreLines.Add(line);
        //InvokeRepeating("UpdatecarnivoreVisionRadius", 0.15f, updateTime);

        line = ddDataDiagram.AddLine("carnivoreEnergyDrain", new Color(1.0f, 1.0f, 0.0f)); 
        carnivoreLines.Add(line);
        //InvokeRepeating("UpdatecarnivoreEnergyDrain", 0.2f, updateTime);

        line = ddDataDiagram.AddLine("carnivoreSpeed", new Color(0.33f, 0.60f, 0.95f)); 
        carnivoreLines.Add(line);
        //InvokeRepeating("UpdatecarnivoreSpeed", 0.25f, updateTime);


        line = ddDataDiagram.AddLine("carnivoreMaxHealth", new Color(0.8f, 0, 0)); 
        carnivoreLines.Add(line);
        //InvokeRepeating("UpdatecarnivoreMaxHealth", 0.3f, updateTime);

        line = ddDataDiagram.AddLine("carnivoreThirstDrain", new Color(0, 0.9f, 1f)); 
        carnivoreLines.Add(line);
        //InvokeRepeating("UpdatecarnivoreThirstDrain", 0.4f, updateTime);

        
        line = ddDataDiagram.AddLine("carnivoreHungerDrain", new Color(1f, 0.6f, 0.1f)); 
        carnivoreLines.Add(line);
        //InvokeRepeating("UpdatecarnivoreHungerDrain", 0.35f, updateTime);


        line = ddDataDiagram.AddLine("carnivoreMaxAge", new Color(0.55f, 0.35f, 0.3f)); 
        carnivoreLines.Add(line);
    }

    void AddPlantData()
    {
        GameObject line = ddDataDiagram.AddLine("plantPopulation", new Color(0.8f, 1.0f, 0.55f)); 
        plantLines.Add(line);

        Debug.Log("Line plants: " + line);


        line = ddDataDiagram.AddLine("plantWaterUsage", new Color(0.2f, 0.6f, 1.0f)); 
        plantLines.Add(line);


        line = ddDataDiagram.AddLine("plantRootRadius", new Color(0.4f, 0.2f, 0.0f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantMaxBiomass", new Color(0.2f, 1.0f, 0.8f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantSeedEnergyCost", new Color(0.9f, 1.0f, 1.0f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantSeedSize", new Color(0.6f, 0.3f, 0.0f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantSeedDispersion", new Color(0.1f, 0.5f, 0.4f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantPollenDispersion", new Color(0.95f, 1.0f, 0.7f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantMaxAge", new Color(0.7f, 0.4f, 1.0f)); 
        plantLines.Add(line);

        line = ddDataDiagram.AddLine("plantMaxHealth", new Color(1.0f, 0.4f, 0.4f)); 
        plantLines.Add(line);

        Debug.Log(plantLines.Count);

        //InvokeRepeating("UpdateHerbivorePopulation", 0.1f, updateTime);
    }


    public void ProcessHerbivoreData()
    {
        List<GameObject> herbivores = animalController.GetComponent<AnimalController>().GetHerbivores();

        float populationCount = 0;

        float averageEnergyDrain = 0;

        float averageThirstDrain = 0;

        float averageHungerDrain = 0;

        float averageVisionRadius = 0;

        float averageSpeed = 0;

        float averageMaxAge = 0;

        float averageMaxHealth = 0;

        populationCount = herbivores.Count; //so doesn't get too unusable

        if(populationCount == 0){
            foreach(var line in lines){
                ddDataDiagram.InputPoint(line, new Vector2(1, 0));
            }           
        } else {

        float baseStatValue = animalController.GetComponent<AnimalController>().baseStatValue;

        foreach(var animal in herbivores){
            averageEnergyDrain += animal.GetComponent<DNAstats>().energyDrainMod;
            averageThirstDrain += animal.GetComponent<DNAstats>().thirstDrainMod;
            averageHungerDrain += animal.GetComponent<DNAstats>().hungerDrainMod;
            averageVisionRadius += animal.GetComponent<CreatureMover>().visionRadius;
            averageSpeed += animal.GetComponent<CreatureMover>().speed; //change to mod, because speed is affected by injury
            averageMaxAge += animal.GetComponent<DNAstats>().getMaxAge();
            averageMaxHealth += animal.GetComponent<Animal>().getMaxHealth();
        }

            averageEnergyDrain /= populationCount;
            averageThirstDrain /= populationCount;
            averageHungerDrain /= populationCount;
            averageVisionRadius /= populationCount;
            averageSpeed /= populationCount;
            averageMaxAge /= populationCount;
            averageMaxHealth /= populationCount;



            averageEnergyDrain = baseStatValue / averageEnergyDrain;
            averageThirstDrain = baseStatValue / averageThirstDrain;
            averageHungerDrain = baseStatValue / averageHungerDrain;
            averageSpeed *= 10.0f;


            ddDataDiagram.InputPoint(findLine("herbivorePopulation"), new Vector2(1, populationCount / 10f));
            ddDataDiagram.InputPoint(findLine("herbivoreEnergyDrain"), new Vector2(1, averageEnergyDrain));
            ddDataDiagram.InputPoint(findLine("herbivoreThirstDrain"), new Vector2(1, averageThirstDrain));
            ddDataDiagram.InputPoint(findLine("herbivoreHungerDrain"), new Vector2(1, averageHungerDrain));
            ddDataDiagram.InputPoint(findLine("herbivoreVision"), new Vector2(1, averageVisionRadius));
            ddDataDiagram.InputPoint(findLine("herbivoreSpeed"), new Vector2(1, averageSpeed));
            ddDataDiagram.InputPoint(findLine("herbivoreMaxAge"), new Vector2(1, averageMaxAge));
            ddDataDiagram.InputPoint(findLine("herbivoreMaxHealth"), new Vector2(1, averageMaxHealth));
        }
    }

    public void ProcessCarnivoreData()
    {
        List<GameObject> carnivores = animalController.GetComponent<AnimalController>().GetCarnivores();

        float populationCount = 0;

        float averageEnergyDrain = 0;

        float averageThirstDrain = 0;

        float averageHungerDrain = 0;

        float averageVisionRadius = 0;

        float averageSpeed = 0;

        float averageMaxAge = 0;

        float averageMaxHealth = 0;

        populationCount = carnivores.Count;

        if(populationCount == 0){
            foreach(var line in carnivoreLines){
                ddDataDiagram.InputPoint(line, new Vector2(1, 0));
            }           
        } else {

        float baseStatValue = animalController.GetComponent<AnimalController>().baseStatValue;

        foreach(var animal in carnivores){
            averageEnergyDrain += animal.GetComponent<DNAstats>().energyDrainMod;
            averageThirstDrain += animal.GetComponent<DNAstats>().thirstDrainMod;
            averageHungerDrain += animal.GetComponent<DNAstats>().hungerDrainMod;
            averageVisionRadius += animal.GetComponent<CreatureMover>().visionRadius;
            averageSpeed += animal.GetComponent<CreatureMover>().speed; //change to mod, because speed is affected by injury
            averageMaxAge += animal.GetComponent<DNAstats>().getMaxAge();
            averageMaxHealth += animal.GetComponent<Animal>().getMaxHealth();
        }

            averageEnergyDrain /= populationCount;
            averageThirstDrain /= populationCount;
            averageHungerDrain /= populationCount;
            averageVisionRadius /= populationCount;
            averageSpeed /= populationCount;
            averageMaxAge /= populationCount;
            averageMaxHealth /= populationCount;



            averageEnergyDrain = baseStatValue / averageEnergyDrain;
            averageThirstDrain = baseStatValue / averageThirstDrain;
            averageHungerDrain = baseStatValue / averageHungerDrain;
            averageSpeed *= 10.0f;


            //ddDataDiagram.InputPoint(findLine("carnivorePopulation"), new Vector2(1, populationCount));
            
            ddDataDiagram.InputPoint(findLine("carnivorePopulation"), new Vector2(1, populationCount / 10f));
            ddDataDiagram.InputPoint(findLine("carnivoreEnergyDrain"), new Vector2(1, averageEnergyDrain));
            ddDataDiagram.InputPoint(findLine("carnivoreThirstDrain"), new Vector2(1, averageThirstDrain));
            ddDataDiagram.InputPoint(findLine("carnivoreHungerDrain"), new Vector2(1, averageHungerDrain));
            ddDataDiagram.InputPoint(findLine("carnivoreVision"), new Vector2(1, averageVisionRadius));
            ddDataDiagram.InputPoint(findLine("carnivoreSpeed"), new Vector2(1, averageSpeed));
            ddDataDiagram.InputPoint(findLine("carnivoreMaxAge"), new Vector2(1, averageMaxAge));
            ddDataDiagram.InputPoint(findLine("carnivoreMaxHealth"), new Vector2(1, averageMaxHealth));
        }

    }


    public void ProcessPlantData()
    {
        List<GameObject> plants = plantController.GetPlants();

        float populationCount = 0;

        float averageWaterUsage = 0;
        float averageRootRadius = 0;
        float averageMaxBiomass = 0;
        float averageSeedEnergyCost = 0;
        float averageSeedSize = 0;
        float averageSeedDispersion = 0;
        float averagePollenDispersion = 0;
        float averageMaxAge = 0;
        float averageMaxHealth = 0;

        populationCount = plants.Count;

        if(populationCount == 0){
            foreach(var line in plantLines){
                ddDataDiagram.InputPoint(line, new Vector2(1, 0));
            }           
        } else {

            float baseStatValue = plantController.baseStatValue;

            foreach(var plant in plants){
                averageWaterUsage += plant.GetComponent<Plant>().waterUsage;
                averageRootRadius += plant.GetComponent<Plant>().rootRadius;
                averageMaxBiomass += plant.GetComponent<Plant>().maxBiomass;
                averageSeedDispersion += plant.GetComponent<Plant>().seedDispersionRadius;
                averagePollenDispersion += plant.GetComponent<Plant>().pollenDispersionRadius;
                averageSeedEnergyCost += plant.GetComponent<Plant>().seedEnergyCost;
                averageMaxAge += plant.GetComponent<PlantDNAstats>().GetMaxAge();
                averageSeedSize += plant.GetComponent<PlantDNAstats>().seedSizeMod;
                averageMaxHealth += plant.GetComponent<Plant>().GetMaxHealth();
            }

                averageWaterUsage /= populationCount;
                averageRootRadius /= populationCount;
                averageMaxBiomass /= populationCount;
                averageSeedEnergyCost /= populationCount;
                averageSeedDispersion /= populationCount;
                averagePollenDispersion /= populationCount;
                averageMaxAge /= populationCount;
                averageMaxHealth /= populationCount;
                averageSeedSize /= populationCount;

                averageRootRadius = averageRootRadius * 10f;
                averageWaterUsage = averageWaterUsage * 10f;
                averageSeedSize = averageSeedSize * 100f;

                //ddDataDiagram.InputPoint(findLine("plantWaterUsage"), new Vector2(counter/updateTime, averageWaterUsage));

            ddDataDiagram.InputPoint(findLine("plantPopulation"), new Vector2(1, populationCount / 10f));
            ddDataDiagram.InputPoint(findLine("plantWaterUsage"), new Vector2(1, averageWaterUsage));
            ddDataDiagram.InputPoint(findLine("plantRootRadius"), new Vector2(1, averageRootRadius));
            ddDataDiagram.InputPoint(findLine("plantMaxBiomass"), new Vector2(1, averageMaxBiomass));
            ddDataDiagram.InputPoint(findLine("plantSeedEnergyCost"), new Vector2(1, averageSeedEnergyCost));
            ddDataDiagram.InputPoint(findLine("plantSeedSize"), new Vector2(1, averageSeedSize));
            ddDataDiagram.InputPoint(findLine("plantSeedDispersion"), new Vector2(1,  averageSeedDispersion));
            ddDataDiagram.InputPoint(findLine("plantPollenDispersion"), new Vector2(1, averagePollenDispersion));
            ddDataDiagram.InputPoint(findLine("plantMaxAge"), new Vector2(1, averageMaxAge));
            ddDataDiagram.InputPoint(findLine("plantMaxHealth"), new Vector2(1, averageMaxHealth));

        }

    }

    public void StartGraph()
    {

        //InvokeRepeating("IncreaseCounter", updateTime, updateTime); //must be earlier than others, but with offset by one tick

        /*GameObject line = ddDataDiagram.AddLine("herbivorePopulation", new Color(0, 1f, 0)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivorePopulation", 0.1f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreVision", new Color(0, 0, 1f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreVisionRadius", 0.15f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreEnergyDrain"); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreEnergyDrain", 0.2f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreSpeed", new Color(0.33f, 0.50f, 1f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreSpeed", 0.25f, updateTime);


        line = ddDataDiagram.AddLine("herbivoreMaxHealth", new Color(1f, 0, 0)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreMaxHealth", 0.3f, updateTime);

        line = ddDataDiagram.AddLine("herbivoreThirstDrain", new Color(0, 1f, 1f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreThirstDrain", 0.4f, updateTime);

        
        line = ddDataDiagram.AddLine("herbivoreHungerDrain", new Color(1f, 0.5f, 0)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreHungerDrain", 0.35f, updateTime);


        line = ddDataDiagram.AddLine("herbivoreMaxAge", new Color(0.48f, 0.3f, 0.3f)); 
        lines.Add(line);
        //InvokeRepeating("UpdateHerbivoreMaxAge", 0.45f, updateTime);

        //UpdateGraph(); */
        AddPlantData();
        ProcessPlantData();
        AddHervbivoreData();
        ProcessHerbivoreData();
        AddCarnivoreData();
        ProcessCarnivoreData();

        timeBeforeUpdateGraph = updateTime;
        activated = true;
    }

    GameObject findLine(string name){
        foreach (var line in lines){
            if (line.name == name) {
                return line;
            }
        }
        foreach (var line in carnivoreLines){
            if (line.name == name) {
                return line;
            }
        }
        foreach (var line in plantLines){
            if (line.name == name) {
                return line;
            }
        }
        return null;
    }

    void IncreaseCounter(){
        counter += updateTime;
        //Debug.Log(counter);
    }


    void UpdateHerbivorePopulation(){
        int population = animalController.GetComponent<AnimalController>().GetPopulation();
//        Debug.Log(population);
        ddDataDiagram.InputPoint(findLine("herbivorePopulation"), new Vector2(counter/updateTime, population));
       // counter += updateTime;
    }


    void UpdateHerbivoreEnergyDrain(){
        float energyDrain = animalController.GetComponent<AnimalController>().GetAverageEnergyDrain();
//        Debug.Log(energyDrain);
        ddDataDiagram.InputPoint(findLine("herbivoreEnergyDrain"), new Vector2(counter/updateTime, energyDrain));
       // counter += updateTime;
    }

    void UpdateHerbivoreThirstDrain(){
        float averageThirstDrain = animalController.GetComponent<AnimalController>().GetAverageThirstDrain();
        ddDataDiagram.InputPoint(findLine("herbivoreThirstDrain"), new Vector2(counter/updateTime, averageThirstDrain));
       // counter += updateTime;
    }

    void UpdateHerbivoreHungerDrain(){
        float averageHungerDrain = animalController.GetComponent<AnimalController>().GetAverageHungerDrain();
        ddDataDiagram.InputPoint(findLine("herbivoreHungerDrain"), new Vector2(counter/updateTime, averageHungerDrain));
       // counter += updateTime;
    }


    void UpdateHerbivoreVisionRadius(){
        float visionRadius = animalController.GetComponent<AnimalController>().GetAverageVisionRadius();
       // Debug.Log(visionRadius + " VR");
        ddDataDiagram.InputPoint(findLine("herbivoreVision"), new Vector2(counter/updateTime, visionRadius));
       // counter += updateTime;
    }

    void UpdateHerbivoreSpeed(){
        float averageSpeed = animalController.GetComponent<AnimalController>().GetAverageSpeed() * 10; //to make changes more visible
        ddDataDiagram.InputPoint(findLine("herbivoreSpeed"), new Vector2(counter/updateTime, averageSpeed));
       // counter += updateTime;
    }


    void UpdateHerbivoreMaxAge(){
        float averageMaxAge = animalController.GetComponent<AnimalController>().GetAverageMaxAge();
        ddDataDiagram.InputPoint(findLine("herbivoreMaxAge"), new Vector2(counter/updateTime, averageMaxAge));
       // counter += updateTime;
    }

    void UpdateHerbivoreMaxHealth(){
        float averageMaxHealth = animalController.GetComponent<AnimalController>().GetAverageMaxHealth();
        ddDataDiagram.InputPoint(findLine("herbivoreMaxHealth"), new Vector2(counter/updateTime, averageMaxHealth));
       // counter += updateTime;
    }

    // Update is called once per frame

}
