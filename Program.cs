// See https://aka.ms/new-console-template for more information

using Simulation;

Model model = new Model();

StateEvent create = new StateEvent(model, "Create", true);
Channel createChannel = new Channel(create, () => Random.Shared.NextDouble());

StateEvent process = new StateEvent(model, "Process", false);
Channel processChannel1 = new Channel(process, () => Random.Shared.NextDouble());
Channel processChannel2 = new Channel(process, () => Random.Shared.NextDouble());

create.SetTransitions((process, 1));

create.TryStart(0);
model.Simulate(100);