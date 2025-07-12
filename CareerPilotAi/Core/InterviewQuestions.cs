using CareerPilotAi.Core.Exceptions;

namespace CareerPilotAi.Core;

internal class InterviewQuestions
{
    internal readonly int _maxQuestions = 20;

    internal Guid JobApplicationId { get; private set; }
    internal string JobRole { get; set; } = string.Empty;
    internal string CompanyName { get; set; } = string.Empty;
    internal string InterviewPreparationContent { get; set; } = string.Empty;
    internal List<SingleInterviewQuestion> Questions { get; private set; }

    internal InterviewQuestions(Guid jobApplicationId, string jobRole, string companyName, string interviewPreparationContent)
    {
        if (jobApplicationId == Guid.Empty)
            throw new JobApplicationIdCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(jobRole))
            throw new JobApplicationTitleCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(companyName))
            throw new JobApplicationCompanyCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(interviewPreparationContent))
            throw new InterviewPreparationContentCannotBeEmptyException();

        JobApplicationId = jobApplicationId;
        JobRole = jobRole;
        CompanyName = companyName;
        InterviewPreparationContent = interviewPreparationContent;
        Questions = new List<SingleInterviewQuestion>();
    }

    internal void AddQuestion(SingleInterviewQuestion question)
    {
        Questions.Add(question);
    }

    internal List<SingleInterviewQuestion> GetActiveQuestions()
    {
        return Questions.Where(q => q.IsActive == true).ToList();
    }
}