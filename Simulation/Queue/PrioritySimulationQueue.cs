namespace Simulation;

public class PrioritySimulationQueue: ISimulationQueue
{
    private readonly List<Client> _list = new ();
    private readonly int _maxSize;
    private readonly IComparer<Client> _comparer;
    public PrioritySimulationQueue(int maxSize, IComparer<Client> comparer)
    {
        _maxSize = maxSize;
        _comparer = comparer;
    }
    public int Count => _list.Count;
    public int MaxSize => _maxSize;

    public Client? Dequeue()
    {
        Client? result = _list.Min(_comparer);
        if (result != null) _list.Remove(result);
        return result;
    }
    public bool Enqueue(Client client)
    {
        if (_maxSize == _list.Count) return false;
        _list.Add(client);
        return true;
    }
    public Client? Peek()
    {
        return _list.Max(_comparer);
    }
}
