namespace Distribution;

public static class Distribution
{
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
    public static double Exponential(double l, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        //double rand = Random.Shared.NextDouble();
        return -1 / l * Math.Log10(rand());
    }
    public static double Gaus(double a, double q, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        double sum = -6;
        for (int i = 0; i < 12; i++)
        {
            sum += rand();
        }
        return q * sum + a;
    }
    public static double Range(double min, double max, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        return min + (max - min) * rand();
    }
    public static int Range(int min, int max, Func<double>? rand = null)
    {
        if (rand == null) rand = () => Random.Shared.NextDouble();
        return min + (int)Math.Ceiling((max - min) * rand());
    }
}