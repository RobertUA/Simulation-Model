namespace Simulation;

public class State
{
    public string Name;
    public Model Model;
    public List<Channel> Channels = new();
    public readonly List<ITransition> Transitions = new();
    public readonly List<ITransition> FailTransitions = new();
    public Statistic? Statistic;
    public Workload? Workload;
    //
    private readonly Comparison<Channel>? _channelComparison = null;
    private readonly bool _noQueuePriotity;
    private readonly bool _autoBalanceQueue;
    
    public State(Model model, string name, bool statistic, bool workload, Comparison<Channel>? channelComparer = null, bool noQueuePriotity = true, bool autoBalanceQueue=false)
    {
        Model = model;
        Model.States.Add(this);
        Name = name;
        if (statistic)
        {
            Statistic = new();
        }
        if (workload)
        {
            Workload = new();
        }
        _channelComparison = channelComparer;
        _noQueuePriotity = noQueuePriotity;
        _autoBalanceQueue = autoBalanceQueue;
    }
    public bool TryStartChannel(double timeStart)
    {
        if(Statistic!=null) Statistic.TotalCount++;
        if (_autoBalanceQueue)
        {
            // TODO
        }
        if (_channelComparison != null) Channels.Sort(_channelComparison);
        foreach (var channel in Channels)
        {
            if(channel.TryStart(timeStart))
            {
                return true;
            }
            else if (_noQueuePriotity == false)
            {
                if (channel.TryAddToQueue())
                {
                    return true;
                }
            }
        }
        if (_noQueuePriotity == true)
        {
            foreach (var channel in Channels)
            {
                if (channel.TryAddToQueue())
                {
                    return true;
                }
            }
        }
        if (Statistic != null) Statistic.FailCount++;
        foreach (var failTransition in FailTransitions)
        {
            State? nextState = failTransition.GetTransitionState();
            if(nextState!=null) nextState.TryStartChannel(timeStart);
        }
        return false;
    }
}
