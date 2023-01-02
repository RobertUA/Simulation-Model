namespace Simulation;

public class Model
{
    public HashSet<Client> Clients = new();
    public HashSet<Process> Processes = new();
    public PriorityQueue<ITimeEvent, double> Closest = new();
    public Statistic Statistic = new Statistic();
    public Timeline Timeline = new Timeline();
    private double _currentTime;
    private ITimeEvent? _currentChannel;
    public double CurrentTime => _currentTime;
    public void Simulate(double totalTime, bool printSteps=false)
    {
        _currentChannel = Closest.Dequeue();
        _currentTime = _currentChannel.EndTime;
        while (_currentTime < totalTime)
        {
            _currentChannel.End();
            if(printSteps) PrintStepInfo();

            _currentChannel = Closest.Dequeue();
            _currentTime = _currentChannel.EndTime;
        }
        //PrintEndInfo();
    }
    public void PrintEndInfo()
    {
        Console.WriteLine("------------------------ [END] ------------------------");
        foreach (var process in Processes)
        {
            if (process.Statistic != null)
            {
                double failChance = (double)process.Statistic.FailsCount / process.Statistic.TotalCount;
                double workloadPercent = process.Timeline.WorkloadTime / process.Timeline.TotalTime;
                Console.WriteLine($"==== {process.Name} stats: {failChance} ({process.Statistic.FailsCount}/{process.Statistic.TotalCount})" +
                    $"\tWorkload: {workloadPercent} ({process.Timeline.WorkloadTime} / {process.Timeline.TotalTime})");

                if (process.Channels.Count > 1)
                {
                    foreach (var channel in process.Channels)
                    {
                        failChance = (double)channel.Statistic.FailsCount / channel.Statistic.TotalCount;
                        workloadPercent = channel.Timeline.WorkloadTime / channel.Timeline.TotalTime;
                        Console.WriteLine($"Channel {channel.Id}) stats: {failChance} ({channel.Statistic!.FailsCount}/{channel.Statistic.TotalCount})" +
                        $"\tWorkload: {workloadPercent} ({channel.Timeline.WorkloadTime} / {channel.Timeline.TotalTime})");
                    }
                }
            }
        }
    }
    public void PrintStepInfo()
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
                Console.WriteLine($"{state.Name} | Channels: [{string.Join(';', state.Channels.Select(x => x.Client!=null ? x.Client.Type : 0))}] | Queues: [{string.Join(';', state.Channels.Select(x => x.Queue != null ? x.Queue.Count : 0))}]");
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