using UnityEngine;

public class ArtificialHorizon : MonoBehaviour
{
    [SerializeField] private GameObject plane; //plane source
    [SerializeField] private Transform childTransform;
    [Range(-90.0F, 90.0F)] public float angle =  0;   //degrees
    [SerializeField] public float uvRange; // 90°
    [SerializeField] public float uvScale; //0.346898
    [SerializeField] public float roll;
    [SerializeField] public float pitch;

    private Renderer _renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.materials[0].SetFloat("_angleLimit", uvRange);
        _renderer.materials[0].SetFloat("_angleScale", uvScale);
    }


    // Update is called once per frame
    void Update()
    {
        _renderer.materials[0].SetFloat("_pitch", pitch);
        childTransform.rotation = Quaternion.AngleAxis(roll, new Vector3(0,0,-1));
    }
}
