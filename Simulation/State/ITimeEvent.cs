namespace Simulation;

public interface ITimeEvent
{
    public string Info { get; }
    public double StartTime { get; }
    public double EndTime { get; }
    public void End();
}
