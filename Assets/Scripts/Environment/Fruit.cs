using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour //TODO: make amount of nutrition gotten by animal depend on its size
{
    // Start is called before the first frame update
    EnvironmentController environmentController; //TODO: just assign as public, find is harder
    void Awake(){
        environmentController = GameObject.Find("EnvironmentController").GetComponent<EnvironmentController>();
    }
    void OnTriggerEnter2D(Collider2D col){
        //Debug.Log("Fruit trigger " + col.name);
        if(IsInLayerMask(col.gameObject.layer, LayerMask.GetMask("Animal")) && col.gameObject.GetComponent<CreatureMover>().getState() == States.SEARCH_FOOD){
            //Debug.Log(col.transform.localScale.x);
            col.gameObject.GetComponent<Animal>().AlterHunger(environmentController.nutritionalValueOfFruits / col.transform.localScale.x , true);
            col.gameObject.GetComponent<Animal>().AlterThirst(environmentController.amountOfWaterInFruits / col.transform.localScale.x, true);
            Destroy(this.gameObject);
        }           

    }

    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;
    public static bool IsInLayerMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
