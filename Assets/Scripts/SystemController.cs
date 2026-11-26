using UnityEngine;



public class SystemController : MonoBehaviour
{
    //const string ON = "BaseLayer.on";
    //const string OFF = "BaseLayer.off";
    //const string BLINK = "BaseLayer.blinking";
    const string ON = "on";
    const string OFF = "off";
    const string BLINK = "blinking";


    [SerializeField] private Animated battAnim;
    [SerializeField] private Animated apuAnim;
    [SerializeField] private Animated armAnim;
    [SerializeField] private Animated battLight;
    [SerializeField] private Animated apuLight;

    [SerializeField] private Switch battSwitch;
    [SerializeField] private Switch apuSwitch;
    [SerializeField] private Switch armSwitch;



    //state variables
    private bool batt = false;
    private bool apu = false;
    private bool arm = false;

    public void OnEnable()
    {
        //Connecting Switches
        if (battSwitch != null)
            battSwitch.OnSwitchPressed += OnBattSwitchPressedHandler;
        if (apuSwitch != null)
            apuSwitch.OnSwitchPressed += OnAPUSwitchPressedHandler;
        if (armSwitch != null)
            armSwitch.OnSwitchPressed += OnARMSwitchPressedHandler;
        // Don't forget to unsubscribe in OnDisable
    }

    private void Start()
    {
        // Initialize both battery visuals to the off frame.
        if (battAnim != null) battAnim.ChangeAnimationState(batt ? ON : OFF);
        if (battLight != null) battLight.ChangeAnimationState(batt ? ON : OFF);
        if (apuAnim != null) apuAnim.ChangeAnimationState(apu ? ON : OFF);
        if (apuLight != null) apuLight.ChangeAnimationState(apu ? ON : OFF);
        if (armAnim != null) armAnim.ChangeAnimationState(arm ? ON : OFF);
    }

    private void OnDisable()
    {
        //Disconnecting Switches
        if (battSwitch != null)
            battSwitch.OnSwitchPressed -= OnBattSwitchPressedHandler;
        if (apuSwitch != null)
            apuSwitch.OnSwitchPressed -= OnAPUSwitchPressedHandler;
        if (armSwitch != null)
            armSwitch.OnSwitchPressed -= OnARMSwitchPressedHandler;
    }

    void OnBattSwitchPressedHandler()
    {
        batt = !batt;
        if (battAnim != null) battAnim.ChangeAnimationState(batt ? ON : OFF);
        if (battLight != null) battLight.ChangeAnimationState(batt ? ON : OFF);
    }

    void OnAPUSwitchPressedHandler()
    {
        apu = !apu;
        if (battAnim != null) apuAnim.ChangeAnimationState(apu ? ON : OFF);
        if (battLight != null) apuLight.ChangeAnimationState(apu ? ON : OFF);
    }

    void OnARMSwitchPressedHandler()
    {
        arm = !arm;
        if (armAnim != null) armAnim.ChangeAnimationState(arm ? ON : OFF);
    }

    // Update is called once per frame

    void Update()
    {

    }
}
