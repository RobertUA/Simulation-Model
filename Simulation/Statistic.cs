﻿namespace Simulation;

public class Statistic
{
    public int TotalCount = 0;
    public int FailCount = 0;
    public int SuccessCount
    {
        get { return TotalCount - FailCount; }
    }
}