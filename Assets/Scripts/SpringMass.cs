using UnityEngine;

public class SpringMass : MonoBehaviour
{
    [SerializeField] private Vector3 movement;
    [SerializeField] private double spring = 1.0;
    [SerializeField] private double damper = 1.0;
    [SerializeField] private double mass = 1.0;
    [SerializeField] private double displacement = 0.0;
    
    private double _speed = 0.0;
    private double _acceleration = 0.0;
    

    public override string ToString()
    {
        return "This spring-mass-damper system has the following constants:\nK: {_spring} N/m\nb: {{_damper}} Ns/m\nm: {{{{_mass}}}} kg\n"; 
    }

    public double Acceleration()
    {
        return _acceleration = (-spring * displacement - damper * _speed) / mass;
    }

    public double[] Acceleration(double[] v, double t)
    {
        return new[] { Acceleration() };
    }

    public void SetSpeed(double speed)
    {
        _speed = speed;
    }

    public double[] GetSpeed(double[] v, double t)
    {
        return new[] { _speed };
    }

    public void SetPos(double position)
    {
        displacement = position;
    }    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { 
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var (speed, _) = Heun.Integrate(Acceleration, new[] { _speed }, Time.fixedDeltaTime, Time.timeAsDouble);
        var (displacement, _) = Heun.Integrate(GetSpeed, new[] { this.displacement }, Time.fixedDeltaTime, Time.timeAsDouble);
        _speed = speed[0];
        this.displacement = displacement[0];

        transform.position = movement * (float)this.displacement;
    }
}
