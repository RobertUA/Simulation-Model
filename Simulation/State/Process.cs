namespace Simulation;

public class Process : State
{
    public List<Channel> Channels = new();
    public readonly List<ITransition> FailTransitions = new();
    public ChannelStatistic? Statistic;
    public Workload? Workload;
    private Action<Process>? _beforeAction = null;
    public Process(Model model, string name, bool statistic, bool workload) : base(name, model)
    {
        Model = model;
        Model.Processes.Add(this);
        Name = name;
        if (statistic)
        {
            Statistic = new();
        }
        if (workload)
        {
            Workload = new();
        }
    }
    public void SetBeforeAction(Action<Process> beforeAction)
    {
        _beforeAction = beforeAction;
    }
    public bool TryStartChannel(double startTime, Client client)
    {
        if(Statistic!=null) Statistic.TotalCount++;
        if (_beforeAction != null)
        {
            _beforeAction.Invoke(this);
        }
        foreach (var channel in Channels)
        {
            if (channel.TryStart(startTime, client))
            {
                return true;
            }
            else if (channel.TryAddToQueue(client))
            {
                return true;
            }
        }
        if (Statistic != null) Statistic.FailCount++;
        foreach (var failTransition in FailTransitions)
        {
            Process? nextState = failTransition.GetTransitionProcess();
            if(nextState!=null) nextState.TryStartChannel(startTime, client);
        }
        return false;
    }
}
