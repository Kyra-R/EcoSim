using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantSaveData 
{
    public PlantDNAstatsData statsMod;

    public string objectName;

    public float health;

    public float biomass;

    public float energy;

    public float age;

    public int generation;

    public int offspringCount;

    public PlantStates state;

    public float position_x;

    public float position_y;
}
