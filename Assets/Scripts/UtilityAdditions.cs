using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityAdditions 
{
    // Start is called before the first frame update
public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius){ //to some misc class

    Vector2 randomDirection = Random.insideUnitCircle.normalized; // There are more efficient ways, but well
    float minRadius2 = minRadius * minRadius;
    float maxRadius2 = maxRadius * maxRadius;
    float randomDistance = Mathf.Sqrt(Random.value * (maxRadius2 - minRadius2) + minRadius2);
    return origin + randomDirection * randomDistance;
}
}
