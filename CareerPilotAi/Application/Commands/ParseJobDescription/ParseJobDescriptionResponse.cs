using CareerPilotAi.ViewModels.JobApplication;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.ParseJobDescription;

public class ParseJobDescriptionResponse
{
    public bool IsSuccess { get; set; }
    public string FeedbackMessage { get; set; } = string.Empty;
    public ParseJobDescriptionResultViewModel? ParsedData { get; set; }
    public ProblemDetails? ProblemDetails { get; set; }
}

