namespace Simulation;

public class Channel
{
    public static PriorityQueue <Channel, double> Closest = new PriorityQueue<Channel, double>();
    public Channel? ParentChanell;
    public Channel[]? SubChanells;
    public StateEvent? State;
    public Queue<StateEvent> Queue = new Queue<StateEvent>();
    public int QueueSize;
    public double TimeStart;
    public double TimeEnd = -1;
    public Channel()
    {
        SubChanells = null;
        QueueSize = -1;
    }
    public Channel(Channel[] subChanells)
    {
        SubChanells = subChanells;
        QueueSize = -1;
    }
    public Channel(int queueSize)
    {
        SubChanells = null;
        QueueSize = queueSize;
    }
    public Channel(int queueSize, Channel[] subChanells)
    {
        SubChanells = subChanells;
        QueueSize = queueSize;
    }
    public bool TryAdd(double timeStart, StateEvent state)
    {
        if(SubChanells != null)
        {
            foreach (var subChanell in SubChanells)
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
            if(State != null)
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
        else if (ParentChanell != null)
        {
            return ParentChanell.TryAddToQueue(state);
        }
        return false;
    }
    private void Start(double timeStart, StateEvent state)
    {
        State = state;
        TimeStart = timeStart;
        TimeEnd = State.RandFunc();
        Closest.Enqueue(this, 1 / TimeEnd);
    }
    public void End()
    {
        if(State!.Transitions != null)
        {
            StateEvent? nextState = State.Transitions[0]!.state;
            if(nextState != null)
            {
                nextState.Start(TimeEnd);
            }
        }
        //
        if (Queue.Count > 0)
            Start(TimeEnd, Queue.Dequeue());
        else
        {
            State = null;
            TimeEnd = -1;
        }
    }
}
