using Simulation;
using RobRandom;

//Lab1();
//Lab2();
//Lab3();
Nikita();

static void Lab1()
{
    static void WriteValues(StreamWriter streamWriter, string name, int count, Func<double> rand)
    {
        const char separator = ';';
        double[] randValues = new double[count];
        for (int i = 0; i < randValues.Length; i++)
        {
            randValues[i] = rand();
        }
        streamWriter.WriteLine($"{name}{separator} {string.Join(separator, randValues)}");

        Console.WriteLine($"{name} | {count} - DONE");
    }

    StreamWriter streamWriter = new ("DistributionResults.txt");

    Console.WriteLine($"================ {DateTime.Now} ================");
    for (double l = 0.5; l <= 4; l += 0.5f)
    {
        WriteValues(streamWriter, $"Exponential({l})", 10000, () => Distribution.Exponential(l));
    }
    //
    for (double a = 1; a <= 2; a += 0.5f)
    {
        for (double q = 1; q <= 2; q += 0.5f)
        {

            WriteValues(streamWriter, $"Gaus({a}, {q})", 10000, () => Distribution.Gaus(a, q));
        }
    }
    //
    for (double a = 1; a <= 2; a += 0.5f)
    {
        for (double c = 1; c <= 2; c += 0.5f)
        {
            Uniform uniform = new (1, Math.Pow(5, 13), 0, Math.Pow(2, 31));
            WriteValues(streamWriter, $"Uniform ({a}, {c})", 10000, () => uniform.Next());
        }
    }
    //
    Uniform concreteUniform = new (1, Math.Pow(5, 13), 0, Math.Pow(2, 31));
    WriteValues(streamWriter, "Uniform (5^13, 2^31)", 10000, () => concreteUniform.Next());
    //
    streamWriter.Close();
}

static void Lab2()
{
    Model model = new();

    Create create = new(model, "Create", () => Random.Shared.NextDouble());

    Process process1 = new(model, "Process-1");
    process1.SetBeforeAction(() => ChannelsSort(process1));
    Channel process1channel1 = new(process1, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process1channel2 = new(process1, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    Process process2 = new(model, "Process-2");
    process2.SetBeforeAction(() => ChannelsSort(process2));
    Channel process2channel1 = new(process2, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process2channel2 = new(process2, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    Process process3 = new(model, "Process-3");
    process3.SetBeforeAction(() => ChannelsSort(process3));
    Channel process3channel1 = new(process3, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process3channel2 = new(process3, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    create.Transitions.Add(new TransitionSimple(process1));
    process1.Transitions.Add(new TransitionSimple(process2));
    process2.Transitions.Add(new TransitionSimple(process3));
    //process3.Transitions.Add(new TransitionSimple(process2)); // Multy transitions;

    create.Start(0);
    model.Simulate(100000);
}

static void Lab3()
{
    Task2();
    static void Task2()
    {
        Model model = new();

        Create create = new(model, "Create", () => Distribution.Exponential(0.5));
        Process process = new (model, "Process");
        process.SetBeforeAction(() =>
        {
            BalanceQueues(process);
            ChannelsSort(process);
        });
        Channel channel1 = new(process, () => Distribution.Exponential(0.3), new SimpleSimulationQueue(3));
        Channel channel2 = new(process, () => Distribution.Exponential(0.3), new SimpleSimulationQueue(3));

        channel1.Start(0, Distribution.Gaus(1, 0.3), new Client(create.Client));
        channel1.Queue!.Enqueue(new Client(create.Client));
        channel1.Queue!.Enqueue(new Client(create.Client));

        channel2.Start(0, Distribution.Gaus(1, 0.3), new Client(create.Client));
        channel2.Queue!.Enqueue(new Client(create.Client));
        channel2.Queue!.Enqueue(new Client(create.Client));

        create.Transitions.Add(new TransitionSimple(process));

        create.Start(0, 0.1);

        model.Simulate(1000000);
    }
}

static void Nikita()
{
    Model model = new();
    Create create = new(model, "Creator", () => Distribution.Range(5, 15));

    SimpleSimulationQueue abQueue = new(20);
    ConditionalSimulationQueue bc1Queue = new();
    ConditionalSimulationQueue bc2Queue = new();

    int totalBoostCheckCount = 0;
    int boostCount = 0;

    bool BoostCheck()
    {
        totalBoostCheckCount++;
        if (bc1Queue.Count + bc2Queue.Count >= 20)
        {
            boostCount++;
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CanAddToQueue() => bc1Queue.Count + bc2Queue.Count < 25;

    Process ab1 = new(model, "AB1");
    Channel ab1Channel = new(ab1, () => 20, abQueue); 
    Process ab2 = new(model, "AB2");
    Channel ab2Channel = new(ab2, () => Distribution.Range(15, 25), abQueue);

    bc1Queue.SetCondition(CanAddToQueue);
    bc2Queue.SetCondition(CanAddToQueue);

    Process bc1 = new(model, "BC1");
    Channel bc1Channel = new(bc1, () => BoostCheck() ? 15 : Distribution.Range(23, 28), bc1Queue);
    Process bc2 = new(model, "BC2");
    Channel bc2Channel = new(bc2, () => BoostCheck() ? 15 : 20, bc2Queue);

    create.Transitions.Add(new TransitionConditional(
        (ab1, (_) => ab1.Channels[0].Client == null),
        (ab2, (_) => true)));
    ab1.Transitions.Add(new TransitionSimple(bc1));
    ab2.Transitions.Add(new TransitionSimple(bc2));

    create.Start(0);
    model.Simulate(100000, true);

    Console.WriteLine($"BoostStat: {(float)boostCount/totalBoostCheckCount} ({boostCount}/{totalBoostCheckCount})");
}

static int ChannelComparison(Channel x, Channel y)
{
    if (x.Queue == null && y.Queue == null) return 0;
    else if (x.Queue == null && y.Queue != null) return -1;
	else if (x.Queue != null && y.Queue == null) return 1;
    else return x.Queue!.Count.CompareTo(y.Queue!.Count);
}

static void ChannelsSort(Process state)
{
    state.Channels.Sort(ChannelComparison);
}

static void BalanceQueues(Process state)
{
	ChannelsSort(state);
    for (int i = 0; i < state.Channels.Count; i++)
	{
		if(state.Channels[i].Queue != null 
			&& state.Channels[i].Queue!.Count == ((SimpleSimulationQueue) state.Channels[i].Queue!).MaxSize)
		{
			for (int j = 0; j < state.Channels.Count; j++)
			{
				if(state.Channels[j].Queue != null
					&& state.Channels[i].Queue!.Count - state.Channels[j].Queue!.Count >= 2)
				{
					state.Channels[j].Queue!.Enqueue(state.Channels[i].Queue!.Dequeue()!);
					break;
				}
			}
		}
	}
}