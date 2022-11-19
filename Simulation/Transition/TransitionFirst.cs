using Simulation;

public class TransitionFirst : ITransition
{
    private readonly ITransition[] _transitions;
    public TransitionFirst(ITransition[] transitions)
    {
        _transitions = transitions;
    }
    public Process? GetTransitionProcess(Client client)
    {
        foreach (var transition in _transitions)
        {
            Process? process = transition.GetTransitionProcess(client);
            if (process != null) return process;
        }
        return null;
    }
}