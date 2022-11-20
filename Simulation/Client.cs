namespace Simulation;

public class Client
{
    public Create Create;
    public int Type;
    public double DesposeTime;
    //
    public double CreateTime;
    public bool Fail = false;
    public Timeline Timeline = new();
    public Client(int type, Create create) // OnCreate
    {
        Create = create;
        Type = type;
    }
    public Client(Client client)
    {
        Create = client.Create;
        Type = client.Type;
    }
    public void OnCreate(double createTime)
    {
        CreateTime = createTime;
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
