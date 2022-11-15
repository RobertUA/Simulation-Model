namespace Simulation;

public interface ISimulationQueue
{
    public void Enqueue(Client state);
    public Client? Peek();
    public Client? Dequeue();
    public int Count { get; }
    public int MaxSize { get; }
}
