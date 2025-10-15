namespace CareerPilotAi.Core;

public static class ExperienceLevel
{
    public const string Junior = "Junior";
    public const string Mid = "Mid";
    public const string Senior = "Senior";
    public const string NotSpecified = "NotSpecified";
    
    public static readonly IReadOnlyList<string> ValidLevels = new List<string>
    {
        Junior,
        Mid,
        Senior,
        NotSpecified
    }.AsReadOnly();
}

