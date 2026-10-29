using UnityEngine;
using UnityEngine.LightTransport;

public class MassaMola : MonoBehaviour
{
        public double _k { get; private set; }
        public double _b { get; private set; }
        public double _m { get; private set; }
        public double _x { get; private set; }
        double _dotx;
        double _a;
        

        public MassaMola()
        {
            _k = 1;
            _b = 1;
            _m = 1;
            _x = 0;
            _dotx = 0;
            _a = 0;
        }

        public MassaMola(double _k, double _b, double _m, double _x)
        {
            if (_k <= 0 || _b < 0 || _m <= 0)
            {
                switch (_k)
                {
                    case 0:
                        _k += 1;
                        break;
                    default:
                        _k = -_k;
                        break;
                }
                switch (_m)
                {
                    case 0:
                        _m += 1;
                        break;
                    default:
                        _m = -_m;
                        break;
                }
                _b = -_b;
                throw new System.ArgumentOutOfRangeException("The physical parameters must be greater or equal than zero");
            }
            else
            {
                this._k = _k;
                this._b = _b;
                this._m = _m;
                this._x = _x;
                this._dotx = 0;
                this._a = 0;
            }
        }

        public override string ToString()
        {
            string a = "This spring-mass-damper system has the following constants:\n" +
                        $"K: {this._k} N/m\n" + $"b: {this._b} Ns/m\n" + $"m: {this._m} kg\n"; 
            return a;
        }

        public double Acceleration()
        {
            this._a = (-this._k * this._x - (this._b * this._dotx)) / this._m;
            return this._a;
        }

        public double[] Acceleration(double[] v, double t)
        {
            double[] _temp = new double[1];
            this._a = (-this._k * this._x - (this._b * this._dotx)) / this._m;
            _temp[0] = this._a;
            return _temp;
        }

        public void SetSpeed(double speed)
        {
            this._dotx = speed;
        }

        public double[] GetSpeed(double[] v, double t)
        {
            double[] _temp = new double[1];
            _temp = v;
            return _temp;
        }

        public void SetPos(double position)
        {
            this._x = position;
        }    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { 
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
