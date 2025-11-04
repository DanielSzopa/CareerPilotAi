using System;
using System.Collections.Generic;

namespace CareerPilotAi.ViewModels.Dashboard
{
    public class DashboardChartsDataViewModel
    {
        public IReadOnlyList<StatusChartItemViewModel> StatusDistribution { get; set; } = Array.Empty<StatusChartItemViewModel>();

        public ApplicationsOverTimeDataViewModel ThirtyDaysData { get; set; } = ApplicationsOverTimeDataViewModel.Empty();

        public ApplicationsOverTimeDataViewModel SixtyDaysData { get; set; } = ApplicationsOverTimeDataViewModel.Empty();

        public ApplicationsOverTimeDataViewModel NinetyDaysData { get; set; } = ApplicationsOverTimeDataViewModel.Empty();
    }

    public class StatusChartItemViewModel
    {
        public string Status { get; set; } = string.Empty;

        public int Count { get; set; }

        public string Color { get; set; } = string.Empty;

        public decimal Percentage { get; set; }
    }

    public class ApplicationsOverTimeDataViewModel
    {
        public IReadOnlyList<string> Labels { get; set; } = Array.Empty<string>();

        public IReadOnlyList<int> Values { get; set; } = Array.Empty<int>();

        public static ApplicationsOverTimeDataViewModel Empty() => new ApplicationsOverTimeDataViewModel();
    }
}

