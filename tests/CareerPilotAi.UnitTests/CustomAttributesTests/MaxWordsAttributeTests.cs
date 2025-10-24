using System.ComponentModel.DataAnnotations;
using CareerPilotAi.Application.CustomValidationAttributes;
using Shouldly;
using Xunit;

namespace CareerPilotAi.Tests.CustomAttributesTests;

public class MaxWordsAttributeTests
{
    [Theory]
    [InlineData(5, "This is a test")] // 4 words, max 5
    [InlineData(10, "Hello world")] // 2 words, max 10
    [InlineData(3, "One")] // 1 word, max 3
    public void IsValid_ShouldReturnTrue_WhenWordCountIsBelowLimit(int maxWords, string text)
    {
        var attribute = new MaxWordsAttribute(maxWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(4, "This is a test")] // 4 words, max 4
    [InlineData(2, "Hello world")] // 2 words, max 2
    [InlineData(1, "One")] // 1 word, max 1
    public void IsValid_ShouldReturnTrue_WhenWordCountIsEqualToLimit(int maxWords, string text)
    {
        var attribute = new MaxWordsAttribute(maxWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(3, "This is a test message")] // 5 words, max 3
    [InlineData(1, "Hello world")] // 2 words, max 1
    [InlineData(2, "One two three")] // 3 words, max 2
    public void IsValid_ShouldReturnFalse_WhenWordCountExceedsLimit(int maxWords, string text)
    {
        var attribute = new MaxWordsAttribute(maxWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe($"The field cannot contain more than {maxWords} words.");
    }

    [Theory]
    [InlineData(5, "This  is\ta\ntest\r\nmessage")] // Multiple spaces, tabs, newlines - 4 words
    [InlineData(3, "Hello   world\t\ntest")] // Multiple spaces and tabs - 3 words
    [InlineData(2, "One\r\n\r\ntwo")] // Multiple newlines - 2 words
    public void IsValid_ShouldCountWordsCorrectly_WithMultipleSpacesAndNewlines(int maxWords, string text)
    {
        var attribute = new MaxWordsAttribute(maxWords);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ShouldReturnSuccess_ForNullOrEmptyString(string text)
    {
        var attribute = new MaxWordsAttribute(5);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(text, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }
}
