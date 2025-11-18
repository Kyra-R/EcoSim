using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalSaveData
{

    public string objectName;

    public NeatNNData neatNN;

    public DNAstatsData statsMod;

    public CreatureMoverData mover;

    //==============================survival

    public float energy;

    public float hunger;

    public float thirst;

    public float health;

    //==============================misc

    public float age;

    public int generation;

    public bool isHerbivore;

    //==============================reproduction

    public bool female;

    public int offspringCount;

    public float breedTimeout;

    public float reprodTimer;

    //private float interactionAvoidanceTimeout;

    public bool readyToBreed;

    //==============================movement
    public float position_x;

    public float position_y;

}