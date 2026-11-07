using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Unity.Mathematics.math;

//Inputs
public static class CarInput
{
    //Keyboard keys pressed
    private static KeyControl GetKey(Key key) => Keyboard.current[key];
    // Input keys
    private const Key ThrottleKey = Key.W;
    private const Key BrakeKey = Key.S;
    private const Key LeftKey = Key.A;
    private const Key RightKey = Key.D;
    //Inputs
    public static float Brake => GetKey(BrakeKey).value;
    public static float Throttle => GetKey(ThrottleKey).value;
    // Left is -1, right is +1
    public static float Steer => GetKey(RightKey).value - GetKey(LeftKey).value;
}

//Helper Classes
public static class Aerodynamics
{ 
    public static float Drag(float Cd, float V, float rho, float S)
    {
        return (1/2) * pow(V,2f) * rho * S * Cd;
    }

    public static float Thrust(float V, float P)
    {
        //Quick and Dirty conditioning
        if (V <= 0)
        {
            //Isn't physically accurate
            V = 1;
        }
        return P / V;
    }
}


public class CarController : MonoBehaviour
{
    //properties of vehicle
    [SerializeField] private float mass = 200f; // kg
    [SerializeField] private float drivePower = 15; // kilowatts
    [SerializeField] private float rotationLimit = 20f; //°
    [SerializeField] private float rotationSpeed = 90f; //°/s
    [SerializeField] private float decayRate = 45f; //°/s
    [SerializeField] private float distFWheels; //distance of front wheels to center
    [SerializeField] private float Cd = 1.45f; //Drag Coefficient
    [SerializeField] private float frontArea;  //m²
    [SerializeField] private float rho = 1.225f;//kg/m³
    //parts of vehicle
    [SerializeField] private GameObject vehicle;
    [SerializeField] private Transform lfWheel;
    [SerializeField] private Transform rfWheel;
    [SerializeField] private Transform rwdWheels;
    //state variables
    private float speed = 0;
    private float accel = 0;
    private float angSpeed = 0;
    private Quaternion rotVehicle; //rotation of vehicle
    private Vector3 posVehicle; //position of vehicle
    private float rotrfWheel; //rotation left forward wheel
    private float rotlfWheel; //rotation left forward wheel
    private float t = 0;

    public float Accelerate(float V)
    {
        return Aerodynamics.Thrust(speed, drivePower * 1000) -
            Aerodynamics.Drag(Cd, speed, rho, frontArea) / mass;
    }

    public void Start()
    {
        //The vehicle's initial setup
        posVehicle = vehicle.transform.position;
        rotVehicle = vehicle.transform.rotation;
        rotrfWheel = 0;
        rotlfWheel = 0;
        //it doesn't technically need the wheels to be alligned
        //but for now that'll be the case
        rfWheel.localRotation = Quaternion.identity;
        lfWheel.localRotation = Quaternion.identity;
    }

    public void FixedUpdate()
    {
        var steer = CarInput.Steer;
        var throttle = CarInput.Throttle - CarInput.Brake;

        //-=> WHEEL ROTATION <=-
        if (abs(steer) > 0.05f)
        {
            if (abs(rotrfWheel) < 15)
            {
                rotrfWheel += rotationSpeed * steer * Time.fixedDeltaTime;
                rotlfWheel += rotationSpeed * steer * Time.fixedDeltaTime;
            } else
            {
                rotrfWheel = rotrfWheel / abs(rotrfWheel) * 15;
                rotlfWheel = rotlfWheel / abs(rotlfWheel) * 15;
            }
        } else
        {
            if(abs(rotrfWheel) >= 1f)
            {
                var signR = rotrfWheel / abs(rotrfWheel);
                var signL = rotlfWheel / abs(rotlfWheel);
                rotrfWheel -= signR * decayRate * Time.fixedDeltaTime; 
                rotlfWheel -= signL * decayRate * Time.fixedDeltaTime;
            } else
            {
                rotrfWheel = 0f;
                rotlfWheel = 0f;
            }
        }

        //-=>VEHICLE MOVEMENT<=-
        (speed, _) = Heun.Integrate(Accelerate, speed, Time.fixedDeltaTime, t);
        t += Time.fixedDeltaTime;

        //// ### ADDD THEEEE RESSSST HEEEEREEEE ### ?? ?? ?? ?? ////

        Debug.Log($"{rotlfWheel}");
    }

    public void Update()
    {
        //making sure the floats go to zero if the value is small
        //so far we are keeping their angles always identical
        if (abs(rotrfWheel) <= 1 && abs(rotlfWheel) <= 1)
        {
            rfWheel.localRotation = Quaternion.identity;
            lfWheel.localRotation = Quaternion.identity;
            //note to Nosh: You can do
            //"rotrfWheel = rotlfWheel = Quaternion.identity;"
            //in C#.
        }
        else
        {
            //the meshes orient according to the rotation angle
            rfWheel.localRotation = Quaternion.AngleAxis(rotrfWheel, rfWheel.transform.up);
            lfWheel.localRotation = Quaternion.AngleAxis(rotlfWheel, lfWheel.transform.up);
        }
    }
}
