namespace Simulation;

public class Channel : ITimeEvent
{
    public Process Process;
    public Func<double> RandFunc;
    //
    public int Id;
    public ISimulationQueue? StateQueue;
    public Client? Client = null;
    //
    public Statistic Statistic = new();
    public Timeline Timeline = new();
    //
    private double _startTime;
    private double _endTime = -1;
    public string Info 
        => $"Channel {Id} of {Process.Name} | Client: {(Client != null ? Client.Type : "null")}";
    public double EndTime => _endTime;
    public double StartTime => _startTime;
    public Channel(Process process, Func<double> randFunc, ISimulationQueue? queue = null)
    {
        Process = process;
        Process.Channels.Add(this);
        RandFunc = randFunc;
        StateQueue = queue;
    }
    public bool TryStart(double startTime, Client client)
    {
        Statistic.TotalCount++;
        if (Client == null)
        {
            Start(startTime, client);
            return true;
        }
        return false;
    }
    public bool TryAddToQueue(Client client)
    {
        if (StateQueue!=null && StateQueue.Count < StateQueue.MaxSize)
        {
            Statistic.AdditionsToQueueCount++;
            StateQueue.Enqueue(client);
            return true;
        }
        Statistic.FailsCount++;
        client.OnFail();
        return false;
    }
    private void Start(double startTime, Client client)
    {
        Statistic.StartsCount++;
        Client = client;
        _startTime = startTime;
        double workTime = RandFunc();
        _endTime = StartTime + workTime;
        Process.Model.Closest.Enqueue(this, EndTime);
        //
        Process.OnChannelStart();
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void End()
    {
        Client!.OnChannelEnd(StartTime, EndTime);
        Process.OnChannelEnd(StartTime, EndTime);

        Timeline.Add(StartTime, EndTime);
        //Console.WriteLine($"End {State!.Name}");
        foreach (var transition in Process!.Transitions)
        {
            Process? nextState = transition.GetTransitionProcess(Client!);
            if (nextState != null)
            {
                nextState.TryStartChannel(EndTime, Client!);
            }
        }
        //
        if (StateQueue != null && StateQueue.Count > 0)
        {
            Start(EndTime, StateQueue.Dequeue()!);
        }
        else
        {
            Client!.OnDespose(EndTime);
            Client = null;
        }
    }
}
