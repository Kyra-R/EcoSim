using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    public Image waterBarFill;



    public void UpdateLevel(float value)
    {
        waterBarFill.fillAmount = value;
    }

    void Update()
    {

    }
}
