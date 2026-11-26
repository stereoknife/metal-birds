using UnityEngine;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MeshRenderer))]
public class Animated : MonoBehaviour
{
    Animator animator;
    MeshRenderer meshRenderer;
    Material materialInstance;

    bool isOn = false;
    //readonly string textureProperty = "_UnlitColorMap";
    readonly string textureProperty = "_BaseMap";
    Vector2 initialOffset;

    public string currentState { get; private set; }


    private void Awake()
    {
        //animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Create a separate material instance for this object.
        materialInstance = meshRenderer.material;

        initialOffset = materialInstance.GetTextureOffset(textureProperty);

    }

    public void ChangeAnimationState(string newState)
    {
        //blink doesn't care
        if (currentState != "blinking")
        {
            //stop the same animation from interrupting itself
            if (newState == currentState)
                return;
        }
        Debug.Log(newState);
        switch (newState)
        {
            case "on":
                initialOffset = new Vector2(0f, 0.5f);
                materialInstance.SetTextureOffset(
                    textureProperty,
                    initialOffset);
                isOn = true;
               break;
            case "off":
                initialOffset = new Vector2(0f, 0.0f);
                materialInstance.SetTextureOffset(
                    textureProperty,
                    initialOffset);
                isOn = false;
                break;
            case "blinking":
                isOn = !isOn;
                initialOffset = new Vector2(0f, isOn ? 0.5f : 0.0f);
                materialInstance.SetTextureOffset(
                    textureProperty,
                    initialOffset);
                break;
            //default is off
            default:
                initialOffset = new Vector2(0f, 0.0f);
                materialInstance.SetTextureOffset(
                    textureProperty,
                    initialOffset);
                isOn = false;
                break;
        }
        ////play animation
        //animator.Play(newState,-1);
        ////reassign current state
        //currentState = newState;
    }

    private void OnDestroy()
    {
       if (materialInstance != null)
            Destroy(materialInstance);
    }
}
