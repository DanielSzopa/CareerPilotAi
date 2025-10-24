using Bogus;
using CareerPilotAi.Core;
using CareerPilotAi.Core.Exceptions;
using Shouldly;

namespace CareerPilotAi.UnitTests.Core;

public class InterviewQuestionsTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldCreateInstance_WithValidParameters()
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();

        var interviewQuestions = new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent);

        interviewQuestions.ShouldNotBeNull();
        interviewQuestions.JobApplicationId.ShouldBe(jobApplicationId);
        interviewQuestions.JobRole.ShouldBe(jobRole);
        interviewQuestions.CompanyName.ShouldBe(companyName);
        interviewQuestions.InterviewPreparationContent.ShouldBe(interviewPreparationContent);
        interviewQuestions.Questions.ShouldNotBeNull();
        interviewQuestions.Questions.ShouldBeEmpty();
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenJobApplicationIdIsEmpty()
    {
        var emptyGuid = Guid.Empty;
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();

        var exception = Should.Throw<JobApplicationIdCannotBeEmptyException>(
            () => new InterviewQuestions(emptyGuid, jobRole, companyName, interviewPreparationContent));

        exception.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowException_WhenJobRoleIsEmpty(string jobRole)
    {
        var jobApplicationId = _faker.Random.Guid();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();

        var exception = Should.Throw<JobApplicationTitleCannotBeEmptyException>(
            () => new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent));

        exception.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowException_WhenCompanyNameIsEmpty(string companyName)
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var interviewPreparationContent = _faker.Lorem.Paragraph();

        var exception = Should.Throw<JobApplicationCompanyCannotBeEmptyException>(
            () => new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent));

        exception.ShouldNotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowException_WhenInterviewPreparationContentIsEmpty(string interviewPreparationContent)
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();

        var exception = Should.Throw<InterviewPreparationContentCannotBeEmptyException>(
            () => new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent));

        exception.ShouldNotBeNull();
    }

    [Fact]
    public void AddQuestion_ShouldAddQuestionToList_WhenQuestionIsValid()
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();
        var interviewQuestions = new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent);

        var questionId = _faker.Random.Guid();
        var questionText = _faker.Lorem.Sentence();
        var answer = _faker.Lorem.Paragraph();
        var guide = _faker.Lorem.Paragraph();
        var isActive = _faker.Random.Bool();
        var question = new SingleInterviewQuestion(questionId, questionText, answer, guide, isActive);

        interviewQuestions.AddQuestion(question);

        interviewQuestions.Questions.Count.ShouldBe(1);
        interviewQuestions.Questions.ShouldContain(question);
    }

    [Fact]
    public void AddQuestion_ShouldThrowException_WhenQuestionIsNull()
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();
        var interviewQuestions = new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent);

        var exception = Should.Throw<SingleInterviewQuestionCannotBeNullException>(
            () => interviewQuestions.AddQuestion(null!));

        exception.ShouldNotBeNull();
        interviewQuestions.Questions.ShouldBeEmpty();
    }

    [Fact]
    public void GetActiveQuestions_ShouldReturnOnlyActiveQuestions()
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();
        var interviewQuestions = new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent);

        var activeQuestion1 = new SingleInterviewQuestion(_faker.Random.Guid(), _faker.Lorem.Sentence(), _faker.Lorem.Paragraph(), _faker.Lorem.Paragraph(), true);
        var activeQuestion2 = new SingleInterviewQuestion(_faker.Random.Guid(), _faker.Lorem.Sentence(), _faker.Lorem.Paragraph(), _faker.Lorem.Paragraph(), true);
        var inactiveQuestion = new SingleInterviewQuestion(_faker.Random.Guid(), _faker.Lorem.Sentence(), _faker.Lorem.Paragraph(), _faker.Lorem.Paragraph(), false);

        interviewQuestions.AddQuestion(activeQuestion1);
        interviewQuestions.AddQuestion(activeQuestion2);
        interviewQuestions.AddQuestion(inactiveQuestion);

        var activeQuestions = interviewQuestions.GetActiveQuestions();

        activeQuestions.Count.ShouldBe(2);
        activeQuestions.ShouldContain(activeQuestion1);
        activeQuestions.ShouldContain(activeQuestion2);
        activeQuestions.ShouldNotContain(inactiveQuestion);
    }

    [Fact]
    public void GetActiveQuestions_ShouldReturnEmptyList_WhenNoActiveQuestionsExist()
    {
        var jobApplicationId = _faker.Random.Guid();
        var jobRole = _faker.Name.JobTitle();
        var companyName = _faker.Company.CompanyName();
        var interviewPreparationContent = _faker.Lorem.Paragraph();
        var interviewQuestions = new InterviewQuestions(jobApplicationId, jobRole, companyName, interviewPreparationContent);

        var inactiveQuestion1 = new SingleInterviewQuestion(_faker.Random.Guid(), _faker.Lorem.Sentence(), _faker.Lorem.Paragraph(), _faker.Lorem.Paragraph(), false);
        var inactiveQuestion2 = new SingleInterviewQuestion(_faker.Random.Guid(), _faker.Lorem.Sentence(), _faker.Lorem.Paragraph(), _faker.Lorem.Paragraph(), false);

        interviewQuestions.AddQuestion(inactiveQuestion1);
        interviewQuestions.AddQuestion(inactiveQuestion2);

        var activeQuestions = interviewQuestions.GetActiveQuestions();

        activeQuestions.ShouldBeEmpty();
    }
}
