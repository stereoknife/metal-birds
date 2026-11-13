using System;

/* This is the superset of what we need, but we can get away
with something simpler */
/* 
static double[] Heun(Func<double[], double> f, params double[][] arrays)
{
    //Size of Array of Each Parameter
    int n = arrays[0].Length;
    //Same as the size of Array of result 
    var r = new double[n];
    var args = new double[arrays.Length];
    for (int i = 0; i < n; i++)
    {
        for (int k = 0; k < arrays.Length; k++) args[k] = arrays[k][i];
        r[i] = f(args);
    }
    return r;
}
*/
//Params keyword -> https://learn.microsoft.com/en-gb/previous-versions/visualstudio/visual-studio-2008/w5zay9db(v=vs.90)
//allows multiple params of the same type
public static class Heun
{
    
//Heun's Method
    public static (double[], double) Integrate(
        Func<double[], double, double[]> f,
       double[] previousValue,
       double step,
       double t, params double[] paramsArray)
    {
        //number of parameters
        int n = previousValue.Length;
    
        //variables associated with the result, necessary for the method
        double[] r1 = new double[n];
        double[] r0 = new double[n];
        double[] rtilde = new double[n];
    
        //Initial value
        r0 = previousValue;
    
        //result of running function
        double[] v = new double[n];
        v = f(r0, t);
        for (int i = 0; i < v.Length; i++)
        {
            rtilde[i] = r0[i] + step * v[i];
        }
    
        double[] vtilde;
        vtilde = f(rtilde, t + step);
        for (int i = 0; i < v.Length; i++)
        {
            r1[i] = r0[i] + step / 2 * (v[i] + vtilde[i]);
        }
    
        return (r1, t + step);
    }
    //When calling parameters, f can expect parameters like ([x1,x2,x3,...], t)
    //So that its outputs are, amongst others, [t+step]


    //Signature of Heun for single variable equations
    public static (float, float) Integrate(
       Func<float, float[], float> f,
float[] x0, float previousValue,
       float step,
       float t, params float[] paramsArray)
    {

        //variables associated with the result, necessary for the method
        float r1;
        float r0;
        float rtilde;

        //Initial value
        r0 = previousValue;

        //result of running function
        float v;
        v = f(r0, paramsArray);
        rtilde = r0 + step * v;

        float vtilde = f(rtilde, paramsArray);

        r1 = r0 + step / 2 * (v + vtilde);
        return (r1, t + step);
    }

	//Heun's Signature for all floats
    public static (float[], float) Integrate(
        Func<float[], float, float[]> f,
       float[] previousValue,
       float step,
       float t)
    {
        //number of parameters
        int n = previousValue.Length;
    
        //variables associated with the result, necessary for the method
        float[] r1 = new float[n];
        float[] r0 = new float[n];
        float[] rtilde = new float[n];
    
        //Initial value
        r0 = previousValue;
    
        //result of running function
        float[] v = new float[n];
        v = f(r0, t);
        for (int i = 0; i < v.Length; i++)
        {
            rtilde[i] = r0[i] + step * v[i];
        }
    
        float[] vtilde;
        vtilde = f(rtilde, t + step);
        for (int i = 0; i < v.Length; i++)
        {
            r1[i] = r0[i] + step / 2 * (v[i] + vtilde[i]);
        }
    
        return (r1, t + step);
    }

}