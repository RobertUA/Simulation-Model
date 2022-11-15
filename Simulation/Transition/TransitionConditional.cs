namespace Simulation;

public class TransitionConditional : ITransition
{
    private readonly (Process? process, Func<bool> condition)[] _conditionalProcesss; 
    public TransitionConditional(params (Process? Process, Func<bool> condition)[] conditionalProcesss)
    {
        _conditionalProcesss = conditionalProcesss;
    }
    public Process? GetTransitionProcess()
    {
        for (int i = 0; i < _conditionalProcesss.Length; i++)
        {
            if (_conditionalProcesss[i].condition.Invoke()) return _conditionalProcesss[i].process;
        }
        return null;
    }
}
