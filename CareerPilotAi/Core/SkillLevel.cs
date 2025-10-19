namespace CareerPilotAi.Core;

public static class SkillLevel
{
    public const string NiceToHave = "Nice to have";
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

    public static int GetLevelIndex(string? level)
    {
        if (string.IsNullOrWhiteSpace(level))
        {
            return -1;
        }

        return level switch
        {
            NiceToHave => 0,
            Junior => 1,
            Regular => 2,
            Advanced => 3,
            Master => 4,
            _ => 5
        };
    }
}
