using Microsoft.AspNetCore.Identity;

namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class UserSettingsDataModel
{
    internal Guid UserSettingsId { get; set; }
    internal string UserId { get; set; }
    internal IdentityUser User { get; set; }
    internal string TimeZoneId { get; set; }
    internal TimeZoneDataModel TimeZone { get; set; }
}

