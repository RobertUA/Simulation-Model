using System.Diagnostics;

namespace Simulation;

public class Client
{
    public int Id = 0;
    public Create? Create;
    public int Type;
    public double DesposeTime;
    //
    public double CreateTime;
    public bool Fail = false;
    public bool InQueue = false;
    public Process? LastProccess;
    public Action<Client>? OnDesposeAction = null;
    public Client(int type=0)
    {
        Type = type;
    }
    public Client(Client client)
    {
        Create = client.Create;
        Type = client.Type;
        OnDesposeAction = client.OnDesposeAction;
        //Console.WriteLine($"new Client | type = {Type}");
    }
    public void OnCreate(double createTime)
    {
        CreateTime = createTime;
    }
    public void OnDespose(double desposeTime)
    {
        DesposeTime = desposeTime;
        if (OnDesposeAction != null) OnDesposeAction.Invoke(this);
        //if (DesposeTime < CreateTime)
        //{
        //    Console.WriteLine($"{DesposeTime} - {CreateTime} = {DesposeTime - CreateTime}");
        //    Console.WriteLine($"");
        //}
    }
    public void OnChannelStart(double startTime, double endTime)
    {
        //Console.WriteLine($"Client {Id} Ch ({LastProccess}) START ({startTime} | {endTime}) ");
    }
    public void OnChannelEnd(double startTime, double endTime)
    {
        //Timeline.Add(startTime, endTime);
        //Console.WriteLine($"Client {Id} Ch ({LastProccess}) END ({startTime} | {endTime}) ");
    }
    public void OnFail()
    {
        Fail = true;
    }
}
