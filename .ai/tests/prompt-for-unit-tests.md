You are Senior .NET Developer. Your task is to create Unit Tests for @ApplicationStatus.cs file based on below rules.

<rules>
@tests.mdc
</rules>

<test_plan>
@test-plan.md
</test_plan>

<testcases>
1.  **ApplicationStatus**
    1.1. **Constructor\_ShouldCreateValidStatus\_WhenStatusExistsInValidStatuses** - Test weryfikuje, czy konstruktor poprawnie tworzy obiekt `ApplicationStatus`, gdy podany status znajduje się na liście `ValidStatuses`.
    1.2. **Constructor\_ShouldThrowArgumentException\_WhenStatusIsInvalid** - Test sprawdza, czy konstruktor rzuca wyjątek `ArgumentException`, gdy podany status nie istnieje na liście `ValidStatuses`.
    1.3. **Constructor\_ShouldThrowArgumentException\_WhenStatusIsNull** - Test weryfikuje, czy konstruktor rzuca wyjątek `ArgumentException`, gdy podany status jest `null`.
    1.4. **Constructor\_ShouldThrowArgumentException\_WhenStatusIsEmpty** - Test sprawdza, czy konstruktor rzuca wyjątek `ArgumentException`, gdy podany status jest pustym ciągiem znaków.
    1.5. **Equality\_ShouldReturnTrue\_ForTwoInstancesWithSameStatus** - Test analizuje, czy dwa obiekty `ApplicationStatus` z takim samym statusem są sobie równe.
    1.6. **Equality\_ShouldReturnFalse\_ForTwoInstancesWithDifferentStatuses** - Test weryfikuje, czy dwa obiekty `ApplicationStatus` z różnymi statusami nie są sobie równe.
    1.7. **StaticProperties\_ShouldReturnCorrectStatusInstances** - Test sprawdza, czy statyczne właściwości (np. `ApplicationStatus.Draft`) zwracają poprawnie zainicjalizowane obiekty `ApplicationStatus`.
</testcases>

<tech_stack>
@tech-stack-tests.md
</tech_stack>

Przed implementacją testów upewnij się, że:

1. Rozumiesz dobrze test casy i logikę pliku.
2. Rozumiesz dobrany tech stack i test plan.

W przypadku jakichkolwiek pytań, pytaj mnie przed zaczęciem implementacji.