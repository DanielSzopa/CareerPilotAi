using CareerPilotAi.ViewModels.JobApplication;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.ParseJobDescription;

public class ParseJobDescriptionResponse
{
    public bool IsSuccess { get; set; }
    public ParsingResultType ParsingResult { get; set; }
    public List<string>? MissingFields { get; set; }
    public CreateJobApplicationViewModel? ParsedData { get; set; }
    public ProblemDetails? ProblemDetails { get; set; }
}

