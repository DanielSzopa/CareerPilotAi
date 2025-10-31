# CareerPilotAi - Comprehensive Test Plan

## Executive Summary

This document provides a comprehensive test plan for the CareerPilotAi application, a web-based job application management system built with ASP.NET Core MVC and .NET 8. The plan covers unit testing, integration testing, and end-to-end testing strategies using the specified technologies: xUnit, NSubstitute, Bogus, Shouldly, and Playwright.

---

## 1. Testing Technology Setup and Recommendations

### 1.1 xUnit Configuration

**Setup Recommendations:**
- Version: 2.4.1+ (ensure compatibility with .NET 8)
- Configure test discovery through xUnit.Runner.VisualStudio
- Use TestCaseOrderer for deterministic test execution where needed
- Set up Trait-based test categorization for organizing large test suites

**Project Structure:**
```
tests/
├── CareerPilotAi.UnitTests/
├── CareerPilotAi.IntegrationTests/
└── CareerPilotAi.E2ETests/
```

### 1.2 NSubstitute Configuration

**Best Practices:**
- Create mock factories for commonly mocked services
- Use `Substitute.For<T>()` for interface mocking
- Configure returns using `.Returns()` for synchronous operations
- Use `.Returns(x => ...)` for dynamic return values
- Implement mock verification for critical paths

### 1.3 Bogus Data Generation

**Setup:** Version 34.0+ with .NET 8 support

**Example Faker:**
```csharp
public class JobApplicationFaker : Faker<JobApplicationDataModel>
{
    public JobApplicationFaker()
    {
        RuleFor(ja => ja.JobApplicationId, f => f.Random.Guid());
        RuleFor(ja => ja.Title, f => f.Job.Title());
        RuleFor(ja => ja.Company, f => f.Company.CompanyName());
        RuleFor(ja => ja.Status, f => f.PickRandom(ApplicationStatus.ValidStatuses));
    }
}
```

### 1.4 Shouldly Assertions

**Configuration:** Version 4.1.0+

**Usage Pattern:**
```csharp
result.Should().NotBeNull();
result.ShouldBe(expectedValue);
items.ShouldHaveCount(5);
```

### 1.5 PostgreSQL Test Container Setup

**Docker Configuration:**
- Use TestContainers for PostgreSQL (port 5433 for isolation)
- Implement database initialization in test fixtures
- Use Respawn library to reset database state between tests

### 1.6 Konfiguracja E2E z Playwright i Testcontainers

Testy End-to-End (E2E) wykorzystują Playwright do interakcji z aplikacją i Testcontainers do zarządzania kontenerami Dockera. Takie podejście zapewnia w pełni izolowane środowisko testowe, które obejmuje bazę danych i samą aplikację.

**Setup:**
- `Microsoft.Playwright` - Do sterowania przeglądarką.
- `Testcontainers` - Do programistycznego zarządzania kontenerami Dockera.
- `DotNet.Testcontainers` - Dostarcza implementację Testcontainers dla .NET.

**Kluczowe kroki w konfiguracji testów E2E:**
1.  **Tworzenie sieci Dockera:** Izolowana sieć jest tworzona, aby umożliwić komunikację między kontenerem aplikacji a kontenerem bazy danych.
2.  **Uruchomienie kontenera bazy danych:** Kontener PostgreSQL jest uruchamiany w stworzonej sieci.
3.  **Budowanie i uruchamianie kontenera aplikacji:** Obraz Docker dla aplikacji jest budowany (na podstawie `Dockerfile.e2e`), a następnie kontener jest uruchamiany i dołączany do tej samej sieci.
4.  **Konfiguracja Playwright:** Playwright jest inicjalizowany, aby mógł połączyć się z aplikacją działającą w kontenerze.
---

## 5. Additional Testing Recommendations

### 5.1 Performance Testing

Consider future addition of:

1. **Load Testing**
   - Concurrent registrations: 100+ simultaneous
   - Concurrent searches: 50+ with filters
   - Interview question generation: Load on OpenRouter

2. **Database Performance**
   - Query N+1 problems: Ensure efficient loading
   - Index performance: Sorting/filtering on large datasets
   - Pagination: With 10,000+ applications

### 5.2 Security Testing

1. **CSRF Protection**
   - Verify tokens in forms
   - Test token validation

2. **Authentication Security**
   - Password hashing: Strong algorithm
   - Token expiration: Session timeout
   - Concurrent logins: Same user, multiple devices

3. **Authorization**
   - User data isolation: Users can't access others' data
   - Role-based access (if roles exist)

4. **Input Validation**
   - SQL injection in search/filters
   - XSS attempts in text fields
   - File upload security (if applicable)

### 5.4 Browser Compatibility

Playwright tests should run against:
- Chromium (primary)
- Firefox (if needed)
- WebKit (Safari equivalent)

### 5.5 CI/CD Integration

```yaml
- name: Run Unit Tests
  run: dotnet test tests/CareerPilotAi.UnitTests
  
- name: Run Integration Tests
  run: dotnet test tests/CareerPilotAi.IntegrationTests
  
- name: Run E2E Tests
  run: dotnet test tests/CareerPilotAi.E2ETests
```
---

## Conclusion

This comprehensive test plan provides detailed guidance for implementing robust testing for CareerPilotAi. The layered approach (unit → integration → E2E) ensures rapid feedback and comprehensive validation of user workflows.