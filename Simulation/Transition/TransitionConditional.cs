namespace Simulation;

public class TransitionConditional : ITransition
{
    private readonly (Process? process, Func<Client, bool> condition)[] _conditionalProcesss; 
    public TransitionConditional(params (Process? Process, Func<Client, bool> condition)[] conditionalProcesss)
    {
        _conditionalProcesss = conditionalProcesss;
    }
    public Process? GetTransitionProcess(Client client)
    {
        for (int i = 0; i < _conditionalProcesss.Length; i++)
        {
            if (_conditionalProcesss[i].condition.Invoke(client)) return _conditionalProcesss[i].process;
        }
        return null;
    }
}
