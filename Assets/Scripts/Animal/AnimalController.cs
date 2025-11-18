using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    // Start is called before the first frame update
    public float baseStatValue = 100;

    public int initialPopulationPerSex = 5;

    public int initialCarnivorePopulationPerSex = 1;
    public GameObject creaturePrefab;

    public GameObject carnivoreCreaturePrefab;

    public List<GameObject> population = new List<GameObject>();
    public List<GameObject> carnivorePopulation = new List<GameObject>();

    //========================================

    public float mutationRate = 0.5f;

    public float maxMutationIntensity = 0.1f;

    public float percentOfIntenseMutations = 0.15f;

    //========================================

    public float neuronMutationRate = 0.5f;

    public float connectionMutationRate = 0.1f;

    public float biasAndWeightsMutationRate = 0.15f;

    //========================================

    public int maxPopulation = 3000;

    //==========================================

    public int counter = 0;

    public bool NeuralNetworkOn = false;

    public InnovationTracker globalInnovationTracker;

    public EnvironmentController environmentController;

    //==========================================

    public NeatNN lastBrain;
    void Start()
    {
        globalInnovationTracker = GameObject.Find("InnovationTracker").GetComponent<InnovationTracker>();
        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();
        
    }

    public List<GameObject> GetHerbivores()
    {
        population.RemoveAll(s => s == null);

        //currentIndexHerbivores = 0;

        return population;
    }


    public List<GameObject> GetCarnivores()
    {
        carnivorePopulation.RemoveAll(s => s == null);

        //currentIndexCarnivores = 0;

        return carnivorePopulation;
    }

    public void EndGame()
    {
        foreach(var animal in population)
        {
            if(animal != null)
            Destroy(animal);
        }
        foreach(var animal in carnivorePopulation)
        {
            if(animal != null)           
            Destroy(animal);
        }

        population.Clear();
        carnivorePopulation.Clear();

        updateQueue.Clear();

    }



    public void FromNewGame(){

        GameObject basicAnimal = new GameObject("BaseObject");

        GameObject modifiedAnimal = new GameObject("modifiedObject");
        
        basicAnimal.AddComponent<NeatNN>();
        modifiedAnimal.AddComponent<NeatNN>();

        Debug.Log("LALALAL");

        NeatNN basicNN = basicAnimal.GetComponent<NeatNN>();

        NeatNN modifiedNN = modifiedAnimal.GetComponent<NeatNN>();


        basicNN.InitializeBaseStructure(globalInnovationTracker);

        modifiedNN.Clone(basicNN);
        Debug.Log("Cloning//////////////////////////");
        modifiedNN.ModifyBaseStructureHerbivore(globalInnovationTracker);

        GameObject first;
        GameObject second;

        for(int i = 0; i < initialPopulationPerSex; i++){
        
            first = Instantiate(creaturePrefab, UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0f, environmentController.worldBorder), Quaternion.identity);
            first.name = "FirstGenFemale" + i;
            first.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);
            first.GetComponent<NeatNN>().Clone(modifiedNN);
            first.GetComponent<Animal>().age = first.GetComponent<DNAstats>().getMaxAge() / 5.0f + Random.Range(-first.GetComponent<DNAstats>().getMaxAge() / 6.0f, first.GetComponent<DNAstats>().getMaxAge() / 5.0f);
            population.Add(first);
            updateQueue.Add(first);

            second = Instantiate(creaturePrefab, UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0f, environmentController.worldBorder), Quaternion.identity);
            second.name = "FirstGenMale" + i;
            second.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1f);
            second.GetComponent<Animal>().female = false;
            second.GetComponent<NeatNN>().Clone(modifiedNN);
            second.GetComponent<Animal>().age = second.GetComponent<DNAstats>().getMaxAge() / 5.0f + Random.Range(-first.GetComponent<DNAstats>().getMaxAge() / 6.0f, first.GetComponent<DNAstats>().getMaxAge() / 5.0f);;
            population.Add(second);
            updateQueue.Add(second);
        }

        modifiedNN.Clone(basicNN);
        modifiedNN.ModifyBaseStructureCarnivore(globalInnovationTracker);

        for(int i = 0; i < initialCarnivorePopulationPerSex; i++){
        
            first = Instantiate(carnivoreCreaturePrefab, UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0f, environmentController.worldBorder), Quaternion.identity);
            first.name = "CarnivoreFirstGenFemale" + i;
            first.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
            first.GetComponent<NeatNN>().Clone(modifiedNN);
            first.GetComponent<Animal>().age = first.GetComponent<DNAstats>().getMaxAge() / 5.0f + Random.Range(-first.GetComponent<DNAstats>().getMaxAge() / 6.0f, first.GetComponent<DNAstats>().getMaxAge() / 5.0f);
            carnivorePopulation.Add(first); 
            updateQueue.Add(first);

            second = Instantiate(carnivoreCreaturePrefab, UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0f, environmentController.worldBorder), Quaternion.identity);
            second.name = "CarnivoreFirstGenMale" + i;
            second.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
            second.GetComponent<Animal>().female = false;
            second.GetComponent<NeatNN>().Clone(modifiedNN);
            second.GetComponent<Animal>().age = second.GetComponent<DNAstats>().getMaxAge() / 5.0f + Random.Range(-first.GetComponent<DNAstats>().getMaxAge() / 6.0f, first.GetComponent<DNAstats>().getMaxAge() / 5.0f);
            carnivorePopulation.Add(second);
            updateQueue.Add(second);
        } 

        lastBrain = modifiedNN;
    }

    private float clearListsTimer = 0f;

    private float timer = 0f;
    private float behaviorUpdateInterval = 0.5f;

    private float moverTimer = 0f;
    private float moverUpdateInterval = 0.25f;
   // private int animalsPerUpdate = 200;
    private int groupsToSplitInto = 15;
    private int moverGroupsToSplitInto = 5;

    private int groupSize;

    private int moverGroupSize;

    private int partOfPopulation = 0;

    private int moverPartOfPopulation = 0;

    private List<GameObject> updateQueue = new List<GameObject>();
    void Update()
    {
        if(population.Count > 0 || carnivorePopulation.Count > 0){
            timer += Time.deltaTime;
            moverTimer += Time.deltaTime;
            clearListsTimer += Time.deltaTime;

             if(clearListsTimer >= 10f){
//                Debug.Log("Clear!");
                CleanPopulations();
                clearListsTimer = 0f;
             }

            if (timer >= behaviorUpdateInterval || partOfPopulation > 0)
                {

                    groupSize = updateQueue.Count / groupsToSplitInto + 1;

                    timer = 0f;
                    UpdateAnimals();        
                }


            if (moverTimer >= moverUpdateInterval || moverPartOfPopulation > 0)
                {
                    
                     if (moverTimer >= moverUpdateInterval)
                    moverTimer = 0f;

                    moverGroupSize = updateQueue.Count / moverGroupsToSplitInto + 1;
                    UpdateAnimalMovers();        
                }
        }
    }

 /*   void UpdateAnimals()
    {
        //int countA = 0;
        //int countB = 0;
        foreach(var animal in population)
        {
            if(animal != null)
            animal.GetComponent<Animal>().CheckStatsAndNeedsNN();
        }
        foreach(var animal in carnivorePopulation)
        {
            if(animal != null)
            animal.GetComponent<Animal>().CheckStatsAndNeedsNN();
        }
    } */ //old


    void UpdateAnimals()
    {

        for(int i =  partOfPopulation * groupSize; i < (partOfPopulation + 1) * groupSize && i < updateQueue.Count; i++)
        {
            if(updateQueue[i] != null)
            updateQueue[i].GetComponent<Animal>().CheckStatsAndNeedsNN();
        }

        if((partOfPopulation + 1) * groupSize >= updateQueue.Count)
        {
            partOfPopulation = 0;
        } else {
            partOfPopulation += 1;
        }

    }


    void UpdateAnimalMovers()
    {
        // Debug.Log("Update!-----2");
        for(int i =  moverPartOfPopulation * moverGroupSize; i < (moverPartOfPopulation + 1) * moverGroupSize && i < updateQueue.Count; i++)
        {
            if(updateQueue[i] != null)
            updateQueue[i].GetComponent<CreatureMover>().LookAround();
        }

        if((moverPartOfPopulation + 1) * moverGroupSize >= updateQueue.Count)
        {
            moverPartOfPopulation = 0;
        } else {
            moverPartOfPopulation += 1;
        }
    }



    bool RandomizeSex(){
        if(Random.value < 0.5f){
            return true;
        } else {
            return false;
        }
            
    }

    public float InstantiateOffspring(Vector2 pos, string name, int gen, DNAstats a, DNAstats b, NeatNN nn_a, NeatNN nn_b, bool isHerbivore){

        if(population.Count + carnivorePopulation.Count > maxPopulation){ //TODO: remove later or add setting
            population.RemoveAll(s => s == null);
            carnivorePopulation.RemoveAll(s => s == null);
            return 1;
        }

        GameObject offspring = null;
        float fetusSize = 0;
        if(isHerbivore){
            offspring = Instantiate(creaturePrefab, pos, Quaternion.identity);
            counter += 1;
            offspring.name = counter + name;
            offspring.GetComponent<Animal>().generation = gen; 
            if(RandomizeSex()){
                offspring.GetComponent<Animal>().female = false;
            }

            offspring.GetComponent<NeatNN>().CreateOffspringNetwork(nn_a, nn_b, neuronMutationRate, connectionMutationRate, biasAndWeightsMutationRate);

                        // Set the colour of the creature
            //DNA creatureColorDNA = offspring.GetComponent<DNA>(); 
            if(offspring.GetComponent<Animal>().female){
                offspring.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);

            } else {
                offspring.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1f);
            }

            //DNAstats creatureDNA = offspring.GetComponent<DNAstats>();
            DNAstats newCreatureDNA = offspring.GetComponent<DNAstats>();
            newCreatureDNA.CreateOffspringTraits(a, b,  mutationRate, maxMutationIntensity, percentOfIntenseMutations);
            fetusSize = newCreatureDNA.birthSizeMod;
    
            //offspring.GetComponent<SpriteRenderer>().color = new Color(creatureColorDNA.r, creatureColorDNA.g, creatureColorDNA.b);
            population.Add(offspring);
            updateQueue.Add(offspring);
        } else {
            offspring = Instantiate(carnivoreCreaturePrefab, pos, Quaternion.identity);
            counter += 1;
            offspring.name = counter + name;
            offspring.GetComponent<Animal>().isHerbivore = false;
            offspring.GetComponent<Animal>().generation = gen; 
            if(RandomizeSex()){
                offspring.GetComponent<Animal>().female = false;
            }

            offspring.GetComponent<NeatNN>().CreateOffspringNetwork(nn_a, nn_b, neuronMutationRate, connectionMutationRate, biasAndWeightsMutationRate);

                        // Set the colour of the creature
            DNA creatureColorDNA = offspring.GetComponent<DNA>(); 
            if(offspring.GetComponent<Animal>().female){
                offspring.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);

            } else {
                offspring.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
            }

            DNAstats newCreatureDNA = offspring.GetComponent<DNAstats>();
            newCreatureDNA.CreateOffspringTraits(a, b,  mutationRate, maxMutationIntensity, percentOfIntenseMutations);
            fetusSize = newCreatureDNA.birthSizeMod;
    
            //offspring.GetComponent<SpriteRenderer>().color = new Color(creatureColorDNA.r, creatureColorDNA.g, creatureColorDNA.b);
            carnivorePopulation.Add(offspring);
            updateQueue.Add(offspring);
        }

        lastBrain = offspring.GetComponent<NeatNN>();
        return fetusSize;
    }

    public AnimalControllerData ToData(){

        AnimalControllerData data = new AnimalControllerData();

        data.mutationRate = this.mutationRate;

        data.maxMutationIntensity = this.maxMutationIntensity;

        data.percentOfIntenseMutations = this.percentOfIntenseMutations;

        data.neuronMutationRate = this.neuronMutationRate;

        data.connectionMutationRate = this.connectionMutationRate;
        
        data.biasAndWeightsMutationRate = this.biasAndWeightsMutationRate;

        data.maxPopulation = this.maxPopulation;

        return data;

    }


    public void FromData(AnimalControllerData data){

        this.mutationRate = data.mutationRate;

        this.maxMutationIntensity = data.maxMutationIntensity;

        this.percentOfIntenseMutations = data.percentOfIntenseMutations;

        this.neuronMutationRate = data.neuronMutationRate;

        this.connectionMutationRate = data.connectionMutationRate;
        
        this.biasAndWeightsMutationRate = data.biasAndWeightsMutationRate;

        this.maxPopulation = data.maxPopulation;

    }

    public void FromSave(AnimalSaveData data){
        GameObject animal;
        if(data.isHerbivore){
            animal = Instantiate(creaturePrefab, new Vector2(data.position_x, data.position_y), Quaternion.identity);
            if(data.female){
                animal.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);
            } else {
                animal.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1f);
            }
        } else {
            animal = Instantiate(carnivoreCreaturePrefab, new Vector2(data.position_x, data.position_y), Quaternion.identity);
//            Debug.Log("PREDATOR");
            if(data.female){
                animal.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);

            } else {
                animal.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
            }
        }

        animal.name = data.objectName;

        animal.GetComponent<Animal>().FromData(data);
        
        animal.GetComponent<NeatNN>().FromData(data.neatNN);

        animal.GetComponent<DNAstats>().FromData(data.statsMod);

        animal.GetComponent<CreatureMover>().FromData(data.mover);

        if(animal.GetComponent<CreatureMover>().getState() == States.REST)
        {
            animal.GetComponent<EyeScript>().OpenCloseEye();
        }
            
        if(data.isHerbivore){
            population.Add(animal);
        } else {
            carnivorePopulation.Add(animal);
        }
        updateQueue.Add(animal);
    }


    public int GetPopulation(){
        population.RemoveAll(s => s == null);
        if(population.Count == 0){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPaused = true;
            #endif
        }
        return population.Count;
    }

    void CleanPopulations(){
        population.RemoveAll(s => s == null);
        carnivorePopulation.RemoveAll(s => s == null);

        updateQueue.RemoveAll(s => s == null);

        //currentIndexHerbivores = 0;
        //currentIndexCarnivores = 0;
    }

    public List<GameObject> GetPopulationToSave(){
        population.RemoveAll(s => s == null);

        //currentIndexHerbivores = 0;
        //currentIndexCarnivores = 0;

        return population;
    }

    public float GetAverageEnergyDrain(){ //TODO: get all 3 of those into 1 analysis of base stats? //FIX NANS
        float averageEnergyDrain = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageEnergyDrain += animal.GetComponent<DNAstats>().energyDrainMod;
        }
        averageEnergyDrain /= population.Count;
        averageEnergyDrain = baseStatValue / averageEnergyDrain;
        //Debug.Log(averageEnergyDrain);
        if(population.Count > 0){
            return averageEnergyDrain;
        } else {
            return 0;
        }
         //amount of seconds to fully drain
    }

    
    public float GetAverageThirstDrain(){
        float averageThirstDrain = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageThirstDrain += animal.GetComponent<DNAstats>().thirstDrainMod;
        }
        averageThirstDrain /= population.Count;
        averageThirstDrain = baseStatValue / averageThirstDrain;
        if(population.Count > 0){
            return averageThirstDrain;
        } else {
            return 0;
        } //amount of seconds to fully drain
    }


    public float GetAverageHungerDrain(){
        float averageHungerDrain = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageHungerDrain += animal.GetComponent<DNAstats>().hungerDrainMod;
        }
        averageHungerDrain /= population.Count;
        averageHungerDrain = baseStatValue / averageHungerDrain;
        if(population.Count > 0){
            return averageHungerDrain;
        } else {
            return 0;
        } //amount of seconds to fully drain
    }


    public float GetAverageVisionRadius(){ //
        float averageVisionRadius = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageVisionRadius += animal.GetComponent<CreatureMover>().visionRadius;
        }
        averageVisionRadius /= population.Count;
        if(population.Count > 0){
            return averageVisionRadius;
        } else {
            return 0;
        }
    }


    public float GetAverageSpeed(){ //
        float averageSpeed = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageSpeed += animal.GetComponent<CreatureMover>().speed;
        }
        averageSpeed /= population.Count;
        if(population.Count > 0){
            return averageSpeed;
        } else {
            return 0;
        }
    }


    
    public float GetAverageMaxAge(){ //add also age at death to compare, use demographic formula
        float averageMaxAge = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageMaxAge += animal.GetComponent<DNAstats>().getMaxAge();
        }
        averageMaxAge /= population.Count;
        if(population.Count > 0){
            return averageMaxAge;
        } else {
            return 0;
        }
    }

    public float GetAverageMaxHealth(){ //
        float averageMaxHealth = 0;
        population.RemoveAll(s => s == null);

        foreach(var animal in population){
            averageMaxHealth += animal.GetComponent<Animal>().getMaxHealth();
        }
        averageMaxHealth /= population.Count;
        if(population.Count > 0){
            return averageMaxHealth;
        } else {
            return 0;
        }
    }
}
