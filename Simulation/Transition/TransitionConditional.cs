namespace Simulation;

public class TransitionConditional : TransitionBase
{
    private readonly (Process? process, Func<Client, bool> condition)[] _conditionalProcesss; 
    public TransitionConditional((Process? Process, Func<Client, bool> condition)[] conditionalProcesss, Func<double>? randDelay = null) : base(randDelay)
    {
        _conditionalProcesss = conditionalProcesss;
    }
    public override Process? GetTransitionProcess(Client client)
    {
        for (int i = 0; i < _conditionalProcesss.Length; i++)
        {
            if (_conditionalProcesss[i].condition.Invoke(client)) return _conditionalProcesss[i].process;
        }
        return null;
    }
}
