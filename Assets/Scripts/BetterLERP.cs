using UnityEngine;
using static Unity.Mathematics.math;

public static class BetterLERP
{
    public static float BetterLerp(float a, float b, float r)
    {
        return (a - b) * pow(r, Time.deltaTime) + b;
    }
}
