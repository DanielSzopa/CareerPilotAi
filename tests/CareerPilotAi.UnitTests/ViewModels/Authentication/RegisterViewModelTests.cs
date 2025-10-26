using System.ComponentModel.DataAnnotations;
using CareerPilotAi.ViewModels.Authentication;
using Shouldly;
using Xunit;

namespace CareerPilotAi.Tests.ViewModels.Authentication;

public class RegisterViewModelTests
{
    [Fact]
    public void Validation_ShouldBeValid_WithCorrectData()
    {
        var model = new RegisterViewModel
        {
            Email = "test@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        var validationResults = ValidateModel(model);

        validationResults.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    [InlineData("test.example.com")]
    public void Validation_ShouldBeInvalid_WhenEmailIsIncorrect(string invalidEmail)
    {
        var model = new RegisterViewModel
        {
            Email = invalidEmail,
            Password = "password123",
            ConfirmPassword = "password123"
        };

        var validationResults = ValidateModel(model);

        validationResults.ShouldNotBeEmpty();
        validationResults.ShouldContain(r => r.MemberNames.Contains("Email"));
    }

    [Theory]
    [InlineData("short")]
    [InlineData("1234567")]
    [InlineData("")]
    public void Validation_ShouldBeInvalid_WhenPasswordIsTooShort(string shortPassword)
    {
        var model = new RegisterViewModel
        {
            Email = "test@example.com",
            Password = shortPassword,
            ConfirmPassword = shortPassword
        };

        var validationResults = ValidateModel(model);

        validationResults.ShouldNotBeEmpty();
        validationResults.ShouldContain(r => r.MemberNames.Contains("Password"));
    }

    [Fact]
    public void Validation_ShouldBeInvalid_WhenPasswordsDoNotMatch()
    {
        var model = new RegisterViewModel
        {
            Email = "test@example.com",
            Password = "password123",
            ConfirmPassword = "differentpassword"
        };

        var validationResults = ValidateModel(model);

        validationResults.ShouldNotBeEmpty();
        validationResults.ShouldContain(r => r.MemberNames.Contains("ConfirmPassword"));
    }

    [Theory]
    [InlineData(null, "password123", "password123")]
    [InlineData("test@example.com", null, "password123")]
    [InlineData("test@example.com", "password123", null)]
    public void Validation_ShouldBeInvalid_WhenRequiredFieldsAreNull(string email, string password, string confirmPassword)
    {
        var model = new RegisterViewModel
        {
            Email = email!,
            Password = password!,
            ConfirmPassword = confirmPassword!
        };

        var validationResults = ValidateModel(model);

        validationResults.ShouldNotBeEmpty();
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}
