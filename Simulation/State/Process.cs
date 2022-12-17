namespace Simulation;

public class Process : State
{
    public List<Channel> Channels = new();
    public readonly List<ITransition> FailTransitions = new();
    public Statistic Statistic = new();
    public Timeline Timeline = new();

    private Action? _beforeAction = null;
    public Process(Model model, string name) : base(name, model)
    {
        Model = model;
        Model.Processes.Add(this);
        Name = name;
    }
    public void SetBeforeAction(Action beforeAction)
    {
        _beforeAction = beforeAction;
    }
    public bool TryStartChannel(double startTime, Client client)
    {
        Statistic.TotalCount++;
        if (_beforeAction != null)
        {
            _beforeAction.Invoke();
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
        Statistic.FailsCount++;
        foreach (var failTransition in FailTransitions)
        {
            Process? nextState = failTransition.GetTransitionProcess(client);
            if(nextState!=null) nextState.TryStartChannel(startTime, client);
        }
        return false;
    }
    //
    public void OnChannelStart()
    {
        Statistic.SuccessCount++;
    }
    public void OnChannelEnd(double startTime, double endTime)
    {
        Timeline.Add(startTime, endTime);
    }
}
