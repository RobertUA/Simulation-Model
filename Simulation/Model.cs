namespace Simulation;

public class Model
{
    public HashSet<State> States = new();
    public PriorityQueue<Channel, double> Closest = new();
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
        Console.WriteLine("------------------------ [END] ------------------------");
        foreach (var state in States)
        {
            if (state.Statistic != null)
            {
                double failChance = (double)state.Statistic.FailCount / state.Statistic.TotalCount;
                Console.WriteLine($"{state.Name} stats: {failChance} ({state.Statistic.FailCount}/{state.Statistic.TotalCount}) | [{string.Join(", ", state.Channels.Select(x => $"{x.Statistic!.FailCount}/{x.Statistic.TotalCount}"))}]");
            }
        }
    }
    public void PrintInfo()
    {
        if (States == null) return;

        Console.WriteLine($"------------------------ [{_currentTime}] ------------------------");
        Console.WriteLine($"Current state and channel: {_currentChannel!.State.Name} ({_currentChannel.Id})");

        //Console.WriteLine($"{_currentChannel!.State.Name} - [{string.Join(';', _currentChannel!.State.Channels.Select(x => x.IsBusy ? 1 : 0))}]");
        //Console.WriteLine($"Queues: [{string.Join(';', _currentChannel!.State.Channels.Select(x => x.QueueSize))}]");
        foreach (var state in States)
        {
            if (state.Statistic != null)
            {
                Console.WriteLine($"{state.Name} | Channels: [{string.Join(';', state.Channels.Select(x => x.IsBusy ? 1 : 0))}] | Queues: [{string.Join(';', state.Channels.Select(x => x.QueueSize))}]");
                //Console.Write($"{state.Name} - [");
                //foreach (var channel in state.Channels)
                //{
                //    Console.Write($"({channel.QueueSize}){(channel.IsBusy ? 1 : 0)}");
                //}
                //Console.WriteLine("]");
            }
        }
    }
}