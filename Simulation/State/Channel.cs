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
            //double startTime = Queue.Timeline.LastSegment.EndTime;
            //for (int i = 0; i < Queue.Count-1; i++)
            //{
            //    Queue.Timeline.Add(startTime, _endTime);
            //    Timeline.Add(startTime, _endTime);
            //    Process.Timeline.Add(startTime, _endTime);
            //    Process.Model.Timeline.Add(startTime, _endTime);
            //}
            client.InQueue = true;
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
        Client.LastProccess = Process;
        _startTime = startTime;
        _endTime = endTime;
        Client.OnChannelStart(_startTime, _endTime);
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
        Client!.OnChannelEnd(StartTime, _endTime);
        Process.OnChannelEnd(StartTime, _endTime);

        Timeline.Add(StartTime, EndTime);
        //Console.WriteLine($"End {State!.Name}");
        bool transited = false;
        foreach (var transition in Process!.Transitions)
        {
            Process? nextState = transition.GetTransitionProcess(Client!);
            if (nextState != null)
            {
                double delay = transition.RandDelay != null ? transition.RandDelay.Invoke() : 0;
                if (delay < 0) delay = 0;
                if (nextState.TryStartChannel(_endTime + delay, Client!))
                {
                    transited = true;
                }
            }
        }
        //
        if (transited == false)
        {
            Client?.OnDespose(_endTime);
        }
        Client = null;
        if (Queue != null && Queue.Count > 0)
        {
            double lastQueueStartTime = Queue.Timeline.LastSegment.EndTime;
            for (int i = 0; i < Queue.Count; i++)
            {
                Queue.Timeline.Add(lastQueueStartTime, _endTime);
            }

            Client client = Queue.Dequeue()!;
            client.InQueue = false;
            Start(EndTime, client);
        }
    }
}
