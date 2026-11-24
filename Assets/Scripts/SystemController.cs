using UnityEngine;



[RequireComponent(typeof(Animator))]
public class Animated : MonoBehaviour
{
    Animator animator;
    public string currentState { get; private set; }

    private void Awake()
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

    [SerializeField] private Animated battAnim;
    [SerializeField] private Animated apuAnim;
    [SerializeField] private Animated battLight;
    [SerializeField] private Animated apuLight;
    
    [SerializeField] private Switch battSwitch;
    [SerializeField] private Switch apuSwitch;


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
        if (battAnim.currentState == OFF)
        {
            battAnim.ChangeAnimationState(ON);
            batt = true;
        } else
        {
            battAnim.ChangeAnimationState(OFF);
            batt = false;
        }
    }

    void OnAPUSwitchPressedHandler()
    {
        // Open door
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (batt)
            battLight.ChangeAnimationState(ON);
        else
            battLight.ChangeAnimationState(OFF);
    }
    */
}
