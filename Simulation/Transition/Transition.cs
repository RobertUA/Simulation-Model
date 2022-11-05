namespace Simulation;

public class Transition : ITransition
{
    private State _state;
    public Transition(State state)
    {
        _state = state;
    }
    public State? GetTransitionState()
    {
        return _state;
    }
}