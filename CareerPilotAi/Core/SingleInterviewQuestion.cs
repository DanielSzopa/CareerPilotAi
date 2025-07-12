namespace CareerPilotAi.Core;

public class SingleInterviewQuestion
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Guide { get; set; }
    public bool IsActive { get; set; }

    public SingleInterviewQuestion(Guid id, string question, string answer, string guide, bool isActive)
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
        IsActive = isActive;
    }
}
