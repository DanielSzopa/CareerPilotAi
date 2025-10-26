using CareerPilotAi.Application.CustomValidationAttributes;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace CareerPilotAi.Tests.CustomAttributesTests;

public class SalaryTypeRequiredIfSalaryProvidedAttributeTests
{
    private class TestModel
    {
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? SalaryType { get; set; }
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenSalaryMinIsProvidedWithType()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMin = 50000, SalaryType = "Gross" };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenSalaryMaxIsProvidedWithType()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMax = 70000, SalaryType = "Net" };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenBothSalariesAreProvidedWithType()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMin = 50000, SalaryMax = 70000, SalaryType = "Gross" };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenSalaryMinIsProvidedWithoutType()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMin = 50000, SalaryType = null };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Salary type is required when salary is specified.");
        result.MemberNames.ShouldContain("SalaryType");
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenSalaryMaxIsProvidedWithoutType()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMax = 70000, SalaryType = null };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Salary type is required when salary is specified.");
        result.MemberNames.ShouldContain("SalaryType");
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenNoSalaryIsProvided()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMin = null, SalaryMax = null, SalaryType = null };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenOnlySalaryTypeIsProvided()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMin = null, SalaryMax = null, SalaryType = "Gross" };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenBothSalariesAreProvidedWithoutType()
    {
        var attribute = new SalaryTypeRequiredIfSalaryProvidedAttribute();
        var model = new TestModel { SalaryMin = 50000, SalaryMax = 70000, SalaryType = null };
        var validationContext = new ValidationContext(model);

        var result = attribute.GetValidationResult(null, validationContext);

        result.ShouldNotBe(ValidationResult.Success);
        result.ShouldNotBeNull();
        result.ErrorMessage.ShouldBe("Salary type is required when salary is specified.");
        result.MemberNames.ShouldContain("SalaryType");
    }
}
