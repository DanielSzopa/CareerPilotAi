namespace CareerPilotAi.Core;

public static class SalaryType
{
    public const string Gross = "Gross";
    public const string Net = "Net";
    
    public static readonly IReadOnlyList<string> ValidTypes = new List<string>
    {
        Gross,
        Net
    }.AsReadOnly();
}

