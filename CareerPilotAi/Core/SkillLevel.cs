namespace CareerPilotAi.Core;

public static class SkillLevel
{
    public const string NiceToHave = "NiceToHave";
    public const string Junior = "Junior";
    public const string Regular = "Regular";
    public const string Advanced = "Advanced";
    public const string Master = "Master";
    
    public static readonly IReadOnlyList<string> ValidLevels = new List<string>
    {
        NiceToHave,
        Junior,
        Regular,
        Advanced,
        Master
    }.AsReadOnly();
}

