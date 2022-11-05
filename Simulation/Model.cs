namespace Simulation;

public class Model
{
    public HashSet<State> States = new HashSet<State>();
    public PriorityQueue<Channel, double> Closest = new PriorityQueue<Channel, double>();
    private double _currentTime;
    private Channel? _currentChannel;
    public void Simulate(double totalTime)
    {
        _currentChannel = Closest.Dequeue();
        _currentTime = _currentChannel.TimeEnd;
        while (_currentTime < totalTime)
        {
            _currentChannel.End();
            PrintInfo();

            _currentChannel = Closest.Dequeue();
            _currentTime = _currentChannel.TimeEnd;
        }
    }
    public void PrintInfo()
    {
        if (States == null) return;

        Console.WriteLine($"------------------------ [{_currentTime}] ------------------------");
        Console.WriteLine($"Current state (channel): {_currentChannel!.State.Name} ({_currentChannel.Id})");

        //Console.WriteLine($"{_currentChannel!.State.Name} - [{string.Join(';', _currentChannel!.State.Channels.Select(x => x.IsBusy ? 1 : 0))}]");
        //Console.WriteLine($"Queues: [{string.Join(';', _currentChannel!.State.Channels.Select(x => x.QueueSize))}]");
        foreach (var state in States)
        {
            Console.WriteLine($"{state.Name} - [{string.Join(';', state.Channels.Select(x => x.IsBusy ? 1 : 0))}]");
            Console.WriteLine($"Queues: [{string.Join(';', state.Channels.Select(x => x.QueueSize))}]");
            //Console.Write($"{state.Name} - [");
            //foreach (var channel in state.Channels)
            //{
            //    Console.Write($"({channel.QueueSize}){(channel.IsBusy ? 1 : 0)}");
            //}
            //Console.WriteLine("]");
        }
    }
}