// See https://aka.ms/new-console-template for more information

using Simulation;

Model model = new Model();

State create = new State(model, "Create");
Channel createChannel = new Channel(create, () => Random.Shared.NextDouble());

State process = new State(model, "Process");
Channel processChannel1 = new Channel(process, () => Random.Shared.NextDouble());
Channel processChannel2 = new Channel(process, () => Random.Shared.NextDouble());

create.Transitions.Add(new Transition(create));
create.Transitions.Add(new Transition(process));

create.TryStart(0);
model.Simulate(100);