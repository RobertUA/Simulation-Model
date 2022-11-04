namespace Simulation;

public class StateEvent
{
    public string Name;
    public Model Model;
    public List<Channel> Channels = new List<Channel>();
    public bool Repeat;
    public (StateEvent? state, double weight)[]? Transitions;
    public StateEvent(Model model, string name, bool repeat)
    {
        Model = model;
        Model.StateEvents.Add(this);
        Name = name;
        Repeat = repeat;
    }
    public void SetTransitions(params (StateEvent? state, double weight)[]? transitions)
    {
        Transitions = transitions;
    }
    public bool TryStart(double timeStart)
    {
        foreach (var channel in Channels)
        {
            if(channel.TryStart(timeStart))
            {
                return true;
            }
        }
        foreach (var channel in Channels)
        {
            if (channel.TryAddToQueue())
            {
                return true;
            }
        }
        return false;
    }
}
