namespace Simulation;

public class SimpleSimulationQueue : ISimulationQueue
{
    private readonly Queue<State> _queue = new();
    private readonly int _maxSize;
    public SimpleSimulationQueue(int maxSize=int.MaxValue)
    {
        _maxSize = maxSize;
    }
    public int Count => _queue.Count;
    public int MaxSize => _maxSize;

    public State? Dequeue()
    {
        if (_queue.Count == 0) return null;
        return _queue.Dequeue();
    }
    public void Enqueue(State state)
    {
        _queue.Enqueue(state);
    }
    public State? Peek()
    {
        if (_queue.Count == 0) return null;
        return _queue.Peek();
    }
}
