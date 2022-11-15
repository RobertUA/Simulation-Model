namespace Simulation;

public class PrioritySimulationQueue: ISimulationQueue
{
    private readonly List<State> _list = new ();
    private readonly int _maxSize;
    private readonly IComparer<State> _comparer;
    public PrioritySimulationQueue(int maxSize, IComparer<State> comparer)
    {
        _maxSize = maxSize;
        _comparer = comparer;
    }
    public int Count => _list.Count;
    public int MaxSize => _maxSize;

    public State? Dequeue()
    {
        State? result = _list.Min(_comparer);
        if (result != null) _list.Remove(result);
        return result;
    }
    public void Enqueue(State state)
    {
        _list.Add(state);
    }
    public State? Peek()
    {
        return _list.Max(_comparer);
    }
}
