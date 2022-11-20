namespace Simulation;

public class Timeline
{
    public struct Segment
    {
        public int Count = 1;
        public double StartTime;
        public double EndTime;
        public Segment(double startTime, double endTime, int count = 1)
        {
            StartTime = startTime;
            EndTime = endTime;
            Count = count;
        }
    }
    private Segment _lastSegment = new Segment(0,0,0);
    private double _workLoadTime = 0;
    private double _waitTime = 0;
    private readonly Stack<Segment> _segments = new(new Segment[] { new Segment(0, 0) });
    public Segment[] Segments => _segments.ToArray();
    public double WorkloadTime => _workLoadTime;
    public double WaitTime => _waitTime;
    public double TotalTime => _workLoadTime + _waitTime;
    public void Add(double startTime, double endTime)
    {
        if (startTime >= _lastSegment.EndTime)
        {
            _waitTime += startTime - _lastSegment.EndTime;
            _workLoadTime += _lastSegment.EndTime - _lastSegment.StartTime;
            _lastSegment = new Segment(startTime, endTime);
            _segments.Push(_lastSegment);
        }
        else
        {
            double lastEndTime = _lastSegment.EndTime;
            _lastSegment.EndTime = startTime;
            _workLoadTime += _lastSegment.EndTime - _lastSegment.StartTime;
            Segment middleSegment;
            if (endTime <= lastEndTime) // full contain
            {
                middleSegment = new Segment(startTime, endTime, _lastSegment.Count + 1);
                _lastSegment = new Segment(endTime, lastEndTime, _lastSegment.Count);
            }
            else
            {
                middleSegment = new Segment(startTime, lastEndTime, _lastSegment.Count + 1);
                _lastSegment = new Segment(lastEndTime, endTime, _lastSegment.Count);
            }
            _segments.Push(middleSegment);
            _workLoadTime += middleSegment.EndTime - middleSegment.StartTime;
            _segments.Push(_lastSegment);
        }
    }
}