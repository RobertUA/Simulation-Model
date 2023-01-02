namespace Simulation;

public class Channel : ITimeEvent
{
    public Process Process;
    public Func<Client, double> RandFunc;
    //
    public int Id;
    public SimulationQueueBase? Queue;
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
    public Channel(Process process, Func<Client, double> randFunc, SimulationQueueBase? queue = null)
    {
        Process = process;
        Id = Process.Channels.Count;
        Process.Channels.Add(this);
        RandFunc = randFunc;
        Queue = queue;
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
        if (Queue != null && Queue.TryEnqueue(client))
        {
            //Statistic.AdditionsToQueueCount++;
            double startTime = Queue.Timeline.LastSegment.EndTime;
            for (int i = 0; i < Queue.Count-1; i++)
            {
                Queue.Timeline.Add(startTime, _endTime);
                Timeline.Add(startTime, _endTime);
                Process.Timeline.Add(startTime, _endTime);
                Process.Model.Timeline.Add(startTime, _endTime);
            }
            return true;
        }
        Statistic.FailsCount++;
        client.OnFail();
        return false;
    }
    public void Start(double startTime, double endTime, Client client)
    {
        Statistic.SuccessCount++;
        Client = client;
        _startTime = startTime;
        _endTime = endTime;
        Process.Model.Closest.Enqueue(this, EndTime);
        //
        Process.OnChannelStart();
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void Start(double startTime, Client client)
    {
        double time = RandFunc(client);
        if (time <= 0) time = double.Epsilon;
        Start(startTime, startTime + time, client);
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
        if (Queue != null && Queue.Count > 0)
        {
            double lastQueueStartTime = Queue.Timeline.LastSegment.EndTime;
            for (int i = 0; i < Queue.Count; i++)
            {
                Queue.Timeline.Add(lastQueueStartTime, _endTime);
            }

            Client client = Queue.Dequeue()!;
            Start(EndTime, client);
        }
        else
        {
            Client!.OnDespose(EndTime);
            Client = null;
        }
    }
}
