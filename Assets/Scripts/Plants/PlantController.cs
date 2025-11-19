using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public float baseStatValue = 100;

    public int initialPopulation = 5;
    public GameObject plantPrefab;
    
    public GameObject seedPrefab;

    public List<GameObject> population;

    public List<GameObject> seeds;

    public float mutationRate = 0.5f;

    public float maxMutationIntensity = 0.1f;

    public float percentOfIntenseMutations = 0.15f;

    public int counter = 0;

    public int maxPopulation = 3000;

    public EnvironmentController environmentController;
    public void Start()
    {

        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();
        
    }

    public void EndGame()
    {
        foreach(var plant in population)
        {
            if(plant != null)
            plant.GetComponent<Plant>().DestroyWithFruits();
        }
        foreach(var seed in seeds)
        {
            if(seed != null)
            seed.GetComponent<Seed>().DestroySeed();
        }
        population.Clear();
        updateQueue.Clear();
        seedsUpdateQueue.Clear();
        seeds.Clear();
    }


    public void FromNewGame()
    {
        //Vector2 pos;
        for(int i = 0; i < initialPopulation; i++){

            TryInstantiate();
        }
    }

    private void TryInstantiate()
    {
        Vector2 pos = UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0f, environmentController.worldBorder);
        for(int i = 0; i < 5; i++){
            //if(Physics2D.OverlapCircleAll(pos, 4f, LayerMask.GetMask("Plant", "Watersource")).Length == 0){

            if(Physics2D.OverlapCircleAll(pos, 4f, LayerMask.GetMask("Plant")).Length == 0 && //LayerMask.GetMask("Plant", "Watersource")
            Physics2D.OverlapCircleAll(pos, 2f, LayerMask.GetMask("Watersource")).Length == 0){
                GameObject first;
                first = Instantiate(plantPrefab, pos, Quaternion.identity);
                first.name = "FirstGenPlant";
                first.GetComponent<Plant>().age = first.GetComponent<PlantDNAstats>().GetMaxAge() / 50 + Random.Range(-first.GetComponent<PlantDNAstats>().GetMaxAge() / 75.0f, first.GetComponent<PlantDNAstats>().GetMaxAge() / 5.0f);
                first.GetComponent<Plant>().biomass = baseStatValue;
                //first.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);
                population.Add(first);
                updateQueue.Add(first);
                return;
            }
        }
    }

    float clearListsTimer = 15.0f;

    private float timer = 0f; //update timer

    private float seedUpdateTimer = 1f;

    private int groupsToSplitInto = 15;


    private int groupSize;

    private int partOfPopulation = 0;

    private const float conditionUpdateInterval = 0.5f;

    //======================

    private int seedGroupSize;

    private int partOfSeedAmount = 0;

    void Update() //TODO: update like in animal controller
    {
        if(population.Count > 0){

            clearListsTimer -= Time.deltaTime;
            seedUpdateTimer -= Time.deltaTime;
            timer += Time.deltaTime;

            if(clearListsTimer <= 0){ //this is pre-update
                clearListsTimer = 15.0f;
                ClearPopulation();
            }


            if (timer >= conditionUpdateInterval || partOfPopulation > 0)
            {

                groupSize = updateQueue.Count / groupsToSplitInto + 1;

                timer = 0f;
//                Debug.Log("UPDATING, GROUP SIZE" + groupSize);
                UpdatePlants();        
            }

            if(seedUpdateTimer <= 0 || partOfSeedAmount > 0){

                seedGroupSize = seedsUpdateQueue.Count / groupsToSplitInto + 1;
                seedUpdateTimer = 1f;

                UpdateSeeds();
            }
        }
    }

    public void ClearPopulation(){
        population.RemoveAll(s => s == null);
        updateQueue.RemoveAll(s => s == null);
        seeds.RemoveAll(s => s == null);
        seedsUpdateQueue.RemoveAll(s => s == null);
    }


    private List<GameObject> updateQueue = new List<GameObject>();
    private List<GameObject> seedsUpdateQueue = new List<GameObject>();
    void UpdateSeeds()
    {

        for(int i =  partOfSeedAmount * seedGroupSize; i < (partOfSeedAmount + 1) * seedGroupSize && i < seedsUpdateQueue.Count; i++)
        {
            //if(updateQueue[i] != null)
            //updateQueue[i].GetComponent<Animal>().CheckStatsAndNeedsNN();

            //here plants logic
            if(seedsUpdateQueue[i] != null) //don'r forget
            seedsUpdateQueue[i].GetComponent<Seed>().ReduceTimer();

        }

        if((partOfSeedAmount + 1) * seedGroupSize >= seedsUpdateQueue.Count)
        {
            partOfSeedAmount = 0;
        } else {
            partOfSeedAmount += 1;
        }

    }


    void UpdatePlants()
    {

        for(int i =  partOfPopulation * groupSize; i < (partOfPopulation + 1) * groupSize && i < updateQueue.Count; i++)
        {
            //if(updateQueue[i] != null)
            //updateQueue[i].GetComponent<Animal>().CheckStatsAndNeedsNN();

            //here plants logic
            if(updateQueue[i] != null) //don'r forget
            updateQueue[i].GetComponent<Plant>().checkStats();

        }

        if((partOfPopulation + 1) * groupSize >= updateQueue.Count)
        {
            partOfPopulation = 0;
        } else {
            partOfPopulation += 1;
        }

    }




    public List<GameObject> GetPlants()
    {
        population.RemoveAll(s => s == null);
        return population;
    }


    public void InstantiateSeed(Vector2 pos, string name, int gen, PlantDNAstats a, PlantDNAstats b){ //TODO
        if(population.Count >= maxPopulation)
            return;
            
        GameObject seed = Instantiate(seedPrefab, pos, Quaternion.identity);
        seed.name = counter + name;
        seed.GetComponent<Seed>().generation = gen; 

        //DNAstats creatureDNA = offspring.GetComponent<DNAstats>();
        seed.GetComponent<PlantDNAstats>().CreateOffspringTraits(a, b, mutationRate, maxMutationIntensity, percentOfIntenseMutations);

        seeds.Add(seed);
        seedsUpdateQueue.Add(seed);
    }


    public void InstantiateOffspringFromSeed(Vector2 pos, int gen, PlantDNAstats seedDNA){ //check for overlap here
        if(population.Count >= maxPopulation)
            return;

            
        GameObject offspring = Instantiate(plantPrefab, pos, Quaternion.identity);
        counter += 1;
        offspring.name = counter + "Plant" + gen;
        offspring.GetComponent<Plant>().generation = gen; 

        //DNAstats creatureDNA = offspring.GetComponent<DNAstats>();
        offspring.GetComponent<PlantDNAstats>().Copy(seedDNA);

        population.Add(offspring);
        updateQueue.Add(offspring);
    }

    public void InstantiateOffspring(Vector2 pos, string name, int gen, PlantDNAstats a, PlantDNAstats b){
        if(population.Count >= maxPopulation)
            return;
            
        GameObject offspring = Instantiate(plantPrefab, pos, Quaternion.identity);
        counter += 1;
        offspring.name = counter + name;
        offspring.GetComponent<Plant>().generation = gen; 

        //DNAstats creatureDNA = offspring.GetComponent<DNAstats>();
        offspring.GetComponent<PlantDNAstats>().CreateOffspringTraits(a, b, mutationRate, maxMutationIntensity, percentOfIntenseMutations);

        population.Add(offspring);
        updateQueue.Add(offspring);
    }



    public PlantControllerData ToData(){
        
        PlantControllerData data = new PlantControllerData();

        data.mutationRate = this.mutationRate;

        data.maxMutationIntensity = this.maxMutationIntensity;

        data.percentOfIntenseMutations = this.percentOfIntenseMutations;

        data.maxPopulation = this.maxPopulation;

        return data;

    }


    public void FromData(PlantControllerData data){

        this.mutationRate = data.mutationRate;

        this.maxMutationIntensity = data.maxMutationIntensity;

        this.percentOfIntenseMutations = data.percentOfIntenseMutations;

        this.maxPopulation = data.maxPopulation;

    }


    public void FromSave(PlantSaveData data){
        GameObject plant = Instantiate(plantPrefab, new Vector2(data.position_x, data.position_y), Quaternion.identity);;

        plant.name = data.objectName;

        plant.GetComponent<Plant>().FromData(data);

        plant.GetComponent<PlantDNAstats>().FromData(data.statsMod);

        population.Add(plant);
    }

    public List<GameObject> GetPopulationToSaves()
    {
        population.RemoveAll(s => s == null);
        return population;
    }
}
