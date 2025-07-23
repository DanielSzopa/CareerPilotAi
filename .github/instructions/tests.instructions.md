---
applyTo: 'tests/**/*.cs'
---

# Packages, frameworks and libraries
- Use `xUnit` for unit testing framework
- Use `Substitute` for mocking dependencies in tests
- Use basic Assertions from xUnit if possible or `Shouldly` for more complex assertions
- Dummy data should be created using `Bogus` library

# Conventions
- Use arrangement, act, assert (AAA) pattern in tests
- Use `Fact` for simple tests and `Theory` for parameterized tests
- Use descriptive test names: `MethodName_StateUnderTest_ExpectedBehavior`