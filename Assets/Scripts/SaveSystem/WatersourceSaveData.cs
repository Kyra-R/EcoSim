using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WatersourceSaveData
{
    public float position_x;

    public float position_y;

    public WatersourceType type;

    public WatersourceSaveData(float x, float y, WatersourceType watertype)
    {
        position_x = x;
        position_y = y;
        type = watertype;
    }
}

public enum WatersourceType
{
    POND, PUDDLE
}
