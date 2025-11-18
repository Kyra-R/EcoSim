using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
   PlantDNAstats statsMod;

   public float timeBeforeSprout;

   public int generation;

   PlantController plantController;

   public LayerMask plantMask;

   //ADD TRIGGERS FOR GERMINATION

   void Awake()
   {
    statsMod = this.GetComponent<PlantDNAstats>();
    plantController = GameObject.Find("PlantController").GetComponent<PlantController>();
   }

   

    void Start()
    {
        timeBeforeSprout = statsMod.seedGerminationTime;
        InvokeRepeating("ReduceTimer", 1f, 1f);
    }

    void ReduceTimer(){ //do optimization for plants first
        timeBeforeSprout--;
        if(timeBeforeSprout <= 0){
            if(Physics2D.OverlapCircleAll(transform.position, 2f, plantMask).Length == 0)
            {
                plantController.InstantiateOffspringFromSeed(transform.position, generation, statsMod);
            }
            
            Destroy(this.gameObject);
        }
    }
}
