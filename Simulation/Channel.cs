namespace Simulation;

public class Channel
{
    public State State;
    public Func<double> RandFunc;
    //
    public int Id;
    public ISimulationQueue? StateQueue;
    public State? Creator = null;
    public double TimeStart;
    public double TimeEnd = -1;
    //
    public Statistic? Statistic;
    public Workload? Workload;
    public Channel(State state, Func<double> randFunc, ISimulationQueue? queue = null)
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
        StateQueue = queue;
    }
    public bool TryStart(double timeStart, State creator)
    {
        if (Statistic != null) Statistic.TotalCount++;
        if (Creator == null)
        {
            Start(timeStart, creator);
            return true;
        }
        return false;
    }
    public bool TryAddToQueue(State creator)
    {
        if (StateQueue!=null && StateQueue.Count < StateQueue.MaxSize)
        {
            StateQueue.Enqueue(creator);
            return true;
        }
        if (Statistic != null) Statistic.FailCount++;
        return false;
    }
    private void Start(double timeStart, State creator)
    {
        Creator = creator;
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
                nextState.TryStartChannel(TimeEnd, Creator!);
            }
        }
        //
        if (StateQueue != null && StateQueue.Count > 0)
        {
            Start(TimeEnd, StateQueue.Dequeue()!);
        }
        else
        {
            Creator = null;
        }
    }
}
