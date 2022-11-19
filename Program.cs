using Simulation;
using RobRandom;

Lab1();

static void Lab1()
{
    void WriteValues(StreamWriter streamWriter, string name, int count, Func<double> rand)
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

    StreamWriter streamWriter = new StreamWriter("DistributionResults.txt");

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
            Uniform uniform = new Uniform(1, Math.Pow(5, 13), 0, Math.Pow(2, 31));
            WriteValues(streamWriter, $"Uniform ({a}, {c})", 10000, () => uniform.Next());
        }
    }
    //
    Uniform concreteUniform = new Uniform(1, Math.Pow(5, 13), 0, Math.Pow(2, 31));
    WriteValues(streamWriter, "Uniform (5^13, 2^31)", 10000, () => concreteUniform.Next());
    //
    streamWriter.Close();
}

static void TestSim()
{
    Model model = new();

    Create create = new(model, "Create", () => Random.Shared.NextDouble() / 2);

    Process process1 = new(model, "Process1");
    process1.SetBeforeAction(BalanceQueues);
    Channel process1Channel1 = new(process1, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process1Channel2 = new(process1, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    Process process2 = new(model, "Process2");
    process2.SetBeforeAction(BalanceQueues);
    Channel process2Channel1 = new(process2, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process2Channel2 = new(process2, () => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    //create.Transitions.Add(new TransitionSimple(create));
    create.Transitions.Add(new TransitionSimple(process1));

    process1.Transitions.Add(new TransitionSimple(process2));

    create.Start(0);
    model.Simulate(1000000, false);
}

static int ChannelComparison(Channel x, Channel y)
{
    if (x.StateQueue == null && y.StateQueue == null) return 0;
    else if (x.StateQueue == null && y.StateQueue != null) return -1;
	else if (x.StateQueue != null && y.StateQueue == null) return 1;
    else return x.StateQueue!.Count.CompareTo(y.StateQueue!.Count);
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
		if(state.Channels[i].StateQueue != null 
			&& state.Channels[i].StateQueue!.Count == state.Channels[i].StateQueue!.MaxSize)
		{
			for (int j = 0; j < state.Channels.Count; j++)
			{
				if(state.Channels[j].StateQueue != null
					&& state.Channels[i].StateQueue!.Count - state.Channels[j].StateQueue!.Count >= 2)
				{
					state.Channels[j].StateQueue!.Enqueue(state.Channels[i].StateQueue!.Dequeue()!);
					break;
				}
			}
		}
	}
}