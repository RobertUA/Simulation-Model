namespace Simulation;

public class Channel
{
    public StateEvent State;
    public Func<double> RandFunc;
    public int QueueMaxSize;
    //
    public int QueueSize=0;
    public bool IsBusy = false;
    public double TimeStart;
    public double TimeEnd = -1;
    public Channel(StateEvent state, Func<double> randFunc)
    {
        State = state;
        State.Channels.Add(this);
        RandFunc = randFunc;
        QueueMaxSize = -1;
    }
    public Channel(StateEvent state, Func<double> randFunc, int queueMaxSize)
    {
        State = state;
        State.Channels.Add(this);
        RandFunc = randFunc;
        QueueMaxSize = queueMaxSize;
    }
    public bool TryStart(double timeStart)
    {
        if(IsBusy == false)
        {
            Start(timeStart);
            return true;
        }
        return false;
    }
    public bool TryAddToQueue()
    {
        if (QueueMaxSize <= -1 || QueueSize < QueueMaxSize)
        {
            QueueSize++;
            return true;
        }
        return false;
    }
    private void Start(double timeStart)
    {
        IsBusy = true;
        TimeStart = timeStart;
        TimeEnd = TimeStart + RandFunc();
        State.Model.Closest.Enqueue(this, TimeEnd);
        //Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void End()
    {
        //Console.WriteLine($"End {State!.Name}");
        if (State!.Transitions != null)
        {
            StateEvent? nextState = State.Transitions[0]!.state;
            if(nextState != null)
            {
                nextState.TryStart(TimeEnd);
            }
        }
        //

        if (State!.Repeat)
        {
            Start(TimeEnd);
        }
        else if (QueueSize > 0)
        {
            QueueSize--;
            Start(TimeEnd);
        }
        else
        {
            IsBusy = false;
        }
    }
}
