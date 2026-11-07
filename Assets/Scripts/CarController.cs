using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Unity.Mathematics.math;

public static class CarInput
{
    private static KeyControl GetKey(Key key) => Keyboard.current[key];
    
    private const Key ThrottleKey = Key.W;
    private const Key BrakeKey = Key.S;
    private const Key LeftKey = Key.A;
    private const Key RightKey = Key.D;

    public static float Brake => GetKey(BrakeKey).value;
    public static float Throttle => GetKey(ThrottleKey).value;
    // Left is -1, right is +1
    public static float Steer => GetKey(RightKey).value - GetKey(LeftKey).value;
}

public class CarController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Transform lfWheel;
    [SerializeField] private Transform rfWheel;
    
    public void FixedUpdate()
    {
        var steer = CarInput.Steer;

        if (abs(steer) > 0.5f)
        {
            lfWheel.localRotation = rfWheel.localRotation =
                Quaternion.AngleAxis(rotationSpeed * steer, Vector3.up);
        }
        else
        {
            lfWheel.localRotation = rfWheel.localRotation =
                Quaternion.identity;
        }
        
        var throttle = CarInput.Throttle - CarInput.Brake;
        
        var position = transform.position;
        position += transform.forward * (throttle * Time.fixedDeltaTime * speed);
        var rotation = transform.rotation;
        rotation *= Quaternion.AngleAxis(CarInput.Steer * rotationSpeed * throttle * Time.fixedDeltaTime, transform.up);
        transform.position = position;
        transform.rotation = rotation;
    }
}
