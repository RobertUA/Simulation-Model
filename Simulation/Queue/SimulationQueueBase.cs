namespace Simulation;

public abstract class SimulationQueueBase
{
    public Statistic Statistic = new ();
    public bool TryEnqueue(Client client)
    {
        bool check = CheckEnqueue(client);
        Statistic.TotalCount++;
        if (check == true)
        {
            Statistic.SuccessCount++;
            Enqueue(client);
        }
        else
        {
            Statistic.FailsCount++;
        }
        return check;
    }
    public string GetStats()
    {
        return $"Fails: {Statistic.FailsCount}; Total: {Statistic.TotalCount}; Percent: {(float)Statistic.FailsCount/Statistic.TotalCount}";
    }
    protected abstract bool CheckEnqueue(Client client);
    protected abstract void Enqueue(Client client);
    public abstract Client? Peek();
    public abstract Client? Dequeue();
    public abstract int Count { get; }
}
