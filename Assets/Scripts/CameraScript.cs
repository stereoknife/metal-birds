using System;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static BetterLERP;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;
    [SerializeField] private float deltaHeight;
    [SerializeField] private float deltaXRatio;
    [SerializeField] private float ratioY;
    [SerializeField] private float ratioX;
    [SerializeField] private float ratioLook;
    private UnityEngine.Vector3 target_pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //calculating where the camera should be, given the parameters
        target_pos = target.transform.position;
        target_pos = target_pos - target.transform.forward * (float)deltaXRatio;
        target_pos.y -= (float)deltaHeight;
        //assigning the position to the camera
        cam.transform.position = target_pos;
        //telling where camera ought to look at
        cam.transform.LookAt(target.transform);
    }

    //Convenience Method
    UnityEngine.Vector3 GetTargetPos()
    {
        UnityEngine.Vector3 b = target.transform.position;
        b = b - target.transform.forward * (float)deltaXRatio;
        b.y = target.transform.position.y + (float)deltaHeight;
        return b;
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Vector3 b = GetTargetPos();
        
        //Doing The Lerp
        UnityEngine.Vector3 a = cam.transform.position;
        cam.transform.position = new UnityEngine.Vector3(BetterLERP.BetterLerp(a.x,b.x,ratioX),
            BetterLERP.BetterLerp(a.y, b.y, ratioY),
            BetterLERP.BetterLerp(a.z, b.z, ratioX));
       //target_pos = b;
        cam.transform.LookAt(target.transform);
       //Debug.Log($"{target.transform.forward * (float)deltaXRatio}");
    }
}
