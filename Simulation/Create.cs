namespace Simulation;

public class Create 
{
    public Model Model;
    public Func<double> RandFunc;
    public (StateEvent state, double weight)[]? Transitions;
    public Create(Model model, Func<double> randFunc, (StateEvent state, double weight)[]? transitions)
    {
        Model = model;
        RandFunc = randFunc;
        Transitions = transitions;
    }
}