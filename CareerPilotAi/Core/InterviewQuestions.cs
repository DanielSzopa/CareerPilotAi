using CareerPilotAi.Core.Exceptions;

namespace CareerPilotAi.Core;

internal class InterviewQuestions
{
    internal readonly int _maxQuestions = 30;

    internal Guid JobApplicationId { get; private set; }
    internal string JobRole { get; set; } = string.Empty;
    internal string CompanyName { get; set; } = string.Empty;
    internal string JobDescription { get; set; } = string.Empty;
    internal List<SingleInterviewQuestion> Questions { get; private set; }

    internal InterviewQuestions(Guid jobApplicationId, string jobRole, string companyName, string jobDescription)
    {
        if (jobApplicationId == Guid.Empty)
            throw new JobApplicationIdCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(jobRole))
            throw new JobApplicationTitleCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(companyName))
            throw new JobApplicationCompanyCannotBeEmptyException();

        if (string.IsNullOrWhiteSpace(jobDescription))
            throw new JobApplicationJobDescriptionCannotBeEmptyException();

        JobApplicationId = jobApplicationId;
        JobRole = jobRole;
        CompanyName = companyName;
        JobDescription = jobDescription;
        Questions = new List<SingleInterviewQuestion>();
    }

    internal void AddQuestion(SingleInterviewQuestion question)
    {
        Questions.Add(question);
    }

    internal void AddQuestionsRange(IEnumerable<SingleInterviewQuestion> questions)
    {
        Questions.AddRange(questions);
    }

    internal bool CanGenerateMoreInterviewQuestions()
    {
        if (Questions.Count >= _maxQuestions)
        {
            return false;
        }

        return true;
    }

    internal bool ShouldGenerateFirstInterviewQuestions()
    {
        return Questions.Count == 0;
    }
}