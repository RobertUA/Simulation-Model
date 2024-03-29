﻿namespace RobRandom;

public class Uniform
{
    private double _uniformValue;
    private readonly double _a, _b, _c;
    public Uniform(double firstValue, double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
        _uniformValue = firstValue;
    }
    public double Next()
    {
        _uniformValue = (_a * _uniformValue + _b) % _c;
        return _uniformValue / _c;
    }
}
public static class Distribution
{
    public static double Exponential(double l, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();

        return (-1/l) * Math.Log(rand.Invoke());
    }
    public static double Gaus(double u, double q, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        double sum = -6;
        for (int i = 0; i < 12; i++)
        {
            sum += rand.Invoke();
        }
        return q * sum + u;
    }
    public static double Erlang(double u, double k, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        double sum = 1;
        for (int i = 0; i < k; i++)
        {
            sum *= rand.Invoke();
        }
        return (-1 / (k * u)) * Math.Log(sum);
    }
    public static double RangeDouble(double min, double max, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        return min + (max - min) * rand.Invoke();
    }
    public static int RangeInteger(int min, int max, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        return (int)Math.Floor(RangeDouble(min, max + 1 - double.Epsilon, rand));
    }
    public static void Check(double[] values, Func<double, double> tFunc, int count = 20)
    {
        SortedDictionary<double, int> tabl = new();
        double step = Math.Abs((values.Max() - values.Min()) / count);
        foreach (var value in values)
        {
            double roundVal = Math.Floor(value / step) * step;
            //Console.WriteLine($"roundVal = {roundVal}");
            if (tabl.ContainsKey(roundVal))
                tabl[roundVal] += 1;
            else
                tabl.Add(roundVal, 1);
        }
        double sum = 0;
        foreach (var row in tabl)
        {
            double pi = tFunc(row.Key + step) - tFunc(row.Key);
            double tValue = values.Length * pi;
            double element = Math.Pow(row.Value - tValue, 2) / tValue;
            sum += element;
            Console.WriteLine($"{row.Key}: {row.Value} | {tValue} \t\t\t | {element} | {sum}");
        }
        Console.WriteLine($"\t\tx2 = {sum}");
    }
    public static double ExpF(double x, double l)
    {
        double result;
        if (x <= 0) result = 0;
        else result =  1 -  Math.Exp(-l * x);
        //Console.WriteLine($"ExpF({x}, {l}) = {result}");
        return result;
    }
    public static double GausF(double x, double u, double q)
    {
        double result;
        result = -(Erf((u - x) / (Math.Sqrt(2) * q))) / 2;
        //result = (1 / (q * Math.Sqrt(2 * Math.PI))) * Math.Exp(-Math.Pow(x - u, 2) / (2 * Math.Pow(q, 2)));
        return result;
    }
    static double Erf(double x)
    {
        // constants
        double a1 = 0.254829592;
        double a2 = -0.284496736;
        double a3 = 1.421413741;
        double a4 = -1.453152027;
        double a5 = 1.061405429;
        double p = 0.3275911;

        // Save the sign of x
        int sign = 1;
        if (x < 0)
            sign = -1;
        x = Math.Abs(x);

        // A&S formula 7.1.26
        double t = 1.0 / (1.0 + p * x);
        double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

        return sign * y;
    }
    public static double UniformF(double x, double a, double b)
    {
        double result;
        if (x < 0) result = 0;
        else if (x > 1) result = 1;
        else result = x;
        return result;
    }
}