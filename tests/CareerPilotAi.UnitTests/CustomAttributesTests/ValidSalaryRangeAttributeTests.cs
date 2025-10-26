using CareerPilotAi.Application.CustomValidationAttributes;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace CareerPilotAi.Tests.CustomAttributesTests;

public class ValidSalaryRangeAttributeTests
{
    private class TestModel
    {
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenSalaryMaxIsEqualToSalaryMin()
    {
        var attribute = new ValidSalaryRangeAttribute();
        var model = new TestModel { SalaryMin = 60000, SalaryMax = 60000 };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }
    
    [Fact]
    public void IsValid_ShouldReturnTrue_WhenOnlySalaryMinIsProvided()
    {
        var attribute = new ValidSalaryRangeAttribute();
        var model = new TestModel { SalaryMin = 50000, SalaryMax = null };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenOnlySalaryMaxIsProvided()
    {
        var attribute = new ValidSalaryRangeAttribute();
        var model = new TestModel { SalaryMin = null, SalaryMax = 70000 };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenBothSalariesAreNull()
    {
        var attribute = new ValidSalaryRangeAttribute();
        var model = new TestModel { SalaryMin = null, SalaryMax = null };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(50000, 60000)]
    [InlineData(100000, 150000)]
    [InlineData(0, 1000)]
    public void IsValid_ShouldReturnTrue_ForVariousValidRanges(decimal min, decimal max)
    {
        var attribute = new ValidSalaryRangeAttribute();
        var model = new TestModel { SalaryMin = min, SalaryMax = max };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Theory]
    [InlineData(70000, 50000)]
    [InlineData(100000, 50000)]
    [InlineData(1000, 0)]
    public void IsValid_ShouldReturnFalse_ForVariousInvalidRanges(decimal min, decimal max)
    {
        var attribute = new ValidSalaryRangeAttribute();
        var model = new TestModel { SalaryMin = min, SalaryMax = max };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Maximum salary must be greater than or equal to minimum salary.");
        result.MemberNames.ShouldContain("SalaryMax");
    }
}
