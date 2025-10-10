
namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class TimeZoneDataModel
{
    internal string TimeZoneId { get; set; }
    internal string Name { get; set; }
    internal ICollection<UserSettingsDataModel> UserSettings { get; set; } = new List<UserSettingsDataModel>();
}

