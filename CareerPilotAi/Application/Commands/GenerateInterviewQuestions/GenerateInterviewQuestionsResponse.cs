using CareerPilotAi.Models.JobApplication;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Application.Commands.GenerateInterviewQuestions;

public record GenerateInterviewQuestionsResponse(bool IsSuccess, InterviewQuestionsSectionViewModel InterviewQuestionsSection, ProblemDetails? ProblemDetails);
