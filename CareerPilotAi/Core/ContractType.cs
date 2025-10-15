namespace CareerPilotAi.Core;

public static class ContractType
{
    public const string B2B = "B2B";
    public const string FTE = "FTE";
    public const string Other = "Other";
    
    public static readonly IReadOnlyList<string> ValidTypes = new List<string>
    {
        B2B,
        FTE,
        Other
    }.AsReadOnly();
}

