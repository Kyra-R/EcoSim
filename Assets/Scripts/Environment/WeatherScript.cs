using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sun;
    public GameObject rain;

    void Start()
    {    
        ChangeWeather(true);
    }

    public void ChangeWeather(bool sunny)
    {
        sun.SetActive(sunny);
        rain.SetActive(!sunny);
//        Debug.Log(open);
    }



    // Update is called once per frame

}
