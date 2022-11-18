namespace Simulation;

public class Client
{
    public Create Create;
    public int Type;
    public double CreateTime;
    public double DesposeTime;
    public bool Fail = false;
    public Timeline Timeline = new();
    public Client(double createTime, int type, Create create) // OnCreate
    {
        Create = create;
        CreateTime = createTime;
        Type = type;
    }
    public void OnDespose(double desposeTime)
    {
        DesposeTime = desposeTime;
    }
    public void OnChannelEnd(double startTime, double endTime)
    {
        Timeline.Add(startTime, endTime);
    }
    public void OnFail()
    {
        Fail = true;
    }
}
