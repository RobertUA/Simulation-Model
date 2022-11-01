// See https://aka.ms/new-console-template for more information

using Simulation;

Channel creatorChanell = new Channel();
StateEvent creator = new StateEvent(creatorChanell, () => Random.Shared.NextDouble());


Channel[] processorSubchanells = new Channel[2]
{
    new Channel(),
    new Channel()
};
Channel processChanell = new Channel(processorSubchanells);
StateEvent processor = new StateEvent(processChanell, () => Random.Shared.NextDouble());

Model model = new Model(creator, processor);

model.Simulate(1000);