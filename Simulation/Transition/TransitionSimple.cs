namespace Simulation;

public class TransitionSimple : TransitionBase
{
    private readonly Process _state;
    public TransitionSimple(Process state, Func<double>? randDelay = null) : base(randDelay)
    {
        _state = state;
    }
    public override Process? GetTransitionProcess(Client client)
    {
        return _state;
    }
}