namespace Simulation;

public class Workload
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
    private Segment _lastSegment;
    private double _workLoadTime = 0;
    private readonly Stack<Segment> _segments = new(new Segment[] { new Segment(0, 0) });
    public double WorkloadTime { get { return _workLoadTime; } }
    public void AddToWorkload(double timeStart, double timeEnd)
    {
        if (timeStart >= _lastSegment.EndTime)
        {
            _workLoadTime += _lastSegment.EndTime - _lastSegment.StartTime;
            _lastSegment = new Segment(timeStart, timeEnd);
            _segments.Push(_lastSegment);
        }
        else
        {
            double lastEndTime = _lastSegment.EndTime;
            _lastSegment.EndTime = timeStart;
            _workLoadTime += _lastSegment.EndTime - _lastSegment.StartTime;
            Segment middleSegment;
            if (timeEnd <= lastEndTime) // full contain
            {
                middleSegment = new Segment(timeStart, timeEnd, _lastSegment.Count + 1);
                _lastSegment = new Segment(timeEnd, lastEndTime, _lastSegment.Count);
            }
            else
            {
                middleSegment = new Segment(timeStart, lastEndTime, _lastSegment.Count + 1);
                _lastSegment = new Segment(lastEndTime, timeEnd, _lastSegment.Count);
            }
            _segments.Push(middleSegment);
            _workLoadTime += middleSegment.EndTime - middleSegment.StartTime;
            _segments.Push(_lastSegment);
        }
    }
}