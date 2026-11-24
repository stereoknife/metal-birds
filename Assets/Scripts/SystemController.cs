using System.Linq.Expressions;
using UnityEngine;



public class Animated : MonoBehaviour
{
    Animator animator;
    public string currentState { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play animation
        animator.Play(newState);

        //reassign current state
        currentState = newState;
    }

}

public class SystemController : MonoBehaviour
{
    const string ON = "on";
    const string OFF = "off";
    const string BLINK = "blinking";

    [SerializeField] private Switch battSwitch;
    [SerializeField] private Switch apuSwitch;
    [SerializeField] private Lamp battLight;
    [SerializeField] private Lamp apuLight;


    //state variables
    private bool batt = false;
    private bool apu = false;
    private bool arm = false;

    public void OnEnable()
    {

        battSwitch.OnSwitchPressed += OnBattSwitchPressedHandler;
        // Don't forget to unsubscribe in OnDisable
        apuSwitch.OnSwitchPressed += OnAPUSwitchPressedHandler;

    }

    void OnBattSwitchPressedHandler()
    {
        if (battSwitch.currentState == OFF)
        {
            battSwitch.ChangeAnimationState(ON);
            batt = true;
        } else
        {
            battSwitch.ChangeAnimationState(OFF);
            batt = false;
        }
    }

    void OnAPUSwitchPressedHandler()
    {
        // Open door
    }

    // Update is called once per frame
    void Update()
    {
        if (batt)
            battLight.ChangeAnimationState(ON);
        else
            battLight.ChangeAnimationState(OFF);
    }
}
