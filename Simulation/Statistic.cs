namespace Simulation;

public class Statistic
{
    public int TotalCount = 0;
    public int SuccessCount = 0;
    public int FailsCount = 0;
    public double FailChance => (double)FailsCount / TotalCount;
}