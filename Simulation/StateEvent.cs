namespace Simulation;

public class StateEvent
{
    public Channel MainChanell;
    public Func<double> RandFunc;
    public (StateEvent? state, double weight)[]? Transitions;
    public StateEvent(Channel mainChanell, Func<double> randFunc, params (StateEvent? state, double weight)[]? transitions)
    {
        MainChanell = mainChanell;
        RandFunc = randFunc;
        Transitions = transitions;
    }
    public void Start(double timeStart, Channel? chanell=null)
    {
        if (chanell == null) chanell = MainChanell;
        chanell.TryAdd(timeStart, this);
    }
}
