using UnityEngine;
using UnityEngine.InputSystem;

public class Switch : MonoBehaviour
{
    public delegate void StateChange();
    Collider col;
    public event StateChange OnSwitchPressed;

    // Don't forget to assign components!!!
    public void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null) Debug.LogError($"{gameObject.name} has no collider attached");
    }

    private void OnClick()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        var pos = Mouse.current.position.ReadValue();
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit))
        {
            if (hit.collider == col)
            {
                OnSwitchPressed?.Invoke();
            }
        }
    }
}
