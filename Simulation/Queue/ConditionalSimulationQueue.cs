namespace Simulation;

public class ConditionalSimulationQueue : SimulationQueueBase
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
    public override int Count => _queue.Count;

    public override Client? Dequeue()
    {
        if (_queue.Count == 0) return null;
        return _queue.Dequeue();
    }
    protected override bool CheckEnqueue(Client client)
    {
        return _condition.Invoke();
    }
    protected override void Enqueue(Client client)
    {
        _queue.Enqueue(client);
    }
    public override Client? Peek()
    {
        if(_queue.Count == 0) return null;
        return _queue.Peek();
    }
}
