public class Chanell
{
    public Chanell? ParentChanell;
    public Chanell[]? SubChanells;
    public StateEvent? State;
    public Queue<StateEvent>? Queue;
    public int QueueSize;
    public Chanell(int queueSize = -1)
    {
        SubChanells = null;
        SetQueueSize(queueSize);
    }
    public Chanell(Chanell[]? subChanells, int queueSize = -1)
    {
        SubChanells = subChanells;
        SetQueueSize(queueSize);
    }
    private void SetQueueSize(int queueSize)
    {
        if (queueSize == 0) 
            Queue = null;
        else
            Queue = new Queue<StateEvent>();
    }
    public bool TryAdd(StateEvent state)
    {
        if(SubChanells != null)
        {
            foreach (var subChanell in SubChanells)
            {
                if (subChanell.TryAdd(state))
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
                Start(state);
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
    public void Start(StateEvent state)
    {
        State = state;
    }
    public void End()
    {
        // transitions, queue
        State = null;
    }
}
