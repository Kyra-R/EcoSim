using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour 
{
    private bool initialized = false;

    PlantDNAstats statsMod;
    //===============================speedOfLife
    //float reduceStatsTime = 1f;

    float agingSpeed = 1f;

    //==============================survival
    public float health = 999f; //++++++

    float maxHealth;

    public float waterUsage;

    public float biomass = 0; //alternatively, size //++++++

    public float maxBiomass;

    public float energy = 0;  //++++++ = 0;

    float maxEnergy;

    public float rootRadius = 6f; //radius affects waterdrain //++++++

    //==============================misc

    public float age = 0;

    //private int species = 0; //to DNAstats?

    public int generation = 0; //to DNAstats?

    //==============================reproduction
    public float seedEnergyCost = 10f; //add dependence on size;

    public float seedDispersionRadius = 9f;

    public float pollenDispersionRadius = 12f;

    private int offspringCount = 0; //++++++

    private float breedTimeout;

   // private bool readyToBreed = true;

    //==============================fruits

    private float fruitGrowthTime = 5f; //amount of fuits in DNAstats

    private float fruitBiomassCost = 32;
    

    public GameObject fruitPrefab;

    public List<GameObject> fruits;


    PlantController plantController;

    EnvironmentController environmentController;

    public LayerMask plantMask;

    public LayerMask waterMask;

    public PlantStates state; //++++++

    float baseStatValue;

    void SetState(PlantStates newState){
        if(newState == state){
            return;
        }
        state = newState;
    }

    
    PlantStates GetState(){
        return state;
    }


    // Start is called before the first frame update


    //TODO: if in climatecontroller waterAmount = 0, reduce health, if all biomass is eaten - damage health
    //breed timeout (in reality, amount of seeds produced) should depend on the biomass left
    //biomass is represented by fruits. If full, then spawn all/ If some were eaten, spawn less fruits. fuits, let's say, biomass / 32;
    //biomass recovers at each tick if water and energy is enough
    // if energy is low, then recovering energy first, and only then biomass (?)
    void Awake()
    {
        plantController = GameObject.Find("PlantController").GetComponent<PlantController>();
        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();
        statsMod = this.GetComponent<PlantDNAstats>();

//        Debug.Log("biomass0");

        baseStatValue = plantController.baseStatValue;
        maxEnergy = baseStatValue;
        energy = maxEnergy;

        //health = 999f;
    }


    void Start()
    {

        rootRadius = rootRadius * statsMod.rootRadiusMod;

        
        waterUsage = statsMod.waterDrainMod;

        //maxEnergy = baseStatValue;
        //energy = maxEnergy;
        
        maxHealth = baseStatValue * statsMod.healthMod;
        if(health == 999f)
        health = maxHealth;

        maxBiomass = baseStatValue * statsMod.biomassMod;

        if(this.biomass == 0){
//            Debug.Log("biomass");
            biomass = fruitBiomassCost; 
        }


        seedEnergyCost = seedEnergyCost * statsMod.seedSizeMod; //smaller seeds easier to produce

        seedDispersionRadius = seedDispersionRadius * statsMod.seedDispersionRadiusMod;

        pollenDispersionRadius = pollenDispersionRadius * statsMod.pollenDispersionRadiusMod;

        waterUsage = statsMod.waterDrainMod;

        state = PlantStates.STATIC;

        //GrowFruits();

        InvokeRepeating("GrowFruits", 0f, fruitGrowthTime);
        InvokeRepeating("AgeUp", agingSpeed, agingSpeed);
        //InvokeRepeating("checkStats", 0.5f, 0.5f);

        initialized = true;
    }


    /*void ReduceStats(){ 
        AlterHunger(statsMod.hungerDrainMod, false);
        AlterThirst(statsMod.thirstDrainMod, false);
    }*/

    void AgeUp(){
        this.age += 1;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }


    void AlterHealth(float a, bool increase){
        if(increase){
            this.health += a;
        } else {
            this.health -= a;
        }
        if(this.health > maxHealth){
            this.health = maxHealth;
        }
        if(this.health < 0){
            this.health = 0;
        }
    }

    void AlterBiomass(float a, bool increase){
        if(increase){
            this.biomass += a;
        } else {
            this.biomass -= a;
        }
        if(this.biomass > maxBiomass){
            this.biomass = maxBiomass;
        }
        if(this.biomass < 0){
            this.biomass = 0;
        }
    }


    void AlterEnergy(float a, bool increase){
        //Debug.Log(maxEnergy);
        if(increase){
            this.energy += a;
            //Debug.Log(energy);
        } else {
            this.energy -= a;
        }
        if(this.energy > maxEnergy){
            this.energy = maxEnergy;
        }
        if(this.energy < 0){
            this.energy = 0;
        }
    }



    private void InitialiseFruits() 
    {
        int fruitAmount = (int)(this.biomass / fruitBiomassCost - fruits.Count);
        for (int i = 0; i < fruitAmount; i++)
        {
            // Choose a random position for the creature to appear
            Vector2 pos = new Vector2(transform.position.x + Random.Range(-1.0f, 1.0f),transform.position.y + Random.Range(-1.0f, 1.0f));
 
            // Instantiate a new creature
            GameObject fruit = Instantiate(fruitPrefab, pos, Quaternion.identity);
 
            // Add the creature to the population
            fruits.Add(fruit);
        }
        maxEnergy = (biomass / maxBiomass) * baseStatValue; 
        //Debug.Log("MaxEnergy " + maxEnergy);
    }

    private void GrowFruits(){
        int origFruitAmount = fruits.Count;
        fruits.RemoveAll(s => s == null);
//        Debug.Log("fruits " + (origFruitAmount - fruits.Count));
        AlterBiomass((origFruitAmount - fruits.Count) * fruitBiomassCost, false); //if some fruits were eaten, then biomass reduces
        InitialiseFruits();
    }

    public void DestroyWithFruits()
    {
        foreach(var fruit in fruits){
                Destroy(fruit);
            }
        Destroy(this.gameObject);
    }


    bool checkIfDyingBaseNeeds(){
        Collider2D[] targetsInVisionRadius;
        Collider2D[] waterInVisibleRadius;

        waterInVisibleRadius = Physics2D.OverlapCircleAll(transform.position, rootRadius, waterMask);
        if(waterInVisibleRadius.Length > 0){
            //Debug.Log("Water!");
            return false;
        }
        float areaWaterUsage = 0;
        targetsInVisionRadius = Physics2D.OverlapCircleAll(transform.position, rootRadius, plantMask);
         foreach(var plant in targetsInVisionRadius){
            areaWaterUsage += plant.gameObject.GetComponent<Plant>().waterUsage;
        }
        if(areaWaterUsage > environmentController.waterAmount){
            return true;
        }
        return false;
    }



    public void checkStats(){
        if (!initialized) return;

        countSignals++;
        float originalHealth = health;

        bool notEnoughWater = checkIfDyingBaseNeeds();
//        Debug.Log(notEnoughWater);
        float x;

        if(!notEnoughWater){
            AlterEnergy(statsMod.energyRecoveryMod, true);
            AlterBiomass(2, true);
            AlterHealth(3, true);
            //Debug.Log("Energy");
        } else {

            if(environmentController.waterAmount > 0){
                x = waterUsage / environmentController.waterAmount;
            } else {
                x = waterUsage / 0.001f;
            }
            

            //if(!((3*Mathf.Pow(x, 2.0f) / 
            //(Mathf.Pow(x, 2.0f) + x + 1)) / rootRadius * 2 >=0))
            //Debug.Log("NaN");

            AlterHealth((3*Mathf.Pow(x, 2.0f) / 
            (Mathf.Pow(x, 2.0f) + x + 1)) / rootRadius * 2, false);

            if(originalHealth == 999f)
            Debug.Log("Altering by: " +  ((3*Mathf.Pow(x, 2.0f) / 
            (Mathf.Pow(x, 2.0f) + x + 1)) / rootRadius * 2)  );
            //AlterHealth(2 * (waterUsage / environmentController.waterAmount), false); //those plants which adapted better will be damaged by lack of water less
           // Debug.Log("Water");
        }

         
        

        if(age > statsMod.GetMaxAge()){ 
            AlterHealth(10, false);
           // Debug.Log("Age");
        }
        if(fruits.Count == 0){
            AlterHealth(10f, false);
            //Debug.Log("Fruits");
        }
        if(health <= 0){
            foreach(var fruit in fruits){
                Destroy(fruit);
            }

            Debug.Log("DIED health " + originalHealth);
            Destroy(this.gameObject);
            return;
        }

        if(notEnoughWater || (energy < seedEnergyCost)){
            SetState(PlantStates.STATIC);
        } else if(age > statsMod.GetMaxAge() / 50 && energy > maxEnergy * 0.9f){
            SetState(PlantStates.PRODUCE_GAMETES);
        }

        if(state == PlantStates.PRODUCE_GAMETES){ //TODO: create checkstate method
            ProduceMaleGametes();
        }

    }

    void ProduceMaleGametes(){
        Collider2D[] targetsInVisionRadius;
        targetsInVisionRadius = Physics2D.OverlapCircleAll(transform.position, pollenDispersionRadius, plantMask);
    //    Debug.Log("Producing pollen " + targetsInVisionRadius.Length);
         for(int i=0;i<targetsInVisionRadius.Length;i++){
            if(targetsInVisionRadius[i].gameObject.GetComponent<Plant>().GetState() == PlantStates.PRODUCE_GAMETES){ //check yourself? Or keep as feature
                targetsInVisionRadius[i].gameObject.GetComponent<Plant>().ProduceSeeds(statsMod);
            }
         }
    }

    public void ProduceSeeds(PlantDNAstats pollen){
        if(energy > seedEnergyCost){
            AlterEnergy(seedEnergyCost, false);
//            Debug.Log("Reproduction-----------------");
            Vector2 seedPosition = UtilityAdditions.RandomPointInAnnulus(this.transform.position, rootRadius, seedDispersionRadius); 
            //instead of root radius, transform size height GetComponent<RectTransform>().sizeDelta.y
           // if(Mathf.Abs(seedPosition.x) < environmentController.worldBorder && 
           // Mathf.Abs(seedPosition.y) < environmentController.worldBorder &&
            if(Vector2.Distance(seedPosition, Vector2.zero) <= environmentController.worldBorder &&
            Physics2D.OverlapCircleAll(seedPosition, 4f, plantMask).Length == 0 && //LayerMask.GetMask("Plant", "Watersource")
            Physics2D.OverlapCircleAll(seedPosition, 2f, waterMask).Length == 0)
            { //also check if there is pond or another plant
                if(Random.value * statsMod.seedSizeMod > environmentController.soilHostility){
        //            Debug.Log("Seed survived!");


                    /*plantController.InstantiateOffspring(seedPosition, "|---NewPlant_" + offspringCount + "_gen" + (generation + 1), generation + 1, //OLD
                        this.gameObject.GetComponent<PlantDNAstats>(), pollen);
                        offspringCount++;
                        pollen.gameObject.GetComponent<Plant>().offspringCount++;*/


                        plantController.InstantiateSeed(seedPosition, "|---NewSeed_" + offspringCount + "_gen" + (generation + 1), generation + 1, //NEW
                        this.gameObject.GetComponent<PlantDNAstats>(), pollen);
                        offspringCount++;
                        pollen.gameObject.GetComponent<Plant>().offspringCount++;



                } else {
            //        Debug.Log("Seed failed!");
                }
            }


        } else {
            SetState(PlantStates.STATIC);
        }

        
    }

    // Update is called once per frame
    int countSignals = 0;

    float signalTimer = 0f;

    int errors = 0;
    void Update()
    {
        
        signalTimer += Time.deltaTime;
        //bigTimer += Time.deltaTime;
        if(signalTimer >= 2.05f){
            signalTimer = 0;
            if(countSignals == 0){
                Debug.Log("ZERO UPDATES " + gameObject.name);
                errors++;
            } //else
           /* if(countSignals < 2){
                Debug.Log("TOO FEW UPDATES" + gameObject.name + " " + countSignals);
                errors++;
            }*/
            countSignals = 0;
        }
        
    }

    public PlantSaveData ToData(){
        PlantSaveData data = new PlantSaveData();

        data.objectName = this.name;
//        Debug.Log(this.name);

        data.energy = this.energy;
        data.biomass = this.biomass;
        data.health = this.health;
        data.age = this.age;
        data.generation = this.generation;
        data.offspringCount = this.offspringCount;

        data.state = this.state;

        Vector2 pos = transform.position;
        data.position_x = pos.x;
        data.position_y = pos.y;

        return data;
    }



    public void FromData(PlantSaveData data){
        this.energy = data.energy;
        this.biomass = data.biomass;
        InitialiseFruits();
        
//        Debug.Log(this.biomass + " biomass2");
        this.health = data.health;
        this.age = data.age;
        this.generation = data.generation;
        this.offspringCount = data.offspringCount;

        this.state = data.state;

        //GrowFruits();
    }

}
