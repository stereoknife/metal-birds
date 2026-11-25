using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class ClickCaster : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        
        var pos = Mouse.current.position.ReadValue();
        
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(pos), out hit))
        {
            hit.transform.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }
    }
}