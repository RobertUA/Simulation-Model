namespace Simulation;

public class Model
{
    private readonly StateEvent _creator;
    private readonly StateEvent[] _processes;
    public Model(StateEvent creator, params StateEvent[] processes)
    {
        _creator = creator;
        _processes = processes;
    }
    public void Simulate(float totalTime)
    {
        float currentTime = 0;
        _creator.Start(0);
        while (currentTime < totalTime && Channel.Closest.Count > 0)
        {
            Channel nextChanell = Channel.Closest.Dequeue();
            nextChanell.End();
            //nextState.CurrentChanell.End();
        }
    }
}