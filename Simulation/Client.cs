namespace Simulation;

public class Client
{
    public Create? Create;
    public int Type;
    public double DesposeTime;
    //
    public double CreateTime;
    public bool Fail = false;
    public Timeline Timeline = new();
    public Action<Client>? OnDesposeAction = null;
    public Client(int type)
    {
        Type = type;
    }
    public Client(Client client)
    {
        Create = client.Create;
        Type = client.Type;
        OnDesposeAction = client.OnDesposeAction;
    }
    public void OnCreate(double createTime)
    {
        CreateTime = createTime;
    }
    public void OnDespose(double desposeTime)
    {
        DesposeTime = desposeTime;
        if(OnDesposeAction!=null) OnDesposeAction.Invoke(this);
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
