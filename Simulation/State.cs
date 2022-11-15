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
    private Action<State>? _beforeAction = null;
    
    public State(Model model, string name, bool statistic, bool workload)
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
    }
    public void SetBeforeAction(Action<State> beforeAction)
    {
        _beforeAction = beforeAction;
    }
    public bool TryStartChannel(double timeStart, State creator)
    {
        if(Statistic!=null) Statistic.TotalCount++;
        if (_beforeAction != null)
        {
            _beforeAction.Invoke(this);
        }
        foreach (var channel in Channels)
        {
            if (channel.TryStart(timeStart, creator))
            {
                return true;
            }
            else if (channel.TryAddToQueue(creator))
            {
                return true;
            }
        }
        if (Statistic != null) Statistic.FailCount++;
        foreach (var failTransition in FailTransitions)
        {
            State? nextState = failTransition.GetTransitionState();
            if(nextState!=null) nextState.TryStartChannel(timeStart, creator);
        }
        return false;
    }
}
