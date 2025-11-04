using CareerPilotAi.Application.Services;
using CareerPilotAi.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CareerPilotAi.Controllers;

[Authorize]
[Route("dashboard")]
public class DashboardController : Controller
{
    private readonly IUserService _userService;
    private readonly IDashboardDataService _dashboardDataService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IUserService userService,
        IDashboardDataService dashboardDataService,
        ILogger<DashboardController> logger)
    {
        _userService = userService;
        _dashboardDataService = dashboardDataService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = _userService.GetUserIdOrThrowException();

        try
        {
            var viewModel = await _dashboardDataService.GetDashboardViewModelAsync(userId, cancellationToken);
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load dashboard data for user {userId}.", userId);
            TempData["DashboardError"] = "We could not load your dashboard data right now. Please try again later.";
            return View(new DashboardViewModel { HasApplications = false });
        }
    }
}

