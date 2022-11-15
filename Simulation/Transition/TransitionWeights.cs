namespace Simulation;

public class TransitionWeights : ITransition
{
    private readonly Func<double> _randFunc;
    private readonly (Process? state, double weight)[] _statesWeights;
    public TransitionWeights(Func<double> randFunc, params (Process? state, double weight)[] statesWeights)
    {
        _randFunc = randFunc;
        _statesWeights = statesWeights;
        double sum = 0;
        for (int i = 0; i < _statesWeights.Length; i++)
        {
            sum += _statesWeights[i].weight;
        }
        for (int i = 0; i < _statesWeights.Length; i++)
        {
            _statesWeights[i].weight = _statesWeights[i].weight / sum + (i == 0 ? 0 : _statesWeights[i-1].weight);
        }
    }
    public Process? GetTransitionProcess()
    {
        double rand = _randFunc();
        for (int i = 0; i < _statesWeights.Length; i++)
        {
            if (_statesWeights[i].weight <= rand) return _statesWeights[i].state;
        }
        return null;
    }
}