namespace Simulation;

public class TransitionSimple : ITransition
{
    private readonly State _state;
    public TransitionSimple(State state)
    {
        _state = state;
    }
    public State? GetTransitionState()
    {
        return _state;
    }
}