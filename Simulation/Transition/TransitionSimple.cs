namespace Simulation;

public class TransitionSimple : ITransition
{
    private readonly Process _state;
    public TransitionSimple(Process state)
    {
        _state = state;
    }
    public Process? GetTransitionProcess()
    {
        return _state;
    }
}