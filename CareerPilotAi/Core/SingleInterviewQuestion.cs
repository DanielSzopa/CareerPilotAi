namespace CareerPilotAi.Core;

internal class SingleInterviewQuestion
{
    internal Guid Id { get; set; }
    internal string Question { get; set; }
    internal string Answer { get; set; }
    internal string Guide { get; set; }

    internal SingleInterviewQuestion(Guid id, string question, string answer, string guide)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        
        if (string.IsNullOrWhiteSpace(question))
            throw new ArgumentException("Question cannot be empty.", nameof(question));

        if (string.IsNullOrWhiteSpace(answer))
            throw new ArgumentException("Answer cannot be empty.", nameof(answer));

        if (string.IsNullOrWhiteSpace(guide))
            throw new ArgumentException("Guide cannot be empty.", nameof(guide));

        Id = id;
        Question = question;
        Answer = answer;
        Guide = guide;
    }
}
