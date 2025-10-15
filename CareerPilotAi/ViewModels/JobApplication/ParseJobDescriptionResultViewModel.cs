namespace CareerPilotAi.ViewModels.JobApplication;

public class ParseJobDescriptionResultViewModel
{
    public bool Success { get; set; }
    public ParsingResultType ParsingResult { get; set; }
    public List<string> MissingFields { get; set; } = new List<string>();
    public CreateJobApplicationViewModel? Data { get; set; }
}

public enum ParsingResultType
{
    FullSuccess,
    PartialSuccess,
    Failed
}

