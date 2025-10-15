namespace CareerPilotAi.Core;

public static class WorkMode
{
    public const string Remote = "Remote";
    public const string Hybrid = "Hybrid";
    public const string OnSite = "OnSite";
    
    public static readonly IReadOnlyList<string> ValidModes = new List<string>
    {
        Remote,
        Hybrid,
        OnSite
    }.AsReadOnly();
}

