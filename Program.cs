using Simulation;

Model model = new();

State create = new(model, "Create", false, false);
create.SetBeforeAction(ChannelsSort);
Channel createChannel = new(create, () => Random.Shared.NextDouble()/10);

State process1 = new(model, "Process1", true, true);
process1.SetBeforeAction(BalanceQueues);
Channel process1Channel1 = new(process1, () => Random.Shared.NextDouble(), 2);
Channel process1Channel2 = new(process1, () => Random.Shared.NextDouble(), 2);

State process2 = new(model, "Process2", true, true);
process2.SetBeforeAction(BalanceQueues);
Channel process2Channel1 = new(process2, () => Random.Shared.NextDouble(), 2);
Channel process2Channel2 = new(process2, () => Random.Shared.NextDouble(), 2);

create.Transitions.Add(new TransitionSimple(create));
create.Transitions.Add(new TransitionSimple(process1));

process1.Transitions.Add(new TransitionSimple(process2));

create.TryStartChannel(0);
model.Simulate(20);

static int ChannelComparison(Channel x, Channel y)
{
    return x.QueueSize.CompareTo(y.QueueSize);
}
static void ChannelsSort(State state)
{
    state.Channels.Sort(ChannelComparison);
}
static void BalanceQueues(State state)
{
	ChannelsSort(state);
    for (int i = 0; i < state.Channels.Count; i++)
	{
		if(state.Channels[i].QueueSize == state.Channels[i].QueueMaxSize)
		{
			for (int j = 0; j < state.Channels.Count; j++)
			{
				if(state.Channels[i].QueueSize - state.Channels[j].QueueSize >= 2)
				{
					state.Channels[i].QueueSize -= 1;
					state.Channels[j].QueueSize += 1;
					break;
				}
			}
		}
	}
}