using CareerPilotAi.Application.CustomValidationAttributes;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace CareerPilotAi.Tests.CustomAttributesTests;

public class MinimumCountAttributeTests
{
    [Theory]
    [InlineData(1, 2)] // min 1, count 2
    [InlineData(3, 5)] // min 3, count 5
    [InlineData(2, 4)] // min 2, count 4
    public void IsValid_ShouldReturnTrue_WhenCollectionCountIsAboveMinimum(int minimumCount, int actualCount)
    {
        var attribute = new MinimumCountAttribute(minimumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>(Enumerable.Repeat("item", actualCount));

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(3, 3)] // min 3, count 3
    [InlineData(1, 1)] // min 1, count 1
    [InlineData(5, 5)] // min 5, count 5
    public void IsValid_ShouldReturnTrue_WhenCollectionCountIsEqualToMinimum(int minimumCount, int actualCount)
    {
        var attribute = new MinimumCountAttribute(minimumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>(Enumerable.Repeat("item", actualCount));

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(3, 2)] // min 3, count 2
    [InlineData(5, 3)] // min 5, count 3
    [InlineData(4, 1)] // min 4, count 1
    public void IsValid_ShouldReturnFalse_WhenCollectionCountIsBelowMinimum(int minimumCount, int actualCount)
    {
        var attribute = new MinimumCountAttribute(minimumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>(Enumerable.Repeat("item", actualCount));

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe($"At least {minimumCount} item(s) are required.");
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForEmptyCollection()
    {
        var attribute = new MinimumCountAttribute(1);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>();

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("At least 1 item(s) are required.");
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForNullCollection()
    {
        var attribute = new MinimumCountAttribute(1);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("At least 1 item(s) are required.");
    }

    [Fact]
    public void IsValid_ShouldWorkWithStringAsIEnumerable()
    {
        var attribute = new MinimumCountAttribute(10);
        var validationContext = new ValidationContext(new object());
        var value = "this is a string"; // 16 characters, min 10

        var result = attribute.GetValidationResult(value, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldFailForStringBelowMinimum()
    {
        var attribute = new MinimumCountAttribute(20);
        var validationContext = new ValidationContext(new object());
        var value = "this is a string"; // 16 characters, min 20

        var result = attribute.GetValidationResult(value, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("At least 20 item(s) are required.");
    }

    [Fact]
    public void IsValid_ShouldWorkWithArrayCollection()
    {
        var attribute = new MinimumCountAttribute(2);
        var validationContext = new ValidationContext(new object());
        var collection = new string[] { "item1", "item2", "item3" };

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldWorkWithIEnumerableCollection()
    {
        var attribute = new MinimumCountAttribute(2);
        var validationContext = new ValidationContext(new object());
        var collection = new HashSet<string> { "item1", "item2", "item3" };

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldFailWithIEnumerableCollectionBelowMinimum()
    {
        var attribute = new MinimumCountAttribute(3);
        var validationContext = new ValidationContext(new object());
        var collection = new HashSet<string> { "item1", "item2" };

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("At least 3 item(s) are required.");
    }
}
