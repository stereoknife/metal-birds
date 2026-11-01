using System;
using UnityEngine;

public class BetterLERP : MonoBehaviour
{
    public static float BetterLerp(float a, float b, float r)
    {
        return (a - b) * UnityEngine.Mathf.Pow(r, Time.deltaTime) + b;
    }
}
