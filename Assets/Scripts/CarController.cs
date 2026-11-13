using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
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
        return 0.5f * pow(V,2f) * rho * S * Cd;
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
    private float rotationY = 0;
    private Quaternion rotVehicle; //rotation of vehicle
    private Vector3 posVehicle; //position of vehicle
    private Quaternion initialRfRotation; //Initial Rotations of Left and Right Forward Wheels
    private Quaternion initialLfRotation; 
    private float rotrfWheel; //rotation right forward wheel
    private float rotlfWheel; //rotation left forward wheel
    private float t = 0;
    //helper side-variables
    //avoid garbage collection
    float R;
    float delta;
    float angspeed;
    float dx;
    float dy;
    float x;
    float y;
    float displacement;
    float Theta;
    float dTheta;
    float deltaDeg;
    float signL;
    float signR;
    float steer;
    float throttle;

    /// <summary>
    /// Returns the derivatives of the vehicle's state variables for integration.
    /// State: [Speed, Displacement, Heading]
    /// Derivatives: [Acceleration, Speed, Angular Velocity]
    /// </summary>
    public float[] Accelerate(float[] x0, float t)
    {
        float[] y = new float[3];

        // Longitudinal acceleration from propulsion and aerodynamic drag
        y[0] = (Aerodynamics.Thrust(x0[0], drivePower * 1000) * throttle -
            Aerodynamics.Drag(Cd, x0[0], rho, frontArea)) / mass;

        // Rate of change of displacement
        y[1] = x0[0];

        // Average front-wheel steering angle (radians)
        delta = TORADIANS * (rotrfWheel + rotlfWheel) / 2f;

        // Yaw rate, assuming no lateral slip at the center of mass
        y[2] = x0[0] * tan(delta) / distFWheels;

        return y;
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
        initialRfRotation = rfWheel.localRotation;
        initialLfRotation = lfWheel.localRotation;
    }

    public void FixedUpdate()
    {
        steer  = CarInput.Steer;
        throttle = CarInput.Throttle - CarInput.Brake;

        //-=> WHEEL ROTATION <=-
        if (abs(steer) > 0.05f)
        {
            rotrfWheel = clamp(rotrfWheel + rotationSpeed * steer * Time.fixedDeltaTime,
                  -rotationLimit, rotationLimit);
            rotlfWheel = rotrfWheel;
        } else
        {
            if(abs(rotrfWheel) >= 1f)
            {
                signR = rotrfWheel / abs(rotrfWheel);
                signL = rotlfWheel / abs(rotlfWheel);
                rotrfWheel -= signR * decayRate * Time.fixedDeltaTime; 
                rotlfWheel -= signL * decayRate * Time.fixedDeltaTime;
            } else
            {
                rotrfWheel = 0f;
                rotlfWheel = 0f;
            }
        }

        //-=>VEHICLE MOVEMENT<=-
        float dt = Time.fixedDeltaTime;

        float[] x0 = new float[3];
        x0[0] = speed;
        x0[1] = displacement;
        x0[2] = Theta;


        (float[] x1, _) = Heun.Integrate(Accelerate, x0, dt, t);

        // Calculate increments before updating state variables
        float ds = x1[1] - x0[1];
        // Angular displacement in radians
        dTheta = x1[2] - x0[2];

        // Update integrated state
        speed = x1[0];
        displacement = x1[1];
        Theta = x1[2];

        //delta = TORADIANS * (rotrfWheel + rotlfWheel) / 2f;

        angspeed = speed * tan(delta) / distFWheels;

        // Calculate displacement in vehicle-local coordinates
        if (abs(angspeed) > 0.0001f)
        {
            R = speed / angspeed;
            dx = R * (1f - cos(dTheta));
            dy = R * sin(dTheta); // local Z displacement
        }
        else
        {
            dx = 0f;
            dy = ds;
        }

        t += dt;

        // Convert local displacement to world coordinates
        Vector3 localDisplacement = new Vector3(dx, 0f, dy);

        Vector3 worldDisplacement =
            vehicle.transform.TransformDirection(localDisplacement);

        // Apply translation before changing orientation
        vehicle.transform.position += worldDisplacement;

        // Apply angular displacement (Unity expects degrees)
        deltaDeg = dTheta * TODEGREES;

        vehicle.transform.Rotate(0f, deltaDeg, 0f, Space.Self);

        //Debug.Log($"{rotlfWheel}");
    }

    public void Update()
    {
        //making sure the floats go to zero if the value is small
        //so far we are keeping their angles always identical
        if (abs(rotrfWheel) <= 1 && abs(rotlfWheel) <= 1)
        {
            rfWheel.localRotation = initialRfRotation;
            lfWheel.localRotation = initialLfRotation;
            //note to Nosh: You can do
            //"rotrfWheel = rotlfWheel = Quaternion.identity;"
            //in C#
        }
        else
        {
            //making axis to keep track of "up" axis in the parent coordinate
            Vector3 axisR = rfWheel.parent.InverseTransformDirection(
                vehicle.transform.up
            );

            Vector3 axisL = lfWheel.parent.InverseTransformDirection(
                vehicle.transform.up
            );

            //the meshes orient according to the rotation angle
            // Quaternion multiplication order matters:
            // Applying the steering quaternion on the LEFT rotates around
            // an axis expressed in the wheel's parent coordinate system,
            // preserving the wheel's initial local orientation.
            // Reversing the order would apply the rotation relative to
            // the wheel's initial local coordinate system instead.

            rfWheel.localRotation =
                Quaternion.AngleAxis(rotrfWheel, axisR) * initialRfRotation;

            lfWheel.localRotation =
                Quaternion.AngleAxis(rotlfWheel, axisL) * initialLfRotation;
        }

    }
}
