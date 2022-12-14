namespace Simulation;

public interface ISimulationQueue
{
    public bool Enqueue(Client state);
    public Client? Peek();
    public Client? Dequeue();
    public int Count { get; }
}
