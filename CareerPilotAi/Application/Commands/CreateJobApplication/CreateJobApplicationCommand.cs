using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Models.JobApplication;

namespace CareerPilotAi.Application.Commands.CreateJobApplication;

public record CreateJobApplicationCommand(JobOfferEntryDetailsViewModel vm) : ICommand;
