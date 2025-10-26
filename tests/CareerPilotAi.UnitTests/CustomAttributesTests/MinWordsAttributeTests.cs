using CareerPilotAi.Application.CustomValidationAttributes;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace CareerPilotAi.Tests.CustomAttributesTests;

public class MinWordsAttributeTests
{
    [Theory]
    [InlineData(3, "This is a test message")] // 5 words, min 3
    [InlineData(2, "Hello world")] // 2 words, min 2
    [InlineData(1, "One")] // 1 word, min 1
    public void IsValid_ShouldReturnTrue_WhenWordCountIsAboveMinimum(int minWords, string text)
    {
        var attribute = new MinWordsAttribute(minWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(4, "This is a test")] // 4 words, min 4
    [InlineData(2, "Hello world")] // 2 words, min 2
    [InlineData(1, "One")] // 1 word, min 1
    public void IsValid_ShouldReturnTrue_WhenWordCountIsEqualToMinimum(int minWords, string text)
    {
        var attribute = new MinWordsAttribute(minWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(5, "This is a test")] // 4 words, min 5
    [InlineData(3, "Hello world")] // 2 words, min 3
    [InlineData(2, "One")] // 1 word, min 2
    public void IsValid_ShouldReturnFalse_WhenWordCountIsBelowMinimum(int minWords, string text)
    {
        var attribute = new MinWordsAttribute(minWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe($"The field must contain at least {minWords} words.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ShouldReturnFalse_ForNullOrEmptyString(string text)
    {
        var attribute = new MinWordsAttribute(1);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("The field must contain at least 1 words.");
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForNullValue()
    {
        var attribute = new MinWordsAttribute(1);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("The field must contain at least 1 words.");
    }

    [Fact]
    public void IsValid_ShouldWorkWithNonStringValues_ByConvertingToString()
    {
        var attribute = new MinWordsAttribute(1);
        var validationContext = new ValidationContext(new object());
        var value = 123; // "123" has 1 word

        var result = attribute.GetValidationResult(value, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }
}
