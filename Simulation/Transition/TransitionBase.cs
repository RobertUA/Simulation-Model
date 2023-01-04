namespace Simulation;

public abstract class TransitionBase
{
    public Func<double>? RandDelay;
    public TransitionBase(Func<double>? randDelay = null)
    {
        RandDelay = randDelay;
    }

    public abstract Process? GetTransitionProcess(Client client);
}
