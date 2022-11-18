namespace Simulation;

public class Process : State
{
    public List<Channel> Channels = new();
    public readonly List<ITransition> FailTransitions = new();
    public Statistic Statistic = new();
    public Timeline Timeline = new();
    private Action<Process>? _beforeAction = null;
    public Process(Model model, string name) : base(name, model)
    {
        Model = model;
        Model.Processes.Add(this);
        Name = name;
    }
    public void SetBeforeAction(Action<Process> beforeAction)
    {
        _beforeAction = beforeAction;
    }
    public bool TryStartChannel(double startTime, Client client)
    {
        Statistic.TotalCount++;
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
        //Process Queue TODO
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
        Statistic.StartsCount++;
    }
    public void OnChannelEnd(double startTime, double endTime)
    {
        Timeline.Add(startTime, endTime);
    }
}
