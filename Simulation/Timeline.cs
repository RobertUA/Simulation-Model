namespace Simulation;

public class Timeline
{
    public struct Segment
    {
        public int Count = 1;
        public double StartTime;
        public double EndTime;
        public double Length => EndTime - StartTime;
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
    public Segment LastSegment => _segments.Peek();
    public double WorkloadTime => _workLoadTime;
    public double WaitTime => _waitTime;
    public double TotalTime => _workLoadTime + _waitTime;
    public double WorkloadPercent => _workLoadTime / TotalTime;
    public double CountTimeProduct
    {
        get
        {
            double result = 0;
            foreach (var segment in _segments)
            {
                result += segment.Count * segment.Length;
            }
            return result;
        }
    }
    public double AvarageCount => CountTimeProduct / TotalTime;
    public double AvarageTime => CountTimeProduct / _segments.Count;
    public void Add(double startTime, double endTime, int count = 1)
    {
        if (startTime >= _lastSegment.EndTime)
        {
            _waitTime += startTime - _lastSegment.EndTime;
            _workLoadTime += _lastSegment.EndTime - _lastSegment.StartTime;
            _lastSegment = new Segment(startTime, endTime, count);
            _segments.Push(_lastSegment);
        }
        else
        {
            double lastEndTime = _lastSegment.EndTime;
            _lastSegment.EndTime = startTime;
            Segment middleSegment;
            if (endTime <= lastEndTime) // full contain
            {
                middleSegment = new Segment(startTime, endTime, _lastSegment.Count + count);
                _lastSegment = new Segment(endTime, lastEndTime, _lastSegment.Count);
            }
            else
            {
                middleSegment = new Segment(startTime, lastEndTime, _lastSegment.Count + count);
                _lastSegment = new Segment(lastEndTime, endTime, _lastSegment.Count);
                _workLoadTime += _lastSegment.EndTime - _lastSegment.StartTime;
            }
            _segments.Push(middleSegment);
            _workLoadTime += middleSegment.EndTime - middleSegment.StartTime;
            _segments.Push(_lastSegment);
        }
    }
}