using UnityEngine;


public class Switch : Animated
{
    public delegate void StateChange();
    Collider collider0;
    public event StateChange OnSwitchPressed;
    //public event StateChange OnSwitchReleased;

    /*void OnCollisionEnter(Collision collision)
    {
        // Test for right conditions otherwise return;
        OnSwitchPressed?.Invoke();
    }

    void OnCollisionExit(Collision other)
    {
        // Test for right conditions otherwise return;
        OnSwitchReleased?.Invoke();
    }*/

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider == collider0)
                {
                    OnSwitchPressed?.Invoke();
                }
            }
        }
    }
}
