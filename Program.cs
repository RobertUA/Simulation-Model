// See https://aka.ms/new-console-template for more information

using Simulation;

Model model = new();

State create = new(model, "Create", false, false, OwnComparison);
Channel createChannel = new(create, () => Random.Shared.NextDouble()/10);

State process1 = new(model, "Process1", true, true);
Channel process1Channel1 = new(process1, () => Random.Shared.NextDouble(), 2);
Channel process1Channel2 = new(process1, () => Random.Shared.NextDouble(), 2);

State process2 = new(model, "Process2", true, true);
Channel process2Channel1 = new(process2, () => Random.Shared.NextDouble(), 2);
Channel process2Channel2 = new(process2, () => Random.Shared.NextDouble(), 2);

create.Transitions.Add(new TransitionSimple(create));
create.Transitions.Add(new TransitionSimple(process1));

process1.Transitions.Add(new TransitionSimple(process2));

create.TryStartChannel(0);
model.Simulate(20);

static int OwnComparison(Channel x, Channel y)
{
    return x.QueueSize.CompareTo(y.QueueSize);
}