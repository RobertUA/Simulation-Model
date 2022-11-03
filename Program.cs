// See https://aka.ms/new-console-template for more information

using Simulation;

Model model = new Model();

Channel createChannel = new Channel(model, () => Random.Shared.NextDouble());
StateEvent create = new StateEvent("Create", createChannel, true);

Channel processChannel = new Channel(model, () => Random.Shared.NextDouble());
Channel[] processSubchannels = new Channel[2]
{
    new Channel(model, () => Random.Shared.NextDouble()),
    new Channel(model, () => Random.Shared.NextDouble())
};
processChannel.SetSubChannels(processSubchannels);
StateEvent process = new StateEvent("Process", processChannel, false);

create.SetTransitions((process, 1));

create.Start(0);
model.Simulate(3);