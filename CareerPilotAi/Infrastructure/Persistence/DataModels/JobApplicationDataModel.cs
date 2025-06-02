using Microsoft.AspNetCore.Identity;

namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class JobApplicationDataModel {
    internal Guid JobApplicationId { get; set; }
    internal IdentityUser User { get; set; }
    internal string UserId { get; set; }
    internal string EntryJobDetails_Url { get; set; }
    internal string EntryJobDetails_Text { get; set; }
    internal string PersonalDetails_Text { get; set; }
}
