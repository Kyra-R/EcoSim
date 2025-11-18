using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureMoverData
{

    public bool OnCenterMoving;

    public int randomMoveDirectionIndex;

    public States state;

    public float timeBetweenChangeDirection;

    public float x_movement;

    public float y_movement;
  
}
