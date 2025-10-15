using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.ViewModels.JobApplication;

namespace CareerPilotAi.Application.Commands.CreateJobApplication;

public record CreateJobApplicationCommand(CreateJobApplicationViewModel ViewModel) : ICommand<Guid>;
