using CareerPilotAi.ViewModels.Dashboard;

namespace CareerPilotAi.Application.Services;

public interface IDashboardDataService
{
    Task<DashboardViewModel> GetDashboardViewModelAsync(string userId, CancellationToken cancellationToken);
}

