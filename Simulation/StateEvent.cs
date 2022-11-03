namespace Simulation;

public class StateEvent
{
    public string Name;
    public Channel MainChanell;
    public bool Repeat;
    public (StateEvent? state, double weight)[]? Transitions;
    public StateEvent(string name, Channel mainChanell, bool repeat)
    {
        Name = name;
        MainChanell = mainChanell;
        Repeat = repeat;
    }
    public void SetTransitions(params (StateEvent? state, double weight)[]? transitions)
    {
        Transitions = transitions;
    }
    public void Start(double timeStart, Channel? chanell=null)
    {
        if (chanell == null) chanell = MainChanell;
        chanell.TryAdd(timeStart, this);
    }
}
