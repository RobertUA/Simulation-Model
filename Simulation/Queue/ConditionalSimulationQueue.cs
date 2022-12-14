namespace Simulation;

public class ConditionalSimulationQueue : ISimulationQueue
{
    private readonly Queue<Client> _queue = new();
    private Func<bool> _condition;
    public ConditionalSimulationQueue()
    {
        _condition = () => true;
    }
    public ConditionalSimulationQueue(Func<bool> condition)
    {
        _condition = condition;
    }
    public void SetCondition(Func<bool> condition)
    {
        _condition = condition;
    }
    public int Count => _queue.Count;

    public Client? Dequeue()
    {
        if (_queue.Count == 0) return null;
        return _queue.Dequeue();
    }
    public bool Enqueue(Client client)
    {
        if (_condition.Invoke() == false) return false;
        _queue.Enqueue(client);
        return true;
    }
    public Client? Peek()
    {
        throw new NotImplementedException();
    }
}
