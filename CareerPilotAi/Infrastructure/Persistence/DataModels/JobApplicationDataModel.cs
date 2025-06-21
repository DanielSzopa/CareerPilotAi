using Microsoft.AspNetCore.Identity;

namespace CareerPilotAi.Infrastructure.Persistence.DataModels;

internal class JobApplicationDataModel {
    internal Guid JobApplicationId { get; set; }
    internal IdentityUser User { get; set; }
    internal string UserId { get; set; }
    internal string Title { get; set; }
    internal string Company { get; set; }
    internal string? Url { get; set; }
    internal string JobDescription { get; set; }
}
