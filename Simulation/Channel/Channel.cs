using System.Xml.Linq;

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
    public ChannelStatistic? Statistic;
    public Timeline? Workload;
    //
    private double _startTime;
    private double _endTime = -1;
    public string Info
    {
        get { return $"Channel {Id} of {Process.Name} | Client: {(Client != null ? Client.Type : "null")}"; }
    }
    public double EndTime
    {
        get { return _endTime; }
    }
    public double StartTime
    {
        get { return _startTime; }
    }
    public Channel(Process process, Func<double> randFunc, ISimulationQueue? queue = null)
    {
        Process = process;
        Process.Channels.Add(this);
        RandFunc = randFunc;
        if (process.Statistic != null)
        {
            Statistic = new();
        }
        if (process.Workload != null)
        {
            Workload = new();
        }
        StateQueue = queue;
    }
    public bool TryStart(double startTime, Client client)
    {
        if (Statistic != null) Statistic.TotalCount++;
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
            StateQueue.Enqueue(client);
            return true;
        }
        if (Statistic != null) Statistic.FailCount++;
        return false;
    }
    private void Start(double startTime, Client client)
    {
        Client = client;
        _startTime = startTime;
        double workTime = RandFunc();
        _endTime = StartTime + workTime;
        Process.Model.Closest.Enqueue(this, EndTime);
        if(Workload!=null) Workload.Add(startTime, EndTime);
        if(Process.Workload != null) Process.Workload.Add(startTime, EndTime);
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void End()
    {
        //Console.WriteLine($"End {State!.Name}");
        foreach (var transition in Process!.Transitions)
        {
            Process? nextState = transition.GetTransitionProcess();
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
            Client!.Despose(EndTime);
            Client = null;
        }
    }
}
