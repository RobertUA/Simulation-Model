namespace Simulation;

public class PrioritySimulationQueue: SimulationQueueBase
{
    private readonly List<Client> _list = new ();
    private readonly int _maxSize;
    private readonly IComparer<Client> _comparer;
    public PrioritySimulationQueue(int maxSize, IComparer<Client> comparer)
    {
        _maxSize = maxSize;
        _comparer = comparer;
    }
    public override int Count => _list.Count;
    public int MaxSize => _maxSize;

    public override Client? Dequeue()
    {
        Client? result = _list.Min(_comparer);
        if (result != null) _list.Remove(result);
        return result;
    }
    protected override bool CheckEnqueue(Client client)
    {
        return _list.Count < _maxSize;
    }
    protected override void Enqueue(Client client)
    {
        _list.Add(client);
    }
    public override Client? Peek()
    {
        return _list.Max(_comparer);
    }
}
