# Unit Tests
- Testing framework: xUnit
- Mocking library: NSubstitute
- Fake data: Bogus
- Assertions: Shouldly or Default Assertions

# Integration Tests
- Testing framework: xUnit
- Mocking library: NSubstitute
- Fake data: Bogus
- Assertions: Shouldly or Default Assertions
- Real database: PostgreSQL in test container
- Use Respawn library to reset the database between tests


# End to End Tests
- Testing framework: Playwright (C#)