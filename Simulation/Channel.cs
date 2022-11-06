namespace Simulation;

public class Channel
{
    public State State;
    public Func<double> RandFunc;
    public int QueueMaxSize;
    //
    public int Id;
    public int QueueSize = 0;
    public bool IsBusy = false;
    public double TimeStart;
    public double TimeEnd = -1;
    //
    public Statistic? Statistic;
    public Workload? Workload;
    public Channel(State state, Func<double> randFunc)
    {
        State = state;
        Id = State.Channels.Count;
        State.Channels.Add(this);
        RandFunc = randFunc;
        if(state.Statistic != null)
        {
            Statistic = new();
        }
        if (state.Workload != null)
        {
            Workload = new();
        }
        QueueMaxSize = -1;
    }
    public Channel(State state, Func<double> randFunc, int queueMaxSize)
    {
        State = state;
        State.Channels.Add(this);
        RandFunc = randFunc;
        if (state.Statistic != null)
        {
            Statistic = new();
        }
        if (state.Workload != null)
        {
            Workload = new();
        }
        QueueMaxSize = queueMaxSize;
    }
    public bool TryStart(double timeStart)
    {
        if (Statistic != null) Statistic.TotalCount++;
        if (IsBusy == false)
        {
            Start(timeStart);
            return true;
        }
        return false;
    }
    public bool TryAddToQueue()
    {
        if (QueueMaxSize <= -1 || QueueSize < QueueMaxSize)
        {
            QueueSize++;
            return true;
        }
        if (Statistic != null) Statistic.FailCount++;
        return false;
    }
    private void Start(double timeStart)
    {
        IsBusy = true;
        TimeStart = timeStart;
        double workTime = RandFunc();
        TimeEnd = TimeStart + workTime;
        State.Model.Closest.Enqueue(this, TimeEnd);
        if(Workload!=null) Workload.AddToWorkload(timeStart, TimeEnd);
        if(State.Workload != null) State.Workload.AddToWorkload(timeStart, TimeEnd);
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void End()
    {
        //Console.WriteLine($"End {State!.Name}");
        foreach (var transition in State!.Transitions)
        {
            State? nextState = transition.GetTransitionState();
            if (nextState != null)
            {
                nextState.TryStartChannel(TimeEnd);
            }
        }
        //
        if (QueueSize > 0)
        {
            QueueSize--;
            Start(TimeEnd);
        }
        else
        {
            IsBusy = false;
        }
    }
}
