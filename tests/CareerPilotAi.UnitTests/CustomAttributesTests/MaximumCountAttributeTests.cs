using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CareerPilotAi.Application.CustomValidationAttributes;
using Shouldly;
using Xunit;

namespace CareerPilotAi.Tests.CustomAttributesTests;

public class MaximumCountAttributeTests
{
    [Theory]
    [InlineData(3, 2)] // max 3, count 2
    [InlineData(5, 3)] // max 5, count 3
    [InlineData(1, 0)] // max 1, count 0
    public void IsValid_ShouldReturnTrue_WhenCollectionCountIsBelowMaximum(int maximumCount, int actualCount)
    {
        var attribute = new MaximumCountAttribute(maximumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>(Enumerable.Repeat("item", actualCount));

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(3)] // max 3, count 3
    [InlineData(1)] // max 1, count 1
    [InlineData(0)] // max 0, count 0
    public void IsValid_ShouldReturnTrue_WhenCollectionCountIsEqualToMaximum(int maximumCount)
    {
        var attribute = new MaximumCountAttribute(maximumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>(Enumerable.Repeat("item", maximumCount));

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void IsValid_ShouldReturnTrue_ForNullCollection(int maximumCount)
    {
        var attribute = new MaximumCountAttribute(maximumCount);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void IsValid_ShouldReturnTrue_ForEmptyCollection(int maximumCount)
    {
        var attribute = new MaximumCountAttribute(maximumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>();

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(3, 4)] // max 3, count 4
    [InlineData(1, 2)] // max 1, count 2
    [InlineData(0, 1)] // max 0, count 1
    public void IsValid_ShouldReturnFalse_WhenCollectionCountExceedsMaximum(int maximumCount, int actualCount)
    {
        var attribute = new MaximumCountAttribute(maximumCount);
        var validationContext = new ValidationContext(new object());
        var collection = new List<string>(Enumerable.Repeat("item", actualCount));

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe($"Maximum {maximumCount} item(s) are allowed.");
    }

    [Theory]
    [InlineData(123)]
    [InlineData(123.45)]
    public void IsValid_ShouldReturnFalse_ForNonCollectionValue(object value)
    {
        var attribute = new MaximumCountAttribute(5);
        var validationContext = new ValidationContext(new object());

        var result = attribute.GetValidationResult(value, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Value must be a collection.");
    }

    [Fact]
    public void IsValid_ShouldWorkWithArrayCollection()
    {
        var attribute = new MaximumCountAttribute(3);
        var validationContext = new ValidationContext(new object());
        var collection = new string[] { "item1", "item2" };

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldWorkWithIEnumerableCollection()
    {
        var attribute = new MaximumCountAttribute(3);
        var validationContext = new ValidationContext(new object());
        var collection = new HashSet<string> { "item1", "item2" };

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldFailWithIEnumerableCollectionExceedingMaximum()
    {
        var attribute = new MaximumCountAttribute(2);
        var validationContext = new ValidationContext(new object());
        var collection = new HashSet<string> { "item1", "item2", "item3" };

        var result = attribute.GetValidationResult(collection, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Maximum 2 item(s) are allowed.");
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_ForStringValue()
    {
        var attribute = new MaximumCountAttribute(10);
        var validationContext = new ValidationContext(new object());
        var value = "this is a string";

        var result = attribute.GetValidationResult(value, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Maximum 10 item(s) are allowed.");
    }
}
