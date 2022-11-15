namespace Simulation;

public class Client
{
    public int Type;
    public double CreateTime;
    public double DesposeTime;
    public Client(double createTime, int type)
    {
        CreateTime = createTime;
        Type = type;
    }
    public void Despose(double desposeTime)
    {
        DesposeTime = desposeTime;
    }
}
