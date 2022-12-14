namespace RobRandom;

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

        return (-1 / l) * Math.Log(rand.Invoke());
    }
    public static double Gaus(double a, double q, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        double sum = -6;
        for (int i = 0; i < 12; i++)
        {
            sum += rand.Invoke();
        }
        return q * sum + a;
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
}