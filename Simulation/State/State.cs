namespace Simulation;

public abstract class State
{
    public string Name;
    public Model Model;
    public readonly List<ITransition> Transitions = new();
    public State(string name, Model model)
    {
        Name = name;
        Model = model;
    }
}
