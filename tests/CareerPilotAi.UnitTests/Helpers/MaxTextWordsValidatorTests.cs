using CareerPilotAi.Application.Helpers;

namespace CareerPilotAi.Tests.Helpers;

public class MaxTextWordsValidatorTests
{
    [Theory]
    [InlineData(5, "This is a test")] // 4 words, max 5
    [InlineData(10, "Hello world")] // 2 words, max 10
    [InlineData(3, "One")] // 1 word, max 3
    public void Validate_ShouldReturnTrue_WhenWordCountIsBelowLimit(int maxWords, string text)
    {
        var result = MaxTextWordsValidator.ValidateText(text, maxWords);

        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData(4, "This is a test")] // 4 words, max 4
    [InlineData(2, "Hello world")] // 2 words, max 2
    [InlineData(1, "One")] // 1 word, max 1
    public void Validate_ShouldReturnTrue_WhenWordCountIsEqualToLimit(int maxWords, string text)
    {
        var result = MaxTextWordsValidator.ValidateText(text, maxWords);

        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData(3, "This is a test message")] // 5 words, max 3
    [InlineData(1, "Hello world")] // 2 words, max 1
    [InlineData(2, "One two three")] // 3 words, max 2
    public void Validate_ShouldReturnFalse_WhenWordCountExceedsLimit(int maxWords, string text)
    {
        var result = MaxTextWordsValidator.ValidateText(text, maxWords);

        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("This  is\ta\ntest\r\nmessage", 5)] // Multiple spaces, tabs, newlines - 5 words
    [InlineData("Hello   world\t\ntest", 3)] // Multiple spaces and tabs - 3 words
    [InlineData("One\r\n\r\ntwo", 2)] // Multiple newlines - 2 words
    [InlineData("   spaced   out   ", 2)] // Multiple spaces at start/end - 2 words
    public void Validate_ShouldCorrectlyCountWords_WithVariousWhitespace(string text, int expectedWordCount)
    {
        // First verify the word count is as expected
        var wordCount = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        wordCount.ShouldBe(expectedWordCount);

        // Then test validation passes for limit equal to word count
        var result = MaxTextWordsValidator.ValidateText(text, expectedWordCount);
        result.ShouldBeTrue();

        // And fails for limit below word count
        if (expectedWordCount > 0)
        {
            var resultBelow = MaxTextWordsValidator.ValidateText(text, expectedWordCount - 1);
            resultBelow.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t\n\r")]
    public void Validate_ShouldReturnFalse_ForEmptyStringOrNull(string? text)
    {
        var result = MaxTextWordsValidator.ValidateText(text!, 5);

        result.ShouldBeFalse();
    }
}
