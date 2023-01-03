using Simulation;
using RobRandom;

//Lab1();
//Lab2();
Lab3();
Console.Beep();

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
    Channel process1channel1 = new(process1, (_) => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process1channel2 = new(process1, (_) => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    Process process2 = new(model, "Process-2");
    process2.SetBeforeAction(() => ChannelsSort(process2));
    Channel process2channel1 = new(process2, (_) => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process2channel2 = new(process2, (_) => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    Process process3 = new(model, "Process-3");
    process3.SetBeforeAction(() => ChannelsSort(process3));
    Channel process3channel1 = new(process3, (_) => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));
    Channel process3channel2 = new(process3, (_) => Random.Shared.NextDouble(), new SimpleSimulationQueue(2));

    create.Transitions.Add(new TransitionSimple(process1));
    process1.Transitions.Add(new TransitionSimple(process2));
    process2.Transitions.Add(new TransitionSimple(process3));
    //process3.Transitions.Add(new TransitionSimple(process2)); // Multy transitions;

    create.Start(0);
    model.Simulate(100000);
}

static void Lab3()
{
    //Task2();
    Task3();
    static void Task2()
    {
        int desposeCount = 0;
        double lastDesposeTime = 0;
        double desposeTimeSum = 0;
        int queueChangeCount = 0;
        Model model = new();

        Client templateClient = new Client(0);
        templateClient.OnDesposeAction = new ((client) => 
        {
            desposeCount++;
            if (lastDesposeTime == 0) lastDesposeTime = client.DesposeTime;
            desposeTimeSum += client.DesposeTime - lastDesposeTime;
            lastDesposeTime = client.DesposeTime;
        });

        Create create = new(model, "Create", () => Distribution.Exponential(0.5), templateClient);
        Process process = new (model, "Process");
        process.SetBeforeAction(() =>
        {
            BalanceQueues(process, ref queueChangeCount);
            ChannelsSort(process);
        });
        Channel channel1 = new(process, (_) => Distribution.Exponential(0.3), new SimpleSimulationQueue(3));
        Channel channel2 = new(process, (_) => Distribution.Exponential(0.3), new SimpleSimulationQueue(3));

        channel1.Start(0, Distribution.Gaus(1, 0.3), new Client(create.Client));
        channel1.Queue!.TryEnqueue(new Client(create.Client));
        channel1.Queue!.TryEnqueue(new Client(create.Client));

        channel2.Start(0, Distribution.Gaus(1, 0.3), new Client(create.Client));
        channel2.Queue!.TryEnqueue(new Client(create.Client));
        channel2.Queue!.TryEnqueue(new Client(create.Client));

        create.Transitions.Add(new TransitionSimple(process));

        create.Start(0, 0.1);

        model.Simulate(1000000);

        Console.WriteLine("1) ================" +
            $"\nC1: Avg count = {channel1.Timeline.AvarageCount}; Workload = {channel1.Timeline.WorkloadPercent} ({channel1.Timeline.WorkloadTime}/{channel1.Timeline.TotalTime}" +
            $"\nC2: Avg count = {channel2.Timeline.AvarageCount}; Workload = {channel2.Timeline.WorkloadPercent} ({channel2.Timeline.WorkloadTime}/{channel2.Timeline.TotalTime}");
        Console.WriteLine("2) ================" +
            $"\nAvg client count = {model.Timeline.AvarageCount}");
        Console.WriteLine("3) ================" +
            $"\nAvg time beetween clients despose = {desposeTimeSum/desposeCount} ({desposeTimeSum}/{desposeCount})");
        Console.WriteLine("4) ================" +
            $"\nAvg client time = {model.Timeline.AvarageTime}");
        Console.WriteLine("5) ================" +
            $"\nAvg С1 queue count = {channel1.Queue.Timeline.AvarageCount}" + 
            $"\nAvg С2 queue count = {channel2.Queue.Timeline.AvarageCount}");
        Console.WriteLine("6) ================" +
            $"\nClient fail = {(double)model.Statistic.FailsCount/model.Clients.Count} ({model.Statistic.FailsCount}/{model.Clients.Count})");
        Console.WriteLine("7) ================" +
            $"\nQueue change count = {queueChangeCount}");
    }
    static void Task3()
    {
        int labCount = 0;
        double lastLabTime = -1;
        double labDifSum = 0;
        int ClientComparison(Client a, Client b)
        {
            if(a.Type == b.Type) return a.CreateTime.CompareTo(b.CreateTime);  
            else if (a.Type == 1 && b.Type != 1) return 1;
            else if (a.Type != 1 && b.Type == 1) return -1;
            else return 0;
        }
        Model model = new();
        Client createClient = new();
        Create create = new(model, "Create", () => Distribution.Exponential(15), createClient);
        create.BeforeAction = () =>
        {
            createClient.Type = Distribution.RangeInteger(1, 3);
            //Console.WriteLine($"rand Type = {createClient.Type}");
        };
        PrioritySimulationQueue receptionQueue = new(Comparer<Client>.Create((a, b) => ClientComparison(a, b)));

        Process reception = new(model, "Reception");
        double RandClient(Client client)
        {
            switch (client.Type)
            {
                case 1:
                    return Distribution.Exponential(15);
                case 2:
                    return Distribution.Exponential(40);
                case 3:
                    return Distribution.Exponential(30);
                default:
                    return 0;
            }
        }
        Channel[] doctors = new Channel[2];
        for (int i = 0; i < doctors.Length; i++)
        {
            doctors[i] = new(reception, RandClient, receptionQueue);
        }

        SimpleSimulationQueue escortingQueue = new();
        Process escorting = new(model, "Escorting");
        Channel[] escorts = new Channel[3];
        for (int i = 0; i < escorts.Length; i++)
        {
            escorts[i] = new Channel(escorting, (_) => Distribution.RangeDouble(3, 8), escortingQueue);
        }

        SimpleSimulationQueue registationQueue = new();
        Process registration = new(model, "Registration");
        Channel[] registrators = new Channel[1];
        for (int i = 0; i < registrators.Length; i++)
        {
            registrators[i] = new(registration, (_) => Distribution.Erlang(4.5, 3), registationQueue);
        }

        SimpleSimulationQueue labQueue = new();
        Process labTest = new(model, "Laboratory");
        labTest.SetStartAction(() =>
        {
            if (lastLabTime == -1) lastLabTime = model.CurrentTime;
            else
            {
                labCount++;
                labDifSum += model.CurrentTime - lastLabTime;
                lastLabTime = model.CurrentTime;
            }
        });
        Channel[] labs = new Channel[2];
        for (int i = 0; i < labs.Length; i++)
        {
            labs[i] = new Channel(labTest, (_) => Distribution.Erlang(4, 2), labQueue);
        }

        create.Transitions.Add(new TransitionSimple(reception));

        reception.Transitions.Add(new TransitionConditional(
            (escorting, (client) => client.Type == 1),
            (registration, (client) => true)
            ));

        registration.Transitions.Add(new TransitionSimple(labTest));

        labTest.Transitions.Add(new TransitionConditional(
            (reception, (client) => {
                if (client.Type == 2)
                {
                    reception.TryStartChannel(model.CurrentTime, new Client(1));
                }
                return false;
                })
            ));

        create.Start(0);
        model.Simulate(25, true);

        model.PrintEndInfo();

        Console.WriteLine($"Avg client time = {model.Clients.Sum(client => (client.DesposeTime != 0 ? client.DesposeTime : model.CurrentTime) - client.CreateTime)/model.Clients.Count}");
        Console.WriteLine($"Avg time beetween lab visit = {labDifSum / labCount}");
    }
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

static void BalanceQueues(Process state, ref int changeCount)
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
                    changeCount++;
					state.Channels[j].Queue!.TryEnqueue(state.Channels[i].Queue!.Dequeue()!);
					break;
				}
			}
		}
	}
}