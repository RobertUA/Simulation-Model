namespace Simulation;

public class WeightTransition : ITransition
{
    private Func<double> _randFunc;
    private (State? state, double weight)[] _data;
    public WeightTransition(Func<double> randFunc, params (State? state, double weight)[] statesWeights)
    {
        _randFunc = randFunc;
        _data = statesWeights;
        double sum = 0;
        for (int i = 0; i < _data.Length; i++)
        {
            sum += _data[i].weight;
        }
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].weight = _data[i].weight / sum + (i == 0 ? 0 : _data[i-1].weight);
        }
    }
    public State? GetTransitionState()
    {
        double rand = _randFunc();
        for (int i = 0; i < _data.Length; i++)
        {
            if (_data[i].weight <= rand) return _data[i].state;
        }
        return _data[0].state;
    }
}