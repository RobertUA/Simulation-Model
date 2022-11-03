namespace Simulation;

public class Channel
{
    public Model Model;
    public Channel? ParentChannel = null;
    public Channel[]? SubChannels = null;
    public StateEvent? State = null;
    public Func<double> RandFunc;
    public Queue<StateEvent> Queue = new Queue<StateEvent>();
    public int QueueSize;
    public double TimeStart;
    public double TimeEnd = -1;
    public Channel(Model model, Func<double> randFunc)
    {
        Model = model;
        RandFunc = randFunc;
        QueueSize = -1;
    }
    public Channel(Model model, Func<double> randFunc, int queueSize)
    {
        Model = model;
        RandFunc = randFunc;
        QueueSize = queueSize;
    }
    public void SetSubChannels(params Channel[] subChannels)
    {
        SubChannels = subChannels;
        foreach (var subChannel in subChannels)
        {
            subChannel.ParentChannel = this;
        }
    }
    public bool TryAdd(double timeStart, StateEvent state)
    {
        if(SubChannels != null)
        {
            foreach (var subChanell in SubChannels)
            {
                if (subChanell.TryAdd(timeStart, state))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            if(State == null)
            {
                Start(timeStart, state);
                return true;
            }
            else
            {
                if (TryAddToQueue(state))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool TryAddToQueue(StateEvent state)
    {
        if(QueueSize <= -1 || Queue!.Count < QueueSize)
        {
            Queue!.Enqueue(state);
            return true;
        }
        else if (ParentChannel != null)
        {
            return ParentChannel.TryAddToQueue(state);
        }
        return false;
    }
    private void Start(double timeStart, StateEvent state)
    {
        State = state;
        TimeStart = timeStart;
        TimeEnd = TimeStart + RandFunc();
        Model.Closest.Enqueue(this, TimeEnd);
        Console.WriteLine($"Start {State.Name} (must end at: {TimeEnd})");
    }
    public void End()
    {
        Console.WriteLine($"End {State!.Name}");
        if (State!.Transitions != null)
        {
            StateEvent? nextState = State.Transitions[0]!.state;
            if(nextState != null)
            {
                nextState.Start(TimeEnd);
            }
        }
        //

        if (State!.Repeat)
        {
            Start(TimeEnd, State!);
        }
        else if (Queue.Count > 0)
            Start(TimeEnd, Queue.Dequeue());
        else
        {
            State = null;
        }
    }
}
