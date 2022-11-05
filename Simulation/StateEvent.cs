namespace Simulation;

public class State : StatisticObject
{
    public string Name;
    public Model Model;
    public List<Channel> Channels = new List<Channel>();
    public readonly List<Transition> Transitions = new List<Transition>();
    private Comparison<Channel>? _channelComparison = null;
    private bool _noQueuePriotity;
    private bool _autoBalanceQueue;
    public State(Model model, string name, Comparison<Channel>? channelComparer = null, bool noQueuePriotity = true, bool autoBalanceQueue=false)
    {
        Model = model;
        Model.States.Add(this);
        Name = name;
        _channelComparison = channelComparer;
        _noQueuePriotity = noQueuePriotity;
        _autoBalanceQueue = autoBalanceQueue;
    }
    public bool TryStartChannel(double timeStart)
    {
        TotalCount++;
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
        FailCount++;
        return false;
    }
}
