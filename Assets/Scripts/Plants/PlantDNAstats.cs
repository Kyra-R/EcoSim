using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDNAstats : MonoBehaviour //TODO: this and AnimalDNAstats extensions of some general DNA class
{
    public float energyRecoveryMod = 2;

    public float waterDrainMod = 0.5f;

    public float rootRadiusMod = 1; //todo. In Plant affects how strongly damaged by waterdrain. The bigger the roots, the less damage. Smaller roots - more water drain

    public float biomassMod = 1;

    public float healthMod = 1;

    public float seedDispersionRadiusMod = 1;

    public float pollenDispersionRadiusMod = 1;

    public float seedSizeMod = 1;

    public float maxAge = 500f;

    public float seedGerminationTime = 10f;


    void Start(){

    }

    public void Copy(PlantDNAstats orig)
    {
        this.energyRecoveryMod = orig.energyRecoveryMod;
        this.waterDrainMod = orig.waterDrainMod;
        this.rootRadiusMod = orig.rootRadiusMod;
        this.biomassMod = orig.biomassMod;
        this.healthMod = orig.healthMod;
        this.seedDispersionRadiusMod = orig.seedDispersionRadiusMod;
        this.pollenDispersionRadiusMod = orig.pollenDispersionRadiusMod;
        this.seedSizeMod = orig.seedSizeMod;
        this.maxAge = orig.maxAge;
        this.seedGerminationTime = orig.seedGerminationTime;
    }

    public float GetMaxAge(){
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


    public void CreateOffspringTraits(PlantDNAstats a, PlantDNAstats b, float mRate, float mMaxIntensity, float percentOfmMax){ //TODO: round floats in mutation results to to digits after .

        float strongerExpressionMutationMod;

        float weakerExpressionMutationMod;

        //mix and mutate traits
        this.energyRecoveryMod = MixTraitExpression(a.energyRecoveryMod, b.energyRecoveryMod);
        this.biomassMod = MixTraitExpression(a.biomassMod, b.biomassMod);
        this.waterDrainMod = MixTraitExpression(a.waterDrainMod, b.waterDrainMod);
        this.rootRadiusMod = MixTraitExpression(a.rootRadiusMod, b.rootRadiusMod);

        this.seedDispersionRadiusMod = MixTraitExpression(a.seedDispersionRadiusMod, b.seedDispersionRadiusMod);
        this.pollenDispersionRadiusMod = MixTraitExpression(a.pollenDispersionRadiusMod, b.pollenDispersionRadiusMod);
        this.seedSizeMod = MixTraitExpression(a.seedSizeMod, b.seedSizeMod);
        
        this.maxAge = MixTraitExpression(a.maxAge, b.maxAge);
        this.healthMod = MixTraitExpression(a.healthMod, b.healthMod);

        this.seedGerminationTime = MixTraitExpression(a.seedGerminationTime, b.seedGerminationTime);

        if(Random.value < mRate){ 
           // Debug.Log("Mutation in PLANT ENERGY trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.energyRecoveryMod *= weakerExpressionMutationMod; //faster metabolism = energy drains quicker
                this.biomassMod *= strongerExpressionMutationMod;
            } else {
                this.energyRecoveryMod *= strongerExpressionMutationMod; //energy drains slower
                this.biomassMod *= weakerExpressionMutationMod; //
            }
        }


        if(Random.value < mRate){ 
           // Debug.Log("Mutation in ROOT RADIUS  trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.rootRadiusMod *= strongerExpressionMutationMod; //
                this.waterDrainMod *= weakerExpressionMutationMod;
            } else {
                this.rootRadiusMod *= weakerExpressionMutationMod; //
                this.waterDrainMod *= strongerExpressionMutationMod;
            }
        }



        //mix and mutate WATER
        
        if(Random.value < mRate){ 
           //Debug.Log("Mutation in PLANT WATER trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.waterDrainMod *= strongerExpressionMutationMod; //accumulates more energy by sintisizing biomass
                this.biomassMod *= strongerExpressionMutationMod;
            } else {
                this.waterDrainMod *= weakerExpressionMutationMod; 
                this.biomassMod *= weakerExpressionMutationMod; 
            }
        }



        if(Random.value < mRate){ 
          //  Debug.Log("Mutation in PLANT SEED traits occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.seedDispersionRadiusMod *= strongerExpressionMutationMod; //smaller seed fly farther, but have lower survival
                this.pollenDispersionRadiusMod *= weakerExpressionMutationMod;
                this.seedSizeMod *= weakerExpressionMutationMod;
            } else {
                this.seedDispersionRadiusMod *= weakerExpressionMutationMod; 
                this.pollenDispersionRadiusMod *= strongerExpressionMutationMod; 
                this.seedSizeMod *= strongerExpressionMutationMod; 
            }
        }



                //mix and mutate MaxAge

        if(Random.value < mRate){ //add health because longer lived have better resistance to cancer, diseases, etc
         //   Debug.Log("Mutation in MAXAGE trait occured");

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


         if(Random.value < mRate){ 
//           Debug.Log("Mutation in PLANT GERMINATION trait occured");

            strongerExpressionMutationMod = 1f + RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);
            weakerExpressionMutationMod = 1f - RandomizeExpressionStrength(mRate, mMaxIntensity, percentOfmMax);

            if(ExpressTraitStronger()){
                this.seedGerminationTime *= strongerExpressionMutationMod; //faster metabolism = energy drains quicker
                this.seedSizeMod *= strongerExpressionMutationMod;
            } else {
                this.seedGerminationTime *= weakerExpressionMutationMod; //faster metabolism = energy drains quicker
                this.seedSizeMod *= weakerExpressionMutationMod;
            }
        }

    }

    public PlantDNAstatsData ToData()
    {
        PlantDNAstatsData data = new PlantDNAstatsData();

        data.energyRecoveryMod = this.energyRecoveryMod;
        data.waterDrainMod = this.waterDrainMod;
        data.rootRadiusMod = this.rootRadiusMod;
        data.biomassMod = this.biomassMod;
        data.healthMod = this.healthMod;
        data.seedDispersionRadiusMod = this.seedDispersionRadiusMod;
        data.pollenDispersionRadiusMod = this.pollenDispersionRadiusMod;
        data.seedSizeMod = this.seedSizeMod;
        data.maxAge = this.maxAge;

        return data;
    }


    public void FromData(PlantDNAstatsData data)
    {
        this.energyRecoveryMod = data.energyRecoveryMod;
        this.waterDrainMod = data.waterDrainMod;
        this.rootRadiusMod = data.rootRadiusMod;
        this.biomassMod = data.biomassMod;
        this.healthMod = data.healthMod;
        this.seedDispersionRadiusMod = data.seedDispersionRadiusMod;
        this.pollenDispersionRadiusMod = data.pollenDispersionRadiusMod;
        this.seedSizeMod = data.seedSizeMod;
        this.maxAge = data.maxAge;
    }
}
