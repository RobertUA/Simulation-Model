namespace Simulation;

public class Create : State, ITimeEvent
{
    private double _startTime;
    private double _endTime = -1;
    private readonly Func<double> _randFunc;
    public readonly Client Client;
    public double EndTime => _endTime;
    public double StartTime => _startTime;
    public string Info
    {
        get { return $"{Name} | Client: {Client.Type}"; }
    }
    public Create(Model model, string name, Func<double> randFunc, Client? client = null): base(name, model)
    {
        _randFunc = randFunc;
        Client = client ?? new Client(1);
        Client.Create = this;
    }
    public void Start(double startTime, double endTime)
    {
        _startTime = startTime;
        _endTime = endTime;
        Model.Closest.Enqueue(this, _endTime);
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void Start(double startTime)
    {
        Start(startTime,startTime + _randFunc());
    }
    public void End()
    {
        Start(_endTime);
        Client newClient = new (Client);
        newClient.OnCreate(_endTime);
        Model.Clients.Add(newClient);
        foreach (var transition in Transitions)
        {
            Process? nextState = transition.GetTransitionProcess(newClient);
            if (nextState != null)
            {
                nextState.TryStartChannel(_endTime, newClient);
            }
        }
    }
}
