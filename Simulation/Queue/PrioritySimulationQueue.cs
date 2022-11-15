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
    public void Enqueue(Client client)
    {
        _list.Add(client);
    }
    public Client? Peek()
    {
        return _list.Max(_comparer);
    }
}
