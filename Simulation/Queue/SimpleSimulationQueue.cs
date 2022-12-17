namespace Simulation;

public class SimpleSimulationQueue : SimulationQueueBase
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
    public override int Count => _queue.Count;
    public int MaxSize => _maxSize;

    public override Client? Dequeue()
    {
        if (_queue.Count == 0) return null;
        return _queue.Dequeue();
    }
    protected override bool CheckEnqueue(Client client)
    {
        return _queue.Count < _maxSize;
    }
    protected override void Enqueue(Client client)
    {
        _queue.Enqueue(client);
    }
    public override Client? Peek()
    {
        if (_queue.Count == 0) return null;
        return _queue.Peek();
    }
}
