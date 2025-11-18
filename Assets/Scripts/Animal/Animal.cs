using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    // Start is called before the first frame update
    private bool initialized = false;
    
    CreatureMover mover;

    DNAstats statsMod;

    //===============================speedOfLife
    float reduceStatsTime = 2f;

    float agingSpeed = 1f;

    //==============================survival
    private float baseStatValue;
    [Range(0.0f, 100.0f)]
    public float energy;// = baseStatValue;

    [Range(0.0f, 100.0f)]
    public float hunger;// = baseStatValue;
    [Range(0.0f, 100.0f)]
    public float thirst;// = = baseStatValue;

    public float health = 999f;// = = baseStatValue;

    float maxHealth;

    //==============================misc

    public float age = 0;

    float minScale;
    float maxScale = 1.0f;
    float currScale;

    //private int species = 0; //to DNAstats?

    public int generation = 0; //to DNAstats?

    public bool isHerbivore = true;

    //==============================reproduction

    public bool female = true; 

    public int offspringCount = 0;

    public float breedTimeout;

    private float interactionAvoidanceTimeout = 1.5f;

    public bool readyToBreed = false;

    public float reprodTimer = 0;

    public float adulthoodAge;

    //public bool adult;

    //==============================movement

    public float speed = 3f; //baseline values
    public float viewRadius = 8f;

    //==============================
    //public NN nn;

    public NeatNN nnNEAT;

    float [] inputsToNN;

    //==============================

    AnimalController animalController;
    
    void Awake(){

        //Debug.Log("Animal start" +gameObject.name);

        nnNEAT = gameObject.GetComponent<NeatNN>();
        mover = this.GetComponent<CreatureMover>();
        statsMod = this.GetComponent<DNAstats>();
        inputsToNN = new float[12];

        animalController = GameObject.Find("AnimalController").GetComponent<AnimalController>();

        baseStatValue = animalController.baseStatValue;

        energy = baseStatValue;
        hunger = baseStatValue * 0.45f;
        thirst = baseStatValue * 0.45f;
        //maxHealth = baseStatValue;
        //health = maxHealth;

        //float scale = Mathf.Lerp(minScale, maxScale, t);
    }


    void Start() //TODO: It seems that animals are evolving to be born at full size. Change amount of energy spent by female. But also make adulthood age depend on birthsize
    {
        //speed = speed * statsMod.speedMod;
        //mover.SetVisionAndSpeed(viewRadius * statsMod.visionMod, speed); //Set movement and vision to in mover

        maxHealth = baseStatValue * statsMod.healthMod;
        if(health == 999f)
        health = maxHealth;
        
        

        if(female){
            breedTimeout = statsMod.getMaxAge() / 33;
        } else {
            breedTimeout = statsMod.getMaxAge() / 100;
        }

        if(reprodTimer == 0){
            reprodTimer = breedTimeout;
        }


        


        InvokeRepeating("ReduceStats", reduceStatsTime, reduceStatsTime);
        InvokeRepeating("AgeUp", agingSpeed, agingSpeed);

        if(animalController.NeuralNetworkOn){
            //InvokeRepeating("CheckStatsAndNeedsNN", 0.5f, 0.5f);
        } else {
            InvokeRepeating("checkStats", 0.5f, 0.5f); //old pre-NN solution
        }

        if(isHerbivore){
            minScale = 0.7f * statsMod.birthSizeMod;
            adulthoodAge = statsMod.getMaxAge() * (0.7f + statsMod.adultSizeMod - minScale) / 15.0f; //herbivores are more precocial, so grow quicker
            
        } else {
            minScale = 0.6f * statsMod.birthSizeMod;
            adulthoodAge = statsMod.getMaxAge() * (0.6f + statsMod.adultSizeMod - minScale) / 12.0f; //the bigger difference between birth and adult, the longer growth
        }

        maxScale = 1.0f * statsMod.adultSizeMod; 

        if(minScale > maxScale){
            maxScale = minScale;
        }

        float scale = 1f;
        if(age < adulthoodAge){
            float t;

            t = Mathf.Clamp01(age / adulthoodAge);

            scale = Mathf.Lerp(minScale, maxScale, t);

//            Debug.Log(scale + " at beginning");

            transform.localScale = new Vector3(scale, scale, 1f);
        } else if(generation == 0){
            reprodTimer = Random.Range(0, breedTimeout);
            transform.localScale = new Vector3(maxScale, maxScale, 1f);
        } else {
            transform.localScale = new Vector3(maxScale, maxScale, 1f);
        }
//        Debug.Log("Scale:" + maxScale);
        speed = speed * statsMod.speedMod / (maxScale * maxScale);
        mover.SetVisionAndSpeed(viewRadius * statsMod.visionMod, speed); //Set movement and vision to in mover
        

         //herbivores are more precocial, so grow quicker
        initialized = true;    
    }


    public float GetBaseStatValue()
    {
        return baseStatValue;
    }

    bool checkIfHealthyToBreed(){
        if(female && energy > baseStatValue/5 && hunger > baseStatValue/5 && thirst > baseStatValue/5){
            return true;
        } else
        if(!female && energy > baseStatValue/20 && thirst > baseStatValue/20) {
            return true;
        } else {
            return false;
        }
    }


  /*  void OnCollisionEnter2D(Collision2D col) //pre-NN
    {

        if(col.gameObject.layer == 3) { //comment: walls   
            mover.setState(States.GO_CENTER);
        }
        if(IsInLayerMask(col.gameObject.layer, mover.getFoodMask()) && this.hunger < 90){ //TODO: to on collision stay?
            Destroy(col.gameObject);
            AlterHunger(33, true);
            AlterThirst(5, true);

            

            if(this.hunger >= 90 && mover.getState() == States.SEARCH_FOOD){
                mover.setState(States.WANDER);
            }

        } else
        if(IsInLayerMask(col.gameObject.layer, mover.getWaterMask()) && this.thirst < 90){
            AlterThirst(100, true);
            
      
           
            mover.setState(States.WANDER);

        } else if(checkIfHealthyToBreed() && mover.getTarget() != null && mover.getTarget().gameObject == col.gameObject && mover.getState() == States.SEARCH_MATE){ //TODO: if stay nearby, second oncollision won't work
            offspringCount += 1;
            if(female){
                AlterEnergy(baseStatValue/5, false);
                AlterHunger(baseStatValue/10, false);
                AlterThirst(baseStatValue/10, false);
                animalController.InstantiateOffspring
                    (this.transform.position, "NewCreature_" + offspringCount + "_gen" + (generation + 1), generation + 1,
                    this.gameObject.GetComponent<DNAstats>(), col.gameObject.GetComponent<DNAstats>(), 
                    this.gameObject.GetComponent<NeatNN>(), col.gameObject.GetComponent<NeatNN>()); //add network
            } else {
                AlterEnergy(baseStatValue/20, false);
                AlterThirst(baseStatValue/20, false);
            }
            //Debug.Log("Offspring on the way"); 


            readyToBreed = false;
            Invoke("ReproductionTimeout", breedTimeout);
            mover.setState(States.WANDER);
        } //TODO: reduce energy after mating
        
        
        else {
            
            mover.setState(States.WANDER);
        }

         
    } */


    float CalculateDamageMultiplier(float predatorSize, float preySize)
        {
            float x = predatorSize / preySize; // соотношение размеров
            float k = 5f; // чувствительность кривой
            float midpoint = 0.75f; // жертва получает половину урона здоровью при таком соотношении

            float multiplier = 1f / (1f + Mathf.Exp(-k * (x - midpoint)));
            return multiplier;
        }

    float CalculatePreyResistanceMultiplier(float predatorSize, float preySize)
        {
            float x = predatorSize / preySize; // соотношение размеров
            float k = 10f; 
            float midpoint = 0.7f; //хищник получает половину урона здоровью при таком соотношении

            float multiplier = 1f / (1f + Mathf.Exp(k * (x - midpoint)));
            return multiplier;
        }


     void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.layer == 3) { //comment: walls   
            mover.setState(States.GO_CENTER);
        }

        //FOR PREDATORS
        if(IsInLayerMask(col.gameObject.layer, mover.getFoodMask()) && (mover.getState() == States.HUNT)){ //TODO: to on collision stay? mover.getState() == States.SEARCH_FOOD ||
            
            AlterEnergy(baseStatValue / 5, false);

            Animal prey = col.gameObject.GetComponent<Animal>();

            float damage =  Mathf.Round(CalculateDamageMultiplier(transform.localScale.x , col.transform.localScale.x) * prey.maxHealth);
            //Debug.Log(damage + " damage dealt");
            
            if(damage >= prey.health){
                Destroy(col.gameObject); 
//                Debug.Log("Prey killed for nutrition: " + (col.transform.localScale.x / transform.localScale.x) * baseStatValue);
                AlterHunger((col.transform.localScale.x / transform.localScale.x) * baseStatValue, true); //nutritiousness depends on size
                AlterThirst(baseStatValue / 10, true);
            } else {
                col.gameObject.GetComponent<Animal>().AlterHealth(damage, false);

                float receivedDamage = Mathf.Round(CalculatePreyResistanceMultiplier(transform.localScale.x , col.transform.localScale.x) * maxHealth);
                if(health <= receivedDamage){
                    Destroy(this.gameObject);
                    Debug.Log(this.gameObject.name +" KILLED DURING A HUNT");
                } else {
                    AlterHealth(receivedDamage, false);
//                    Debug.Log(this.gameObject.name +", Prey resisted attack: " + receivedDamage + ", prey damaged for " + damage);
                }
                
//                Debug.Log(prey.maxHealth * damage); //TODO: make injured animal move slower
            }
        } else 
        if(IsInLayerMask(col.gameObject.layer, mover.getWaterMask()) && mover.getState() == States.SEARCH_WATER){
            AlterThirst(100, true);
        } else if(checkIfHealthyToBreed() && mover.getTarget() != null && mover.getTarget().gameObject == col.gameObject && mover.getState() == States.SEARCH_MATE){ //TODO: if stay nearby, second oncollision won't work
            //if(!isHerbivore)
//            Debug.Log("Offspring on the way");
            offspringCount += 1;
            if(female){
                float fetusSize = animalController.InstantiateOffspring
                    (this.transform.position, "NewCreature_" + isHerbivore + offspringCount + "_gen" + (generation + 1), generation + 1,
                    this.gameObject.GetComponent<DNAstats>(), col.gameObject.GetComponent<DNAstats>(), 
                    this.gameObject.GetComponent<NeatNN>(), col.gameObject.GetComponent<NeatNN>(), isHerbivore); //
//                Debug.Log(fetusSize + " size mod");

                AlterEnergy(baseStatValue/5 * fetusSize, false);
                AlterHunger(baseStatValue/10 * fetusSize, false);
                AlterThirst(baseStatValue/10 * fetusSize, false);
            } else {
                AlterEnergy(baseStatValue/20, false);
                AlterThirst(baseStatValue/20, false);
            }
            //Debug.Log("Offspring on the way"); 


            readyToBreed = false;
            Invoke("ReproductionTimeout", breedTimeout);
            //mover.setState(States.WANDER);
        } 
        
        
        else if(!(!isHerbivore && IsInLayerMask(col.gameObject.layer, mover.getFoodMask()))){         
            mover.setState(States.WANDER);
        }

         
    }


    public float getMaxHealth(){
        return maxHealth;
    }
    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;
    public static bool IsInLayerMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;


    void ReproductionTimeout(){
        readyToBreed = true;
    }


    
    

    bool calculatePriority(){ //comment: is priority of hunger over thirst
        if(thirst < hunger*1.1f){
            return false;
        } else {
            return true;
        }
    }


    States calculateStatePriority(){ //comment: new ONLY FOR HUNGER ENERGY THIRST
        //energy, thirst, hunger. Energy must grain slowest, then hunger, then water (1.1 - 1.2 - 1.3 coeff)
        // by percentage?
        // (maxHunger - hunger) - necessity level, then multiply by coeff of speed of draining, max necessity wins
        float energyPriorityCoef = (baseStatValue - energy) * statsMod.energyDrainMod; 
        float hungerPriorityCoef = (baseStatValue - hunger) * statsMod.hungerDrainMod; 
        float thirstPriorityCoef = (baseStatValue - thirst) * statsMod.thirstDrainMod; 

        //max of 3
        float maxCoef = Mathf.Max(energyPriorityCoef, Mathf.Max(hungerPriorityCoef, thirstPriorityCoef));

        if(maxCoef == thirstPriorityCoef){
            return States.SEARCH_WATER;
        } else
        if(maxCoef == hungerPriorityCoef){
            return States.SEARCH_FOOD;
        } else
        if(maxCoef == energyPriorityCoef){
            return States.REST;
        } else {
            return States.WANDER;
        }
    }


    bool checkNeeds(){
        if(hunger <= 50 || thirst <= 50 || energy <= 50){
            return true;
        } else {
            return false;
        }
    }

    void ReduceStats(){ 
        AlterHunger(statsMod.hungerDrainMod, false);
        AlterThirst(statsMod.thirstDrainMod, false);
        if(age < adulthoodAge){
            AlterHunger(statsMod.hungerDrainMod * 0.1f, false);
            AlterThirst(statsMod.thirstDrainMod * 0.1f, false);
        }
        if(mover.getState() == States.FLEE || mover.getState() == States.HUNT){
            AlterEnergy(statsMod.energyDrainMod * statsMod.accelerationMod / transform.localScale.x, false);
        } else
        if(mover.getState() != States.REST){
            AlterEnergy(statsMod.energyDrainMod / transform.localScale.x, false);
        }
//        Debug.Log(statsMod.energyDrainMod / transform.localScale.x);
    }

    

    void AgeUp(){
        this.age += 1;

        if(!readyToBreed && age >= adulthoodAge)
        reprodTimer -= 1;

        if(reprodTimer <= 0){
            reprodTimer = breedTimeout;
            readyToBreed = true;
        }

        if(age <= adulthoodAge){
            float t;
           // Debug.Log(age + " age");
            if(isHerbivore){
                t = Mathf.Clamp01(age / adulthoodAge);
            } else {
                t = Mathf.Clamp01(age / adulthoodAge);
            }
            float scale = Mathf.Lerp(minScale, maxScale, t);

            //Debug.Log(scale + " scale");

            transform.localScale = new Vector3(scale, scale, 1f);
        }

    }

    public void AlterHunger(float a, bool increase){
        if(increase){
            this.hunger += a;
        } else {
            this.hunger -= a;
        }
        if(this.hunger > 100){
            this.hunger = 100;
        }
        if(this.hunger < 0){
            this.hunger = 0;
        }
    }

    void AlterHealth(float a, bool increase){ //TODO: make speed depend on health
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

    public void AlterThirst(float a, bool increase){
        if(increase){
            this.thirst += a;
        } else {
            this.thirst -= a;
        }
        if(this.thirst > 100){
            this.thirst = 100;
        }
        if(this.thirst < 0){
            this.thirst = 0;
        }
    }


    void AlterEnergy(float a, bool increase){
        if(increase){
            this.energy += a;
        } else {
            this.energy -= a;
        }
        if(this.energy > 100){
            this.energy = 100;
        }
        if(this.energy < 0){
            this.energy = 0;
        }
    }

    bool checkIfDyingBaseNeeds(){
        if(hunger == 0 || thirst == 0){ //without energy? || energy == 0
            return true;
        } else {
            return false;
        }
    }

    void checkStats(){
        //move this section to reducestats or something
        AlterHealth(2.5f, true); //TODO: base regen. only if no needs? checkneeds

        if (age > statsMod.getMaxAge()){
            AlterHealth(10, false);
        }
        if(checkIfDyingBaseNeeds()){ //TODO: function which checks if thirst/hunger 0 and reduces health, this is health damage
            AlterHealth(5, false);
        }
        if(health <= 0){
            Destroy(this.gameObject);
        }


        if(mover.getState() == States.REST && energy < 90){
            AlterEnergy(10, true);
        } else
        if(mover.getState() != States.GO_CENTER){
            if(checkNeeds()){

                States desirableState = calculateStatePriority();
                if (desirableState != mover.getState()){
                    mover.setState(desirableState);
                }
                
            }  else if(readyToBreed && age > statsMod.getMaxAge() / 10 && mover.getState() != States.SEARCH_MATE){ //TODO: check average age at which capable of reproducing
                mover.setState(States.SEARCH_MATE);
            }
             else if (mover.getState() != States.WANDER && mover.getState() != States.SEARCH_MATE){ //TODO: better check states at other side?
                mover.setState(States.WANDER);
            }
        }

    }


    States IndexToState(int index){
        switch(index){
            case 0:
                return States.WANDER;
            case 1:
                return States.SEARCH_FOOD;
            case 2:
                return States.SEARCH_WATER;
            case 3:
                return States.SEARCH_MATE;
            case 4:
                return States.REST;
            case 5:
                if(isHerbivore){
                    return States.FLEE;
                } else {
                    return States.HUNT;
                }
        //        return States.SEARCH_FOOD;
            default:
                return States.WANDER;
        }
    }


    float ConvertReadinessIntoFloat(){
        if(readyToBreed){
            return 1.0f;
        } else {
            return -1.0f;
        }
    }

    public float GetPercentOfLostHealth(){
        return (maxHealth - health) / maxHealth;
    }


    int countSignals = 0;
    private float timer = 0f;
    private float bigTimer = 0f;
    int errors = 0;
    void Update() 
    {
        timer += Time.deltaTime;
        //bigTimer += Time.deltaTime;
        if(timer >= 2.05f){
            timer = 0;
            if(countSignals == 0){
//                Debug.Log("ZERO UPDATES " + gameObject.name);
                errors++;
            } //else
           /* if(countSignals < 2){
                Debug.Log("TOO FEW UPDATES" + gameObject.name + " " + countSignals);
                errors++;
            }*/
            countSignals = 0;
        }

    }

    public void CheckStatsAndNeedsNN(){//TODO: force them to rest
        if (!initialized) return;


        countSignals++;
//        Debug.Log(gameObject.name + " check");
        bool injured = false;

        if(health < maxHealth)
        {
            injured = true;
        }

        AlterHealth(4.5f, true); //TODO: base regen. only if no needs? checkneeds

        if (age > statsMod.getMaxAge()){
            AlterHealth(10, false);
        }
        if(checkIfDyingBaseNeeds()){ //TODO: function which checks if thirst/hunger 0 and reduces health, this is health damage
            AlterHealth(10, false);
        }
        if(energy <= 10 && mover.getState() != States.REST){ //TODO: add sleep deprived bool, otherwise first damages before energy is 0
            AlterHealth(4.5f, false);
        }
        if(health <= 0){
            Destroy(this.gameObject);
            return;
        }

       /* if(injured)
        {
            mover.SetSpeed(speed * statsMod.speedMod * (health / maxHealth));
        } */
        float newSpeed = speed; // * statsMod.speedMod;

        if(injured){
            newSpeed *= (health / maxHealth);
        }
        if(mover.getState() == States.FLEE || mover.getState() == States.HUNT){
            newSpeed *= statsMod.accelerationMod + 1.0f;
            //Debug.Log("Old:" + speed + " new:" + newSpeed);
        }

        mover.SetSpeed(newSpeed);

        /*if(injured || mover.getState() == States.FLEE || mover.getState() == States.HUNT){
            float newSpeed = 
            mover.SetSpeed(speed * statsMod.speedMod * (health / maxHealth) * );
        }*/

        if(mover.getState() == States.REST){
            AlterEnergy(10, true);
        }



        inputsToNN[0] = GetPercentOfLostHealth();
        inputsToNN[1] = (baseStatValue - hunger) / baseStatValue;
        inputsToNN[2] = (baseStatValue - thirst) / baseStatValue;
        inputsToNN[3] = (baseStatValue - energy) / baseStatValue;
        inputsToNN[4] = ConvertReadinessIntoFloat();
        inputsToNN[5] = mover.FindPriorityDistanceFoodNN();
        inputsToNN[6] = mover.FindPriorityDistanceWaterNN();
        inputsToNN[7] = mover.FindPopulationDensityNearClosestAnimalNN(); //population density at movement target?

        if(isHerbivore){
            inputsToNN[8] = mover.FindIfHuntingPredatorsInSightNN();
            //Debug.Log(mover.FindPriorityDistancePredatorNN());
            inputsToNN[9] = mover.FindPriorityDistancePredatorNN(); //population density at movement target?
        } else {
            //inputsToNN[8] = mover.FindIfHuntingTargetFoundNN();
            inputsToNN[8] = mover.GetPreyEvaluationNN();
//            Debug.Log(mover.GetPreyEvaluationNN() + " preyEval");
            inputsToNN[9] = mover.GetPreyHealth();
 //           Debug.Log(mover.GetPreyHealth() + " preyEvalHealth");
            inputsToNN[10] = mover.GetPreyDistance();
 //           Debug.Log(mover.GetPreyDistance() + " preyEvalDistance"); //add size evaluation
           // inputsToNN[12] = mover.getPreyState();

            inputsToNN[11] = mover.EvaluateSizeDifferenceNN();
        }

        

        //float [] outputsFromNN = nn.Brain(inputsToNN);

        float [] outputsFromNN = nnNEAT.FeedForward(inputsToNN);
        
        float? maxVal = null; //works even if negatives
        int index = -1;



//        Debug.Log(outputsFromNN.Length + "---------------------");
        for (int i = 0; i < 6; i++)
        {
          // Debug.Log(i + " " + outputsFromNN[i]);
            if (!maxVal.HasValue || outputsFromNN[i] > maxVal.Value)
                {
                    maxVal = outputsFromNN[i];
                    //Debug.Log("max " + maxVal + ", index " + i);
                    index = i;
                }
        }

        if(!isHerbivore){
            mover.SetPreyEvaluation(outputsFromNN[6]);
//            Debug.Log(outputsFromNN[6] + " prey value, search food " + outputsFromNN[1]);
        }

        
        

 //       if(index == 0){
//            Debug.Log("---------" + maxVal + "------" + IndexToState(index) + "------");
 //       }

        States resState = IndexToState(index);
        if(energy == 0 && resState != States.REST){ //passing out from exhaustion?
            mover.setState(States.REST);
        } else
        if (mover.getState() == States.REST && energy < 90){//TODO: remove when you figure out what to do with NN
            
        } else
        if(resState == States.SEARCH_MATE && !readyToBreed){
            //punish NN so it doesnt use wander and search the same?
            //but then, it wouldn't be able to breed anyway
            AlterEnergy(10, false); //let's say it represents efforts in searching and trying to impress

            mover.setState(States.WANDER);
        } else {
            mover.setState(resState);
        }
    }


    public AnimalSaveData ToData(){
        AnimalSaveData data = new AnimalSaveData();

        data.objectName = this.name;

//        Debug.Log(this.name);

        data.energy = this.energy;
        data.hunger = this.hunger;
        data.thirst = this.thirst;
        data.health = this.health;

        data.age = this.age;
        data.generation = this.generation;

        data.isHerbivore = this.isHerbivore;
        data.female = this.female;
        data.offspringCount = this.offspringCount;
        data.breedTimeout = this.breedTimeout;
        data.readyToBreed = this.readyToBreed;
        data.reprodTimer = this.reprodTimer;

        Vector3 pos = transform.position;
        data.position_x = pos.x;
        data.position_y = pos.y;

        return data;
    }

    public void FromData(AnimalSaveData data){
        this.energy = data.energy;
        this.hunger = data.hunger;
        this.thirst = data.thirst;
        this.health = data.health;
        //Debug.Log("Animal load" +gameObject.name);

        this.age = data.age;
        this.generation = data.generation;

        this.isHerbivore = data.isHerbivore;
        this.female = data.female;
        this.offspringCount = data.offspringCount;
        this.breedTimeout = data.breedTimeout;
        this.readyToBreed = data.readyToBreed;
        this.reprodTimer = data.reprodTimer;
    }
}
