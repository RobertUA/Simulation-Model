namespace Simulation;

public class SimpleSimulationQueue : ISimulationQueue
{
    private readonly Queue<Client> _queue = new();
    private readonly int _maxSize;
    public SimpleSimulationQueue()
    {
        _maxSize = int.MaxValue;
    }
    public SimpleSimulationQueue(int maxSize)
    {
        _maxSize = maxSize;
    }
    public int Count => _queue.Count;
    public int MaxSize => _maxSize;

    public Client? Dequeue()
    {
        if (_queue.Count == 0) return null;
        return _queue.Dequeue();
    }
    public void Enqueue(Client client)
    {
        _queue.Enqueue(client);
    }
    public Client? Peek()
    {
        if (_queue.Count == 0) return null;
        return _queue.Peek();
    }
}
