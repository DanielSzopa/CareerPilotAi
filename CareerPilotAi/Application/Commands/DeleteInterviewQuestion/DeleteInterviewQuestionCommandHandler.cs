using CareerPilotAi.Application.Commands.Abstractions;
using CareerPilotAi.Application.Services;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Application.Commands.DeleteInterviewQuestion;

public class DeleteInterviewQuestionCommandHandler : ICommandHandler<DeleteInterviewQuestionCommand, DeleteInterviewQuestionResponse>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUserService _userService;
    private readonly ILogger<DeleteInterviewQuestionCommandHandler> _logger;

    public DeleteInterviewQuestionCommandHandler(
        ApplicationDbContext applicationDbContext,
        IUserService userService,
        ILogger<DeleteInterviewQuestionCommandHandler> logger)
    {
        _applicationDbContext = applicationDbContext;
        _userService = userService;
        _logger = logger;
    }

    public async Task<DeleteInterviewQuestionResponse> HandleAsync(DeleteInterviewQuestionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userService.GetUserIdOrThrowException();
            
            // Find the interview question
            var interviewQuestion = await _applicationDbContext.InterviewQuestions
                .Include(iq => iq.InterviewQuestionsSection)
                .ThenInclude(iq => iq.JobApplication)
                .FirstOrDefaultAsync(iq => iq.Id == command.InterviewQuestionId, cancellationToken);

            if (interviewQuestion is null || interviewQuestion.InterviewQuestionsSection is null || interviewQuestion.InterviewQuestionsSection.JobApplication is null)
            {
                _logger.LogError("Interview question not found for deletion: {interviewQuestionId}, {userId}", command.InterviewQuestionId, userId);
                return new DeleteInterviewQuestionResponse(false, "Interview question not found.");
            }

            if (interviewQuestion.InterviewQuestionsSection.JobApplication.UserId != userId)
            {
                _logger.LogError("User {userId} attempted to delete interview question {interviewQuestionId} that doesn't belong to them", userId, command.InterviewQuestionId);
                return new DeleteInterviewQuestionResponse(false, "Interview question not found or you don't have permission to delete it.");
            }

            _applicationDbContext.InterviewQuestions.Remove(interviewQuestion);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Interview question deleted successfully: {interviewQuestionId} for UserId: {userId}", command.InterviewQuestionId, userId);
            return new DeleteInterviewQuestionResponse(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting interview question: {interviewQuestionId}", command.InterviewQuestionId);
            return new DeleteInterviewQuestionResponse(false, "An error occurred while deleting the interview question.");
        }
    }
}
