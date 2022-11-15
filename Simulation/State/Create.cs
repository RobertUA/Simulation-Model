namespace Simulation;

public class Create : State, ITimeEvent
{
    private double _startTime;
    private double _endTime = -1;
    private readonly int _clientType;
    private readonly Func<double> _randFunc;
    public double EndTime
    {
        get { return _endTime; }
    }
    public double StartTime
    {
        get { return _startTime; }
    }
    public string Info
    {
        get { return $"{Name} | Client: {_clientType}"; }
    }
    public Create(Model model, string name, Func<double> randFunc, int clientType=1) : base(name, model)
    {
        _randFunc = randFunc;
        _clientType = clientType;
    }
    public void Start(double startTime)
    {
        _startTime = startTime;
        double workTime = _randFunc();
        _endTime = StartTime + workTime;
        Model.Closest.Enqueue(this, EndTime);
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void End()
    {
        Start(EndTime);
        Client newClient = new (EndTime, _clientType);
        foreach (var transition in Transitions)
        {
            Process? nextState = transition.GetTransitionProcess();
            if (nextState != null)
            {
                nextState.TryStartChannel(EndTime, newClient);
            }
        }
    }
}
