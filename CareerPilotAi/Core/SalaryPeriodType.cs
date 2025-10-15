namespace CareerPilotAi.Core;

public static class SalaryPeriodType
{
    public const string Monthly = "Monthly";
    public const string Daily = "Daily";
    public const string Hourly = "Hourly";
    public const string Yearly = "Yearly";
    
    public static readonly IReadOnlyList<string> ValidPeriods = new List<string>
    {
        Monthly,
        Daily,
        Hourly,
        Yearly
    }.AsReadOnly();
}

