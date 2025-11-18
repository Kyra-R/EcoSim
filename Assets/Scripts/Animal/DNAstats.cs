using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAstats: MonoBehaviour //TODO: * is a bad one because it's hard to climb out of low and easy to snowball high. Perhaps +- is better, or modifier
{

    public float energyDrainMod = 2;

    public float hungerDrainMod = 1; 

    public float thirstDrainMod = 1.2f; 

    public float speedMod = 1; 

    public float accelerationMod = 0.1f;

    public float visionMod = 1; 
    public float healthMod = 1; 

    public float adultSizeMod = 1;

    public float birthSizeMod = 1;

    public float maxAge = 300; //Already done: later starts breeding the longer lives. 

    public float getMaxAge(){
        return maxAge;
    }

    float MixTraitExpression(float a, float b){
        if(Random.value <0.15f){
             return a;
        } else if (Random.value >= 0.85f) {
            return b;
        } else {
            return (a + b)/2;
        }
    }


    bool ExpressTraitStronger(){ // is stronger expression of trait?
        if(Random.value <0.5f){
             //return a * strongerExpressionMutationMod;
             return true;
        } else {
           // return a * weakerExpressionMutationMod;
           return false;
        }
    }


    float RandomizeExpressionStrength(float mRate, float mMaxIntensity, float percentOfmMax){
        float intensity = Random.value;
        float chanceOfStrongMutation = Random.value;
        if(intensity < mMaxIntensity){
            return intensity;
        } else if(chanceOfStrongMutation <= percentOfmMax){
            return mMaxIntensity;
        } else {
            return intensity * mMaxIntensity;
        }
    }

    public void CreateOffspringTraits(DNAstats a, DNAstats b, float mRate, float mMaxIntensity, float percentOfmMax){ //TODO: round floats in mutation results to to digits after .

        float strongerExpressionMutationMod;

        float weakerExpressionMutationMod;

        //mix and mutate ENERGY
        this.energyDrainMod = MixTraitExpression(a.energyDrainMod, b.energyDrainMod);
        this.hungerDrainMod = MixTraitExpression(a.hungerDrainMod, b.hungerDrainMod);
        this.thirstDrainMod = MixTraitExpression(a.thirstDrainMod, b.thirstDrainMod);

        this.speedMod = MixTraitExpression(a.speedMod, b.speedMod);
        this.accelerationMod = MixTraitExpression(a.accelerationMod, b.accelerationMod);
        this.visionMod = MixTraitExpression(a.visionMod, b.visionMod);

        this.maxAge = MixTraitExpression(a.maxAge, b.maxAge);
        this.healthMod = MixTraitExpression(a.healthMod, b.healthMod);

        this.birthSizeMod = MixTraitExpression(a.birthSizeMod, b.birthSizeMod);
        this.adultSizeMod = MixTraitExpression(a.adultSizeMod, b.adultSizeMod);

//        Debug.Log(1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax));


        if(Random.value < mRate){ 
 //           Debug.Log("Mutation in BIRTH SIZE trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.energyDrainMod *= strongerExpressionMutationMod; //quicker methabolic rate
                this.birthSizeMod *= strongerExpressionMutationMod;
            } else {
                this.energyDrainMod *= weakerExpressionMutationMod; //energy drains slower
                this.birthSizeMod *= weakerExpressionMutationMod; //thus muscles are more adapted to long-term slow movements rather than bursts
            }
        }


            if(Random.value < mRate){ 
 //           Debug.Log("Mutation in ADULT SIZE trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.adultSizeMod *= strongerExpressionMutationMod;
                //this.energyDrainMod *= strongerExpressionMutationMod;  //hunger and thirst slower due to slower metabolism, but more food is needed //energy for speed
                this.hungerDrainMod *= weakerExpressionMutationMod;
                this.thirstDrainMod *= weakerExpressionMutationMod;
                this.accelerationMod *= weakerExpressionMutationMod;
                this.maxAge *= strongerExpressionMutationMod;
            } else {
                this.adultSizeMod *= weakerExpressionMutationMod;
                //this.energyDrainMod *= weakerExpressionMutationMod; //energy drains slower
                this.hungerDrainMod *= strongerExpressionMutationMod;
                this.thirstDrainMod *= strongerExpressionMutationMod;
                this.accelerationMod *= strongerExpressionMutationMod; //thus muscles are more adapted to long-term slow movements rather than bursts
                this.maxAge *= weakerExpressionMutationMod;
            }
        }

        if(Random.value < mRate){ 
 //           Debug.Log("Mutation in ENERGY trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.energyDrainMod *= strongerExpressionMutationMod; //quicker methabolic rate
                this.speedMod *= strongerExpressionMutationMod;
            } else {
                this.energyDrainMod *= weakerExpressionMutationMod; //energy drains slower
                this.speedMod *= weakerExpressionMutationMod; //thus muscles are more adapted to long-term slow movements rather than bursts
            }
        }


        if(Random.value < mRate){ 
 //           Debug.Log("Mutation in ACCELERATION trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.accelerationMod *= strongerExpressionMutationMod; 
            } else {
                this.accelerationMod *= weakerExpressionMutationMod; 
            }
        }


        //mix and mutate HUNGER
       
        if(Random.value < mRate){ 
//            Debug.Log("Mutation in HUNGER trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.hungerDrainMod *= strongerExpressionMutationMod; //hunger drains quicker = + energy? or + speed since less fat storage and smaller stomach? quicker methabolic
                this.speedMod *= strongerExpressionMutationMod;
                this.energyDrainMod *= weakerExpressionMutationMod;
            } else {
                this.hungerDrainMod *= weakerExpressionMutationMod; 
                this.speedMod *= weakerExpressionMutationMod; 
                this.energyDrainMod *= strongerExpressionMutationMod;
            }
        }


        //mix and mutate THIRST
        
        if(Random.value < mRate){ 
//            Debug.Log("Mutation in THIRST trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.thirstDrainMod *= strongerExpressionMutationMod; //thirst drains quicker = + speed since less fat storage / quicker cooling via sweating
                this.speedMod *= strongerExpressionMutationMod;
            } else {
                this.thirstDrainMod *= weakerExpressionMutationMod; 
                this.speedMod *= weakerExpressionMutationMod; 
            }
        }



        //mix and mutate SPEED
       
        if(Random.value < mRate){ 
//            Debug.Log("Mutation in SPEED trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.speedMod *= strongerExpressionMutationMod;
                this.energyDrainMod *= strongerExpressionMutationMod;
                this.thirstDrainMod *= strongerExpressionMutationMod;
            } else {
                this.speedMod *= weakerExpressionMutationMod; 
                this.energyDrainMod *= weakerExpressionMutationMod;
                this.thirstDrainMod *= weakerExpressionMutationMod;
            }
        }


        //mix and mutate VISION
        //this.visionMod = MixTraitExpression(a.visionMod, b.visionMod);
        if(Random.value < mRate){ 
 //           Debug.Log("Mutation in VISION trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.visionMod *= strongerExpressionMutationMod;
                this.energyDrainMod *= strongerExpressionMutationMod;
            } else {
                this.visionMod *= weakerExpressionMutationMod; 
                this.energyDrainMod *= weakerExpressionMutationMod;
            }
        }


                //mix and mutate MaxAge
        
        if(Random.value < mRate){ //add health because longer lived have better resistance to cancer, diseases, etc
//            Debug.Log("Mutation in MAXAGE trait occured");

            strongerExpressionMutationMod = RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.maxAge += strongerExpressionMutationMod*100;
                this.healthMod *= (1f + strongerExpressionMutationMod);
            } else {
                this.maxAge -= weakerExpressionMutationMod*100;
                this.healthMod *= (1f - weakerExpressionMutationMod);
            }
        }

    }


    public DNAstatsData ToData(){
        DNAstatsData data = new DNAstatsData();

        //add ACCELERATION

        data.energyDrainMod = this.energyDrainMod;

        data.hungerDrainMod = this.hungerDrainMod; 

        data.thirstDrainMod = this.thirstDrainMod; 

        data.speedMod = this.speedMod; 


        data.visionMod = this.visionMod; 

        data.healthMod = this.healthMod; 

        data.maxAge = this.maxAge;
        
        return data; 
    }


    public void FromData(DNAstatsData statsData){

        this.energyDrainMod = statsData.energyDrainMod;

        this.hungerDrainMod = statsData.hungerDrainMod; 

        this.thirstDrainMod = statsData.thirstDrainMod; 

        this.speedMod = statsData.speedMod; 


        this.visionMod = statsData.visionMod; 

        this.healthMod = statsData.healthMod; 

        this.maxAge = statsData.maxAge;
        
    }


 
}
