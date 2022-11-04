namespace Simulation;

public class Model
{
    public HashSet<StateEvent> StateEvents = new HashSet<StateEvent>();
    public PriorityQueue<Channel, double> Closest = new PriorityQueue<Channel, double>();
    public void Simulate(double totalTime)
    {
        Channel nextChanell = Closest.Dequeue();
        double currentTime = nextChanell.TimeEnd;
        while(currentTime < totalTime)
        {
            Console.WriteLine($"\n------------------------ [{currentTime}] ------------------------");
            nextChanell.End();

            PrintAllStates();

            if (Closest.Count == 0)
            {
                Console.WriteLine("[!] Break [!] No transitions found");
                break;
            }
            nextChanell = Closest.Dequeue();
            currentTime = nextChanell.TimeEnd;
        }
    }
    public void PrintAllStates()
    {
        if (StateEvents == null) return;

        foreach (var state in StateEvents)
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