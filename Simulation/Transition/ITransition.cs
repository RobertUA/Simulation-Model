namespace Simulation;

public interface ITransition
{
    public Process? GetTransitionProcess(Client client);
}
