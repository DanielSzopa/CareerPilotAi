namespace CareerPilotAi.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public bool HasApplications { get; set; }

        public DashboardMetricsViewModel? Metrics { get; set; }

        public DashboardChartsDataViewModel? ChartsData { get; set; }
    }
}

