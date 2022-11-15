namespace Simulation;

public class Model
{
    public HashSet<Process> Processes = new();
    public PriorityQueue<ITimeEvent, double> Closest = new();
    private double _currentTime;
    private ITimeEvent? _currentChannel;
    public void Simulate(double totalTime)
    {
        _currentChannel = Closest.Dequeue();
        _currentTime = _currentChannel.EndTime;
        while (_currentTime < totalTime)
        {
            _currentChannel.End();
            PrintInfo();

            _currentChannel = Closest.Dequeue();
            _currentTime = _currentChannel.EndTime;
        }
        Console.WriteLine("------------------------ [END] ------------------------");
        foreach (var process in Processes)
        {
            if (process.Statistic != null)
            {
                double failChance = (double)process.Statistic.FailCount / process.Statistic.TotalCount;
                Console.WriteLine($"{process.Name} stats: {failChance} ({process.Statistic.FailCount}/{process.Statistic.TotalCount}) | [{string.Join(", ", process.Channels.Select(x => $"{x.Statistic!.FailCount}/{x.Statistic.TotalCount}"))}]");
            }
        }
    }
    public void PrintInfo()
    {
        if (Processes == null) return;
        Console.WriteLine($"\n------------------------ [{_currentTime}] ------------------------");
        //Console.WriteLine($"Current state and channel: {_currentChannel!.Process.Name} ({_currentChannel.Id})");
        Console.WriteLine($"---[Current: {_currentChannel!.Info}]");

        //Console.WriteLine($"{_currentChannel!.State.Name} - [{string.Join(';', _currentChannel!.State.Channels.Select(x => x.IsBusy ? 1 : 0))}]");
        //Console.WriteLine($"Queues: [{string.Join(';', _currentChannel!.State.Channels.Select(x => x.QueueSize))}]");
        foreach (var state in Processes)
        {
            if (state.Statistic != null)
            {
                Console.WriteLine($"{state.Name} | Channels: [{string.Join(';', state.Channels.Select(x => x.Client!=null ? x.Client.Type : 0))}] | Queues: [{string.Join(';', state.Channels.Select(x => x.StateQueue != null ? x.StateQueue.Count : 0))}]");
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