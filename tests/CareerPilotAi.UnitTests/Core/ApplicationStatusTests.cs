using CareerPilotAi.Core;


namespace CareerPilotAi.UnitTests.Core;

public class ApplicationStatusTests
{
    [Theory]
    [InlineData("Draft")]
    [InlineData("Rejected")]
    [InlineData("Submitted")]
    [InlineData("Interview Scheduled")]
    [InlineData("Waiting for offer")]
    [InlineData("Received offer")]
    [InlineData("No contact")]
    public void Constructor_ShouldCreateValidStatus_WhenStatusExistsInValidStatuses(string validStatus)
    {
        var applicationStatus = new ApplicationStatus(validStatus);

        applicationStatus.ShouldNotBeNull();
        applicationStatus.Status.ShouldBe(validStatus);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenStatusIsInvalid()
    {
        var faker = new Faker();
        var exception = Should.Throw<ArgumentException>(() => new ApplicationStatus(faker.Random.String2(10)));
        exception.Message.ShouldContain("Invalid status");
    }

    [Fact]
    public void StaticProperties_ShouldReturnCorrectStatusInstances()
    {
        ApplicationStatus.Draft.Status.ShouldBe("Draft");
        ApplicationStatus.Rejected.Status.ShouldBe("Rejected");
        ApplicationStatus.Submitted.Status.ShouldBe("Submitted");
        ApplicationStatus.InterviewScheduled.Status.ShouldBe("Interview Scheduled");
        ApplicationStatus.WaitingForOffer.Status.ShouldBe("Waiting for offer");
        ApplicationStatus.ReceivedOffer.Status.ShouldBe("Received offer");
        ApplicationStatus.NoContact.Status.ShouldBe("No contact");
    }

    [Fact]
    public void ValidStatuses_ShouldContainAllExpectedStatuses()
    {
        var expectedStatuses = new[]
        {
            "Draft",
            "Rejected",
            "Submitted",
            "Interview Scheduled",
            "Waiting for offer",
            "Received offer",
            "No contact"
        };

        ApplicationStatus.ValidStatuses.ShouldNotBeNull();
        ApplicationStatus.ValidStatuses.Count.ShouldBe(expectedStatuses.Length);
        foreach (var expectedStatus in expectedStatuses)
        {
            ApplicationStatus.ValidStatuses.ShouldContain(expectedStatus);
        }
    }

    [Fact]
    public void DefaultStatus_ShouldBeDraft()
    {
        ApplicationStatus.DefaultStatus.ShouldBe("Draft");
    }
}
