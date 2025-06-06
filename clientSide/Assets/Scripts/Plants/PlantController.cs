using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public float baseStatValue = 100;

    public int initialPopulation = 5;
    public GameObject plantPrefab;

    public List<GameObject> population;

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
        population.Clear();
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
                return;
            }
        }
    }

    float timer = 15.0f;
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0){
            timer = 15.0f;
            ClearPopulation();
        }
    }

    public void ClearPopulation(){
        population.RemoveAll(s => s == null);
    }

    public List<GameObject> GetPlants()
    {
        population.RemoveAll(s => s == null);
        return population;
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
