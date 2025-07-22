---
goal: Implement Custom Email Validation Attribute with Enhanced Regex
version: 2.0
date_created: 2025-07-22
last_updated: 2025-07-22
owner: CareerPilotAi Development Team
tags: [feature, validation, security, custom-attribute, testing]
---

# Introduction

This plan outlines the implementation of a custom email validation data annotation attribute that uses enhanced regex patterns to handle email validation edge cases more effectively than the built-in `[EmailAddress]` attribute. The custom attribute will provide better validation for international emails, special characters, and RFC 5322 compliance while maintaining reusability across multiple models.

## 1. Requirements & Constraints

- **REQ-001**: Custom validation attribute must inherit from ValidationAttribute
- **REQ-002**: Implement specified regex pattern for email validation: `^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$`
- **REQ-003**: Convert JavaScript regex pattern to C# with proper escaping and compilation
- **REQ-004**: Validate against email edge cases covered by the specified regex pattern
- **REQ-005**: Provide meaningful error messages for different validation failures
- **REQ-006**: Support both client-side and server-side validation
- **REQ-007**: Maintain consistency with existing validation patterns in the project
- **SEC-001**: Prevent malicious input through email validation bypass
- **SEC-002**: Sanitize input to prevent injection attacks
- **CON-001**: Must work with existing ASP.NET Core model validation pipeline
- **CON-002**: Should not break existing RegisterViewModel functionality
- **GUD-001**: Follow C# coding standards defined in project instructions
- **GUD-002**: Use structured logging for validation failures
- **PAT-001**: Follow existing custom validation attribute patterns in the project

## 2. Implementation Steps

### Phase 1: Create Enhanced Email Validation Attribute
1. **TASK-001**: Create `EnhancedEmailAttribute.cs` in `Application/CustomValidationAttributes/`
2. **TASK-002**: Convert and implement the specified regex pattern: `^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$`
3. **TASK-003**: Add proper C# regex escaping and compilation options for performance
4. **TASK-004**: Implement server-side validation logic with specific error messages
5. **TASK-005**: Add client-side validation support through IClientValidatable interface

### Phase 2: Implement and Optimize Regex Pattern
1. **TASK-006**: Convert JavaScript regex to C# Regex pattern with proper escaping
2. **TASK-007**: Add RegexOptions.Compiled for performance optimization
3. **TASK-008**: Test regex pattern against various email formats to ensure compatibility
4. **TASK-009**: Document regex pattern components and validation coverage
5. **TASK-010**: Implement input sanitization and null/empty checks

### Phase 3: Integration and Testing
1. **TASK-011**: Replace `[EmailAddress]` with `[EnhancedEmail]` in RegisterViewModel
2. **TASK-012**: Update any other models using email validation
3. **TASK-013**: Implement comprehensive unit test suite covering all test categories (TEST-001 to TEST-027)
4. **TASK-014**: Create test data sets for valid and invalid email scenarios
5. **TASK-015**: Implement performance benchmarking tests for regex validation
6. **TASK-016**: Create integration tests with ASP.NET Core validation pipeline
7. **TASK-017**: Implement client-side validation testing
8. **TASK-018**: Create security-focused test cases for injection prevention
9. **TASK-019**: Implement comparison tests with built-in EmailAddress attribute
10. **TASK-020**: Create regression test suite for existing functionality

### Phase 4: Documentation and Optimization
1. **TASK-021**: Add XML documentation to the custom attribute
2. **TASK-022**: Execute comprehensive test suite and validate all test cases pass
3. **TASK-023**: Document regex pattern and edge cases handled
4. **TASK-024**: Performance test regex pattern with various inputs and optimize if needed
5. **TASK-025**: Add logging for validation failures if needed
6. **TASK-026**: Create test documentation explaining test coverage and scenarios
7. **TASK-027**: Generate test coverage reports and ensure minimum 95% coverage
8. **TASK-028**: Document test data organization and maintenance procedures

## 3. Alternatives

- **ALT-001**: Use FluentValidation library - Rejected due to requirement for data annotation approach
- **ALT-002**: Use third-party email validation service - Rejected due to external dependency and latency concerns
- **ALT-003**: Implement simple regex without RFC compliance - Rejected due to insufficient edge case handling
- **ALT-004**: Use multiple validation attributes chained together - Rejected due to complexity and maintainability issues

## 4. Dependencies

- **DEP-001**: System.ComponentModel.DataAnnotations namespace for ValidationAttribute
- **DEP-002**: System.Text.RegularExpressions for regex implementation
- **DEP-003**: Microsoft.AspNetCore.Mvc.ModelBinding.Validation for client-side validation
- **DEP-004**: System.Globalization for internationalization support

## 5. Files

- **FILE-001**: `Application/CustomValidationAttributes/EnhancedEmailAttribute.cs` - Main custom validation attribute
- **FILE-002**: `Models/Authentication/RegisterViewModel.cs` - Update to use new attribute
- **FILE-003**: Any other models with email properties - Update validation attributes
- **FILE-004**: `tests/CareerPilotAi.Tests/CustomAttributesTests/EnhancedEmailAttributeTests.cs` - Comprehensive unit test suite
- **FILE-005**: Test data files with valid/invalid email examples for testing
- **FILE-006**: Performance benchmark test files
- **FILE-007**: Integration test files for ASP.NET Core validation pipeline
- **FILE-008**: Documentation files explaining regex pattern and usage
- **FILE-009**: Test coverage and documentation reports

## 6. Testing

### Unit Test Requirements

#### 6.1 Valid Email Test Cases
- **TEST-001**: Standard valid emails
  - `user@example.com`, `test.email+tag@domain.co.uk`, `user123@test-domain.com`
- **TEST-002**: Emails with special characters in local part
  - `user.name@example.com`, `user_name@example.com`, `user-name@example.com`
  - `user+tag@example.com`, `123numbers@example.com`
- **TEST-003**: Quoted string local parts
  - `"test email"@example.com`, `"user@domain"@example.com`, `"spaces here"@test.com`
- **TEST-004**: IP address domains
  - `user@[192.168.1.1]`, `admin@[10.0.0.1]`, `test@[255.255.255.255]`
- **TEST-005**: International domain names
  - `user@domain.info`, `test@example.museum`, `admin@company.travel`
- **TEST-006**: Complex valid cases
  - `a@b.co`, `very.long.email.address@very.long.domain.name.com`
  - `user@sub.domain.example.com`, `test123+filter@multi-word-domain.co.uk`

#### 6.2 Invalid Email Test Cases
- **TEST-007**: Missing @ symbol
  - `userexample.com`, `test.domain.com`, `plainaddress`
- **TEST-008**: Multiple @ symbols
  - `user@domain@example.com`, `test@@domain.com`, `user@domain.com@extra`
- **TEST-009**: Invalid characters
  - `user@domain .com`, `user@domain,com`, `user@domain;com`
  - `user name@domain.com`, `user<>@domain.com`, `user[]@domain.com`
- **TEST-010**: Invalid domain formats
  - `user@`, `user@domain`, `user@.com`, `user@domain.`
  - `user@domain..com`, `user@-domain.com`, `user@domain-.com`
- **TEST-011**: Malformed IP addresses
  - `user@[192.168.1]`, `user@[999.999.999.999]`, `user@[192.168.1.1.1]`
- **TEST-012**: Empty or whitespace
  - ``, `   `, `\t`, `\n`

#### 6.3 Edge Cases and Security Tests
- **TEST-013**: Null and empty value handling
  - `null`, `string.Empty`, whitespace-only strings
- **TEST-014**: Maximum length validation
  - Emails exceeding 254 characters total length
  - Local parts exceeding 64 characters
- **TEST-015**: Security injection attempts
  - Emails containing `\0`, `\r`, `\n` characters
  - Script injection attempts like `user@domain.com<script>`
- **TEST-016**: Regex timeout handling
  - Complex patterns that could cause catastrophic backtracking
  - Extremely long strings designed to trigger timeout

#### 6.4 Validation Context Tests
- **TEST-017**: ValidationContext integration
  - Test with proper ValidationContext including DisplayName
  - Test error message formatting with different display names
- **TEST-018**: Custom error message handling
  - Test default error message
  - Test custom error message constructor
  - Test error message formatting with field names

#### 6.5 Client-Side Validation Tests
- **TEST-019**: IClientModelValidator implementation
  - Test AddValidation method with valid ClientModelValidationContext
  - Verify correct data attributes are added
  - Test null context handling
- **TEST-020**: Client-side pattern validation
  - Verify regex pattern is correctly formatted for client-side
  - Test pattern escaping for HTML attributes

#### 6.6 Performance and Reliability Tests
- **TEST-021**: Regex performance validation
  - Measure validation time for various email formats
  - Test regex compilation performance impact
  - Validate timeout settings prevent hanging
- **TEST-022**: Thread safety tests
  - Test concurrent validation calls
  - Verify static regex compilation is thread-safe

#### 6.7 Comprehensive Email Format Coverage
- **TEST-023**: RFC 5322 compliance test cases
  ```
  Valid cases:
  - simple@example.com
  - very.common@example.com
  - disposable.style.email.with+symbol@example.com
  - x@example.com
  - example@s.example
  - test@example-one.com
  - "very.unusual.@.unusual.com"@example.com
  - "very.(),:;<>[]\".VERY.\"very@\\ \"very\".unusual"@strange.example.com
  - admin@[IPv6:2001:db8::1]  (Note: May not be supported by current regex)
  
  Invalid cases:
  - plainaddress
  - @missingusername.com
  - username@.com
  - username@com
  - username..double.dot@example.com
  - username@-example.com
  - username@example-.com
  ```

#### 6.8 Integration Tests
- **TEST-024**: Model validation integration
  - Test with RegisterViewModel integration
  - Test model state validation pipeline
  - Test validation error collection and display
- **TEST-025**: ASP.NET Core validation pipeline
  - Test with ModelState.IsValid
  - Test validation error propagation
  - Test client and server validation consistency

#### 6.9 Comparison and Regression Tests
- **TEST-026**: Comparison with built-in EmailAddress attribute
  - Side-by-side validation results comparison
  - Document differences in validation behavior
  - Ensure enhanced validation is more restrictive where appropriate
- **TEST-027**: Regression test suite
  - Test previously working email formats continue to work
  - Test that security improvements don't break legitimate use cases
  - Performance regression testing

### Test Implementation Plan

#### Phase 1: Core Validation Logic Tests (TEST-001 to TEST-012)
Create comprehensive test methods covering all basic valid and invalid email scenarios using the enhanced regex pattern.

#### Phase 2: Edge Cases and Security Tests (TEST-013 to TEST-016)
Implement security-focused tests and edge case handling validation.

#### Phase 3: Framework Integration Tests (TEST-017 to TEST-020)
Test integration with ASP.NET Core validation framework and client-side validation.

#### Phase 4: Performance and Reliability Tests (TEST-021 to TEST-022)
Performance benchmarking and thread safety validation.

#### Phase 5: Comprehensive Coverage Tests (TEST-023 to TEST-025)
RFC compliance testing and full integration validation.

#### Phase 6: Comparison and Regression Tests (TEST-026 to TEST-027)
Final validation against existing solutions and regression prevention.

### Test Data Organization

#### Valid Email Test Data Categories:
1. **Basic valid formats**: Standard email patterns
2. **Special character formats**: Emails with dots, hyphens, underscores, plus signs
3. **Quoted local parts**: Emails with quoted strings in local part
4. **IP address domains**: Emails with IP addresses as domains
5. **International domains**: Various TLD formats
6. **Complex valid cases**: Edge cases that should be valid

#### Invalid Email Test Data Categories:
1. **Structural issues**: Missing @, multiple @, malformed structure
2. **Character issues**: Invalid characters, whitespace problems
3. **Domain issues**: Invalid domain formats, TLD problems
4. **Security concerns**: Injection attempts, control characters
5. **Length issues**: Emails exceeding practical limits

## 7. Risks & Assumptions

- **RISK-001**: Regex pattern performance may impact validation speed with high volume requests
- **RISK-002**: Client-side validation may not work consistently across all browsers
- **RISK-003**: Specified regex pattern may not catch all edge cases compared to RFC 5322 compliance
- **RISK-004**: JavaScript to C# regex conversion may introduce subtle differences in behavior
- **ASSUMPTION-001**: Current project structure supports custom validation attributes
- **ASSUMPTION-002**: Client-side validation is required for user experience
- **ASSUMPTION-003**: The specified regex pattern provides sufficient validation for the application's email requirements
- **ASSUMPTION-004**: Team is familiar with regex patterns and validation attribute development
- **ASSUMPTION-005**: JavaScript to C# regex conversion will maintain the same validation behavior

## 8. Related Specifications / Further Reading

- [RFC 5322 - Internet Message Format](https://tools.ietf.org/html/rfc5322)
- [ASP.NET Core Model Validation Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation)
- [Custom Validation Attributes in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation#custom-attributes)
- [Regular Expressions for Email Validation Best Practices](https://emailregex.com/)
- [Internationalized Domain Names (IDN) Specification](https://tools.ietf.org/html/rfc3490)
- [XUnit Testing Framework Documentation](https://xunit.net/)
- [Microsoft.AspNetCore.Testing Documentation](https://docs.microsoft.com/en-us/aspnet/core/test/)

## 9. Test Implementation Specification

### 9.1 Test File Structure
```csharp
// File: tests/CareerPilotAi.Tests/CustomAttributesTests/EnhancedEmailAttributeTests.cs
[TestClass]
public class EnhancedEmailAttributeTests
{
    // Test categories organized as nested classes for clarity
    [TestClass]
    public class ValidEmailTests { }
    
    [TestClass] 
    public class InvalidEmailTests { }
    
    [TestClass]
    public class EdgeCaseTests { }
    
    [TestClass]
    public class SecurityTests { }
    
    [TestClass]
    public class ClientValidationTests { }
    
    [TestClass]
    public class PerformanceTests { }
    
    [TestClass]
    public class IntegrationTests { }
}
```

### 9.2 Test Data Organization
Create static test data classes for organized email examples:

```csharp
public static class ValidEmailTestData
{
    public static readonly string[] StandardEmails = { /* ... */ };
    public static readonly string[] SpecialCharacterEmails = { /* ... */ };
    public static readonly string[] QuotedEmails = { /* ... */ };
    public static readonly string[] IpAddressEmails = { /* ... */ };
    public static readonly string[] InternationalEmails = { /* ... */ };
}

public static class InvalidEmailTestData
{
    public static readonly string[] MissingAtSymbol = { /* ... */ };
    public static readonly string[] MultipleAtSymbols = { /* ... */ };
    public static readonly string[] InvalidCharacters = { /* ... */ };
    public static readonly string[] InvalidDomains = { /* ... */ };
    public static readonly string[] MalformedIpAddresses = { /* ... */ };
}
```

### 9.3 Performance Test Requirements
- Measure validation time for 1000 emails of various complexities
- Ensure average validation time is under 1ms per email
- Test regex timeout functionality with pathological inputs
- Memory allocation testing for regex compilation

### 9.4 Test Coverage Requirements
- Minimum 95% code coverage for EnhancedEmailAttribute
- 100% coverage of all public methods and properties
- All exception paths must be tested
- All validation scenarios must be covered

### 9.5 Test Execution Strategy
1. Run unit tests in isolation for fast feedback
2. Run integration tests against ASP.NET Core pipeline
3. Run performance tests separately with detailed reporting
4. Run security tests with malicious input scenarios
5. Execute regression tests against known working email sets