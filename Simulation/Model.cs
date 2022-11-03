namespace Simulation;

public class Model
{
    public PriorityQueue<Channel, double> Closest = new PriorityQueue<Channel, double>();
    public void Simulate(double totalTime)
    {
        Channel nextChanell = Closest.Dequeue();
        double currentTime = nextChanell.TimeEnd;
        while(currentTime < totalTime)
        {
            Console.WriteLine($"\n------------------------ [{currentTime}] ------------------------");
            nextChanell.End();
            if (Closest.Count == 0)
            {
                Console.WriteLine("[!] Break [!] No transitions found");
                break;
            }
            nextChanell = Closest.Dequeue();
            currentTime = nextChanell.TimeEnd;
        }
    }
}