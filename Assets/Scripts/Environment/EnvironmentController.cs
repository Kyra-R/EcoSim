using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour //TODO: add randomize watersouce
{
    // Start is called before the first frame update
    //public float temperature = 0; //TODO, ignore for now

    public float waterEvaporCoeff = 0.1f; //how much water amount evaporates each climate update time

    public float waterAmount = 1f;//how much water in the soil? 1f - trees have all they need, 0f - trees have nothing

    public float rainFrequency = 0.5f; //percentage of rain during every climate update

    public float rainStrength = 0.6f; //how much of water amount will be restored each rain

    public float soilHostility = 0.3f; //percentage of seeds that will not germinate

    public float climateUpdateTime = 10f;

    public float worldBorder = 20f; 

    public float nutritionalValueOfFruits = 33f;

    public float amountOfWaterInFruits = 3f;

    public List<GameObject> watersources;

    public GameObject pondPrefab;

    public GameObject puddlePrefab;

    public GameObject waterBar;

    public GameObject weatherPicture;

    GameObject field;

    public GameObject camera;

    public int amountOfWatersources = 0;

    bool simulationActive = false;
    

    void Awake()
    {
        field = GameObject.Find("Square");
        Debug.Log(worldBorder);
    }

    public void ChangeWorldBorder(){
        field.transform.localScale = new Vector3(0, 0, 1);
        if(worldBorder * 2.0f > field.transform.localScale.x){
            Debug.Log("yay");
            field.transform.localScale += new Vector3(worldBorder * 2.05f, worldBorder * 2.05f, 0);
        } else if(worldBorder * 2.05f < field.transform.localScale.x){
            Debug.Log("nay");
            field.transform.localScale -= new Vector3(worldBorder * 2.05f, worldBorder * 2.05f, 0);
        }

        //GameObject.Find("Main Camera").GetComponent<CameraController2D>().UpdateMaxZoom();
        camera.GetComponent<CameraController2D>().UpdateMaxZoom();
    }


    public void EndGame()
    {
        foreach(var water in watersources)
        {
            if(water != null) 
            Destroy(water);
        }
        watersources.Clear();
        simulationActive = false;

        
        waterBar.SetActive(false);
        weatherPicture.SetActive(false);
    }

    void Start()
    {
        ChangeWorldBorder();
    }

    public void FromNewGame(){

        //ChangeWorldBorder();

        Renderer renderer = pondPrefab.GetComponent<Renderer>();

        Vector2 size = renderer.bounds.size; // Размер объекта в мировых единицах

        for(int i = 0; i < amountOfWatersources; i++){
            TryInstantiate(size);
        }
        waterBar.GetComponent<WaterBar>().UpdateLevel(waterAmount);

        simulationActive = true;
        
        waterBar.SetActive(true);
        weatherPicture.SetActive(true);
    }

    void TryInstantiate(Vector2 size)
    {
        Vector2 pos;
        for(int i = 0; i < 5; i++){
            pos = UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0.0f, worldBorder);
            if(Physics2D.OverlapBoxAll(pos, size, 0.0f, LayerMask.GetMask("Plant", "Watersource")).Length == 0)
            {
                GameObject water = Instantiate(pondPrefab, pos, Quaternion.identity);
                watersources.Add(water);
                break;
            } else {
                Debug.Log("Overlap with other watersource");
            }
        }
    }


    public EnvironmentSaveData ToData()
    {
        EnvironmentSaveData data = new EnvironmentSaveData();

        /*
    public float waterEvaporCoeff = 0.1f; //how much water amount evaporates each climate update time

    public float waterAmount = 1f;//how much water in the soil? 1f - trees have all they need, 0f - trees have nothing

    public float rainFrequency = 0.5f; //percentage of rain during every climate update

    public float rainStrength = 0.6f; //how much of water amount will be restored each rain */

        data.worldBorder = this.worldBorder;
        data.nutritionalValueOfFruits = this.nutritionalValueOfFruits;
        data.waterAmount = this.waterAmount;
        data.soilHostility = this.soilHostility;
        data.waterEvaporCoeff = this.waterEvaporCoeff;
        data.rainFrequency = this.rainFrequency;
        data.rainStrength = this.rainStrength;
        data.timer = this.timer;

        //data.amountOfWatersources = this.amountOfWatersources;
        watersources.RemoveAll(s => s == null);

        foreach(var watersource in watersources)
        {
            WatersourceType type;
            if(watersource.name == "Puddle"){
                type = WatersourceType.PUDDLE;
            } else {
                type = WatersourceType.POND;
            }
            data.watersources.Add(new WatersourceSaveData(watersource.transform.position.x, watersource.transform.position.y, type));
        }

        return data;
    }

    public void FromData(EnvironmentSaveData data)
    {
        this.worldBorder = data.worldBorder;

        ChangeWorldBorder();

        this.nutritionalValueOfFruits = data.nutritionalValueOfFruits;
        this.waterAmount = data.waterAmount;
        this.soilHostility = data.soilHostility;
        this.waterEvaporCoeff = data.waterEvaporCoeff;
        this.rainFrequency = data.rainFrequency;
        this.rainStrength = data.rainStrength;
        this.timer = data.timer;

        //this.amountOfWatersources = data.amountOfWatersources;

        foreach(var watersource in data.watersources)
        {
            if(watersource.type == WatersourceType.POND){
                this.watersources.Add(Instantiate(pondPrefab, new Vector2(watersource.position_x, watersource.position_y), Quaternion.identity));
            } else {
                this.watersources.Add(Instantiate(puddlePrefab, new Vector2(watersource.position_x, watersource.position_y), Quaternion.identity));
            }
            
        }

        this.amountOfWatersources = watersources.Count;
        waterBar.GetComponent<WaterBar>().UpdateLevel(waterAmount);

        simulationActive = true;

        waterBar.SetActive(true);
        weatherPicture.SetActive(true);

        //return data;
    }

    // Update is called once per frame
    float timer = 0f;
    void Update()
    {
        if(simulationActive){
            timer += Time.deltaTime;
            if(timer >= climateUpdateTime){
                timer = 0;
                
                //Debug.Log("TryRemove");
                foreach(var watersource in watersources)
                    {
                        if(watersource != null && watersource.gameObject.name == "Puddle"){
                            //Debug.Log("Remove");
                            Destroy(watersource);
                        }
                    }
                watersources.RemoveAll(s => s == null);


                if(Random.value <= rainFrequency){

//                    Debug.Log("Rainy");
                    weatherPicture.GetComponent<WeatherScript>().ChangeWeather(false);

                    if(waterAmount + rainStrength > 1f){
                        //puddles
                        TryInstantiatePuddles();
                        waterAmount = 1f;
                    } else {
                        waterAmount += rainStrength;
                    }
                    //waterAmount = Mathf.Clamp(waterAmount + rainStrength, 0, 1);
                } else {
                    Debug.Log("Sunny");
                    weatherPicture.GetComponent<WeatherScript>().ChangeWeather(true);
                    waterAmount = Mathf.Clamp(waterAmount - waterEvaporCoeff, 0, 1);
                }
            waterBar.GetComponent<WaterBar>().UpdateLevel(waterAmount);
            }
        }
        
    }


    private void TryInstantiatePuddles()
    {
//        Debug.Log("Puddle");
        Vector2 pos;

        int amountOfPuddles = (int) (Mathf.Pow(worldBorder, 2) / 300f * (waterAmount + rainStrength));

//        Debug.Log(amountOfPuddles);

        for(int i = 0; i < amountOfPuddles; i++){
            pos = UtilityAdditions.RandomPointInAnnulus(Vector2.zero, 0f, worldBorder);
            //for(int j = 0; j < 3; j++){
                //if(Physics2D.OverlapCircleAll(pos, 4f, LayerMask.GetMask("Plant", "Watersource")).Length == 0){
                //Debug.Log("Puddle-------------------------------");
                if(Physics2D.OverlapCircleAll(pos, 1f, LayerMask.GetMask("Plant")).Length == 0 && //LayerMask.GetMask("Plant", "Watersource")
                Physics2D.OverlapCircleAll(pos, 2f, LayerMask.GetMask("Watersource")).Length == 0){
                    //Debug.Log("Success");
                    GameObject puddle;
                    puddle = Instantiate(puddlePrefab, pos, Quaternion.identity);
                    puddle.name = "Puddle";
                    //first.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);
                    watersources.Add(puddle);
                    //return;
                }
        }
    }
}


