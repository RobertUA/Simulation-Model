namespace Simulation;

public class TransitionConditional : ITransition
{
    private readonly (State? state, Func<bool> condition)[] _conditionalStates; 
    public TransitionConditional(params (State? state, Func<bool> condition)[] conditionalStates)
    {
        _conditionalStates = conditionalStates;
    }
    public State? GetTransitionState()
    {
        for (int i = 0; i < _conditionalStates.Length; i++)
        {
            if (_conditionalStates[i].condition.Invoke()) return _conditionalStates[i].state;
        }
        return null;
    }
}
