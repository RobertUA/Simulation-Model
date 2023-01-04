using Simulation;

public class TransitionFirst : TransitionBase
{
    private readonly TransitionBase[] _transitions;
    public TransitionFirst(params TransitionBase[] transitions)
    {
        _transitions = transitions;
    }
    public override Process? GetTransitionProcess(Client client)
    {
        foreach (var transition in _transitions)
        {
            Process? process = transition.GetTransitionProcess(client);
            if (process != null) return process;
        }
        return null;
    }
}