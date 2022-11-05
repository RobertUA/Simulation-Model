namespace Simulation;

public class State
{
    public string Name;
    public Model Model;
    public List<Channel> Channels = new List<Channel>();
    public readonly List<Transition> Transitions = new List<Transition>();
    public State(Model model, string name)
    {
        Model = model;
        Model.State.Add(this);
        Name = name;
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
