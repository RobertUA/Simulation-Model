namespace Simulation;

public interface ISimulationQueue
{
    public void Enqueue(State state);
    public State? Peek();
    public State? Dequeue();
    public int Count { get; }
    public int MaxSize { get; }
}
