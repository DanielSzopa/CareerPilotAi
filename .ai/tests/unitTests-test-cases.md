Jasne, oto lista przypadków testowych dla każdego pliku, oparta na dostarczonym planie testów i analizie kodu.

### `Core/ApplicationStatusTests.cs`

1.  **ApplicationStatus**
    1.1. **Constructor\_ShouldCreateValidStatus\_WhenStatusExistsInValidStatuses** - Test weryfikuje, czy konstruktor poprawnie tworzy obiekt `ApplicationStatus`, gdy podany status znajduje się na liście `ValidStatuses`.
    1.2. **Constructor\_ShouldThrowArgumentException\_WhenStatusIsInvalid** - Test sprawdza, czy konstruktor rzuca wyjątek `ArgumentException`, gdy podany status nie istnieje na liście `ValidStatuses`.
    1.3. **Constructor\_ShouldThrowArgumentException\_WhenStatusIsNull** - Test weryfikuje, czy konstruktor rzuca wyjątek `ArgumentException`, gdy podany status jest `null`.
    1.4. **Constructor\_ShouldThrowArgumentException\_WhenStatusIsEmpty** - Test sprawdza, czy konstruktor rzuca wyjątek `ArgumentException`, gdy podany status jest pustym ciągiem znaków.
    1.5. **Equality\_ShouldReturnTrue\_ForTwoInstancesWithSameStatus** - Test analizuje, czy dwa obiekty `ApplicationStatus` z takim samym statusem są sobie równe.
    1.6. **Equality\_ShouldReturnFalse\_ForTwoInstancesWithDifferentStatuses** - Test weryfikuje, czy dwa obiekty `ApplicationStatus` z różnymi statusami nie są sobie równe.
    1.7. **StaticProperties\_ShouldReturnCorrectStatusInstances** - Test sprawdza, czy statyczne właściwości (np. `ApplicationStatus.Draft`) zwracają poprawnie zainicjalizowane obiekty `ApplicationStatus`.

### `Core/InterviewQuestionsTests.cs`

1.  **InterviewQuestions**
    1.1. **Constructor\_ShouldCreateInstance\_WithValidParameters** - Test weryfikuje, czy można utworzyć instancję `InterviewQuestions` z poprawnymi danymi (ID aplikacji, rola, firma, treść przygotowania).
    1.2. **Constructor\_ShouldThrowException\_WhenJobApplicationIdIsEmpty** - Test sprawdza, czy konstruktor rzuca wyjątek, gdy `jobApplicationId` jest pustym `Guid`.
    1.3. **Constructor\_ShouldThrowException\_WhenJobRoleIsEmpty** - Test sprawdza, czy konstruktor rzuca wyjątek, gdy `jobRole` jest `null` lub pusty.
    1.4. **Constructor\_ShouldThrowException\_WhenCompanyNameIsEmpty** - Test sprawdza, czy konstruktor rzuca wyjątek, gdy `companyName` jest `null` lub pusty.
    1.5. **Constructor\_ShouldThrowException\_WhenInterviewPreparationContentIsEmpty** - Test sprawdza, czy konstruktor rzuca wyjątek, gdy `interviewPreparationContent` jest `null` lub pusty.
    1.6. **AddQuestion\_ShouldAddQuestionToList\_WhenQuestionIsValid** - Test weryfikuje, czy metoda `AddQuestion` poprawnie dodaje nowe, prawidłowe pytanie do wewnętrznej listy pytań.
    1.7. **AddQuestion\_ShouldNotAddNullQuestion\_AndShouldNotThrowException** - Test sprawdza, czy próba dodania `null` jako pytania nie powoduje błędu i nie dodaje niczego do listy.
    1.8. **GetActiveQuestions\_ShouldReturnOnlyActiveQuestions** - Test analizuje, czy metoda `GetActiveQuestions` zwraca tylko te pytania, które nie zostały oznaczone jako usunięte.
    1.9. **GetActiveQuestions\_ShouldReturnEmptyList\_WhenNoActiveQuestionsExist** - Test sprawdza, czy `GetActiveQuestions` zwraca pustą listę, gdy wszystkie pytania zostały usunięte lub lista jest pusta.

### `CustomValidationAttributes/MaximumCountAttributeTests.cs`

1.  **MaximumCountAttribute**
    1.1. **IsValid\_ShouldReturnTrue\_WhenCollectionCountIsBelowMaximum** - Test weryfikuje, czy walidacja przechodzi pomyślnie, gdy liczba elementów w kolekcji jest mniejsza niż dozwolone maksimum.
    1.2. **IsValid\_ShouldReturnTrue\_WhenCollectionCountIsEqualToMaximum** - Test sprawdza, czy walidacja jest poprawna, gdy liczba elementów jest dokładnie równa maksimum.
    1.3. **IsValid\_ShouldReturnTrue\_ForNullCollection** - Test weryfikuje, czy atrybut akceptuje wartość `null` jako prawidłową.
    1.4. **IsValid\_ShouldReturnTrue\_ForEmptyCollection** - Test sprawdza, czy pusta kolekcja jest poprawnie walidowana (gdy `MaximumCount` > 0).
    1.5. **IsValid\_ShouldReturnFalse\_WhenCollectionCountExceedsMaximum** - Test analizuje, czy walidacja kończy się niepowodzeniem, gdy liczba elementów w kolekcji przekracza maksimum.
    1.6. **IsValid\_ShouldReturnFalse\_ForNonCollectionValue** - Test sprawdza, czy atrybut zwraca błąd walidacji, gdy podana wartość nie jest kolekcją.

### `CustomValidationAttributes/MaxWordsAttributeTests.cs`

1.  **MaxWordsAttribute**
    1.1. **IsValid\_ShouldReturnTrue\_WhenWordCountIsBelowLimit** - Test weryfikuje, czy tekst z liczbą słów mniejszą niż limit przechodzi walidację.
    1.2. **IsValid\_ShouldReturnTrue\_WhenWordCountIsEqualToLimit** - Test sprawdza, czy tekst z liczbą słów równą limitowi jest poprawnie walidowany.
    1.3. **IsValid\_ShouldReturnFalse\_WhenWordCountExceedsLimit** - Test analizuje, czy tekst z liczbą słów przekraczającą limit jest uznawany za nieprawidłowy.
    1.4. **IsValid\_ShouldCountWordsCorrectly\_WithMultipleSpacesAndNewlines** - Test sprawdza, czy atrybut poprawnie liczy słowa, ignorując wielokrotne spacje, tabulatory i znaki nowej linii.
    1.5. **IsValid\_ShouldReturnTrue\_ForNullOrEmptyString** - Test weryfikuje, czy `null` lub pusty ciąg znaków jest uznawany za prawidłowy (ponieważ nie przekracza limitu słów).
    1.6. **IsValid\_ShouldReturnFalse\_ForNonStringValue** - Test sprawdza, czy atrybut zwraca `false`, gdy wartość nie jest ciągiem znaków.

### `CustomValidationAttributes/MinWordsAttributeTests.cs`

1.  **MinWordsAttribute**
    1.1. **IsValid\_ShouldReturnTrue\_WhenWordCountIsAboveMinimum** - Test weryfikuje, czy tekst z liczbą słów większą niż wymagane minimum przechodzi walidację.
    1.2. **IsValid\_ShouldReturnTrue\_WhenWordCountIsEqualToMinimum** - Test sprawdza, czy tekst z liczbą słów równą minimum jest poprawnie walidowany.
    1.3. **IsValid\_ShouldReturnFalse\_WhenWordCountIsBelowMinimum** - Test analizuje, czy tekst z liczbą słów mniejszą niż minimum jest uznawany za nieprawidłowy.
    1.4. **IsValid\_ShouldReturnFalse\_ForEmptyString** - Test weryfikuje, czy pusty ciąg znaków jest nieprawidłowy (ponieważ liczba słów wynosi 0).
    1.5. **IsValid\_ShouldReturnTrue\_ForNullValue** - Test sprawdza, czy `null` jest traktowany jako prawidłowa wartość (zgodnie z domyślnym zachowaniem atrybutów walidacji).
    1.6. **IsValid\_ShouldReturnFalse\_ForNonStringValue** - Test sprawdza, czy atrybut zwraca `false`, gdy wartość nie jest ciągiem znaków.

### `CustomValidationAttributes/MinimumCountAttributeTests.cs`

1.  **MinimumCountAttribute**
    1.1. **IsValid\_ShouldReturnTrue\_WhenCollectionCountIsAboveMinimum** - Test weryfikuje, czy kolekcja z liczbą elementów większą niż minimum przechodzi walidację.
    1.2. **IsValid\_ShouldReturnTrue\_WhenCollectionCountIsEqualToMinimum** - Test sprawdza, czy kolekcja z liczbą elementów równą minimum jest poprawnie walidowana.
    1.3. **IsValid\_ShouldReturnFalse\_WhenCollectionCountIsBelowMinimum** - Test analizuje, czy kolekcja z liczbą elementów mniejszą niż minimum jest uznawana za nieprawidłową.
    1.4. **IsValid\_ShouldReturnFalse\_ForEmptyCollection** - Test weryfikuje, czy pusta kolekcja jest nieprawidłowa (gdy `MinimumCount` > 0).
    1.5. **IsValid\_ShouldReturnFalse\_ForNullCollection** - Test sprawdza, czy `null` jest traktowany jako nieprawidłowa wartość, ponieważ nie jest to kolekcja spełniająca minimum.
    1.6. **IsValid\_ShouldReturnFalse\_ForNonCollectionValue** - Test sprawdza, czy atrybut zwraca błąd walidacji, gdy podana wartość nie jest kolekcją.

### `CustomValidationAttributes/SalaryTypeRequiredIfSalaryProvidedAttributeTests.cs`

1.  **SalaryTypeRequiredIfSalaryProvidedAttribute**
    1.1. **IsValid\_ShouldReturnTrue\_WhenSalaryMinIsProvidedWithType** - Test weryfikuje, czy walidacja przechodzi, gdy podano `SalaryMin` i `SalaryType`.
    1.2. **IsValid\_ShouldReturnTrue\_WhenSalaryMaxIsProvidedWithType** - Test sprawdza, czy walidacja jest poprawna, gdy podano `SalaryMax` i `SalaryType`.
    1.3. **IsValid\_ShouldReturnTrue\_WhenBothSalariesAreProvidedWithType** - Test weryfikuje, czy walidacja przechodzi, gdy podano obie wartości wynagrodzenia oraz `SalaryType`.
    1.4. **IsValid\_ShouldReturnFalse\_WhenSalaryMinIsProvidedWithoutType** - Test analizuje, czy walidacja kończy się niepowodzeniem, gdy podano `SalaryMin`, ale `SalaryType` jest `null`.
    1.5. **IsValid\_ShouldReturnFalse\_WhenSalaryMaxIsProvidedWithoutType** - Test sprawdza, czy walidacja zwraca błąd, gdy podano `SalaryMax`, ale `SalaryType` jest `null`.
    1.6. **IsValid\_ShouldReturnTrue\_WhenNoSalaryIsProvided** - Test weryfikuje, czy walidacja jest pomyślna, gdy żadna z wartości wynagrodzenia nie jest podana.

### `CustomValidationAttributes/ValidSalaryRangeAttributeTests.cs`

1.  **ValidSalaryRangeAttribute**
    1.1. **IsValid\_ShouldReturnTrue\_WhenSalaryMaxIsGreaterThanSalaryMin** - Test weryfikuje, czy prawidłowy zakres wynagrodzeń (`SalaryMax` > `SalaryMin`) przechodzi walidację.
    1.2. **IsValid\_ShouldReturnTrue\_WhenSalaryMaxIsEqualToSalaryMin** - Test sprawdza, czy walidacja jest poprawna, gdy `SalaryMax` jest równe `SalaryMin`.
    1.3. **IsValid\_ShouldReturnFalse\_WhenSalaryMaxIsLessThanSalaryMin** - Test analizuje, czy walidacja kończy się niepowodzeniem, gdy `SalaryMax` jest mniejsze niż `SalaryMin`.
    1.4. **IsValid\_ShouldReturnTrue\_WhenOnlySalaryMinIsProvided** - Test weryfikuje, czy walidacja przechodzi, gdy podano tylko `SalaryMin`.
    1.5. **IsValid\_ShouldReturnTrue\_WhenOnlySalaryMaxIsProvided** - Test sprawdza, czy walidacja jest poprawna, gdy podano tylko `SalaryMax`.
    1.6. **IsValid\_ShouldReturnTrue\_WhenBothSalariesAreNull** - Test weryfikuje, czy walidacja przechodzi pomyślnie, gdy obie wartości wynagrodzenia są `null`.

### `Helpers/MaxTextWordsValidatorTests.cs`

1.  **MaxTextWordsValidator**
    1.1. **Validate\_ShouldReturnTrue\_WhenWordCountIsBelowLimit** - Test weryfikuje, czy metoda `Validate` zwraca `true`, gdy liczba słów w tekście jest mniejsza niż limit.
    1.2. **Validate\_ShouldReturnTrue\_WhenWordCountIsEqualToLimit** - Test sprawdza, czy metoda zwraca `true`, gdy liczba słów jest równa limitowi.
    1.3. **Validate\_ShouldReturnFalse\_WhenWordCountExceedsLimit** - Test analizuje, czy metoda zwraca `false`, gdy liczba słów przekracza limit.
    1.4. **Validate\_ShouldReturnFalse\_ForNullInput** - Test weryfikuje, czy `null` jako wejście powoduje zwrócenie `false`.
    1.5. **Validate\_ShouldReturnFalse\_ForEmptyString** - Test sprawdza, czy pusty ciąg znaków jest traktowany jako nieprawidłowy (lub prawidłowy, w zależności od logiki biznesowej – test plan zakłada `false`).
    1.6. **Validate\_ShouldCorrectlyCountWords\_WithVariousWhitespace** - Test sprawdza, czy metoda poprawnie liczy słowa, niezależnie od użycia wielu spacji, tabulatorów czy znaków nowej linii.

### `Services/ClockTests.cs`

1.  **Clock**
    1.1. **GetDateTimeAdjustedToTimeZone\_ShouldReturnSameDateTime\_ForUtcTimeZone** - Test weryfikuje, czy dla strefy czasowej "UTC" data i czas nie ulegają zmianie.
    1.2. **GetDateTimeAdjustedToTimeZone\_ShouldReturnCorrectlyOffsetDateTime\_ForEst** - Test sprawdza, czy dla strefy "America/New_York" (EST) czas jest poprawnie przesunięty względem UTC.
    1.3. **GetDateTimeAdjustedToTimeZone\_ShouldHandleDaylightSavingCorrectly** - Test analizuje, czy metoda poprawnie uwzględnia czas letni dla danej strefy czasowej.
    1.4. **GetDateTimeAdjustedToTimeZone\_ShouldHandleHalfHourOffsets** - Test weryfikuje, czy strefy czasowe z przesunięciem o 30 minut (np. w Indiach) są poprawnie obsługiwane.
    1.5. **GetDateTimeAdjustedToTimeZone\_ShouldThrowException\_ForInvalidTimeZone** - Test sprawdza, czy podanie nieprawidłowego identyfikatora strefy czasowej rzuca wyjątek.

### `Services/TimeZoneServiceTests.cs`

1.  **TimeZoneService**
    1.1. **GetAllAsync\_ShouldReturnAllTimeZonesFromDatabase** - Test weryfikuje, czy metoda zwraca wszystkie strefy czasowe, które zostały dodane do bazy danych w teście.
    1.2. **GetAllAsync\_ShouldReturnTimeZonesOrderedByName** - Test sprawdza, czy zwrócone strefy czasowe są posortowane alfabetycznie według nazwy.
    1.3. **GetAllAsync\_ShouldReturnEmptyList\_WhenDatabaseIsEmpty** - Test analizuje, czy metoda zwraca pustą listę, gdy w bazie danych nie ma żadnych stref czasowych.
    1.4. **ExistsAsync\_ShouldReturnTrue\_ForExistingTimeZone** - Test weryfikuje, czy metoda zwraca `true` dla identyfikatora strefy czasowej, która istnieje w bazie danych.
    1.5. **ExistsAsync\_ShouldReturnFalse\_ForNonExistingTimeZone** - Test sprawdza, czy metoda zwraca `false` dla identyfikatora strefy, której nie ma w bazie danych.
    1.6. **ExistsAsync\_ShouldReturnFalse\_ForNullOrEmptyId** - Test weryfikuje, czy `null` lub pusty ciąg znaków jako ID strefy czasowej powoduje zwrócenie `false`.

### `Services/UserServiceTests.cs`

1.  **UserService**
    1.1. **GetUserIdOrThrowException\_ShouldReturnUserId\_WhenUserIsAuthenticated** - Test weryfikuje, czy metoda poprawnie zwraca ID użytkownika z `ClaimsPrincipal`, gdy użytkownik jest zalogowany.
    1.2. **GetUserIdOrThrowException\_ShouldThrowUserIdDoesNotExist\_WhenHttpContextIsNull** - Test sprawdza, czy rzucany jest wyjątek `UserIdDoesNotExist`, gdy `HttpContext` jest `null`.
    1.3. **GetUserIdOrThrowException\_ShouldThrowUserIdDoesNotExist\_WhenUserIsNull** - Test weryfikuje, czy wyjątek jest rzucany, gdy `User` w `HttpContext` jest `null`.
    1.4. **GetUserIdOrThrowException\_ShouldThrowUserIdDoesNotExist\_WhenNameIdentifierClaimIsMissing** - Test analizuje, czy wyjątek jest rzucany, gdy w `ClaimsPrincipal` brakuje `NameIdentifier`.
    1.5. **GetUserIdOrThrowException\_ShouldThrowUserIdDoesNotExist\_WhenNameIdentifierIsEmpty** - Test sprawdza, czy pusty `NameIdentifier` również powoduje rzucenie wyjątku.

### `ViewModels/Authentication/RegisterViewModelTests.cs`

1.  **RegisterViewModel**
    1.1. **Validation\_ShouldBeValid\_WithCorrectData** - Test weryfikuje, czy model jest prawidłowy, gdy podano poprawny e-mail oraz pasujące do siebie hasła o odpowiedniej długości.
    1.2. **Validation\_ShouldBeInvalid\_WhenEmailIsIncorrect** - Test sprawdza, czy model jest nieprawidłowy, gdy adres e-mail ma zły format.
    1.3. **Validation\_ShouldBeInvalid\_WhenPasswordIsTooShort** - Test analizuje, czy hasło krótsze niż 8 znaków powoduje błąd walidacji.
    1.4. **Validation\_ShouldBeInvalid\_WhenPasswordsDoNotMatch** - Test weryfikuje, czy błąd walidacji występuje, gdy `Password` i `ConfirmPassword` nie są takie same.
    1.5. **Validation\_ShouldBeInvalid\_WhenRequiredFieldsAreNull** - Test sprawdza, czy brak wartości w wymaganych polach (Email, Password, ConfirmPassword) powoduje błędy walidacji.

### `ViewModels/JobApplication/CreateJobApplicationViewModelTests.cs`

1.  **CreateJobApplicationViewModel**
    1.1. **Validation\_ShouldBeValid\_WithMinimumRequiredData** - Test weryfikuje, czy model jest prawidłowy, gdy wypełniono tylko wymagane pola (np. Company, Position, JobDescription).
    1.2. **Company\_Validation\_ShouldBeInvalid\_WhenTooShortOrTooLong** - Test sprawdza, czy nazwa firmy krótsza niż 2 znaki lub dłuższa niż 100 znaków powoduje błąd walidacji.
    1.3. **JobDescription\_Validation\_ShouldBeInvalid\_WhenWordCountIsOutOfRange** - Test analizuje, czy opis stanowiska mający mniej niż 50 słów lub więcej niż 5000 słów jest nieprawidłowy.
    1.4. **Skills\_Validation\_ShouldBeInvalid\_WhenCountExceedsTwenty** - Test weryfikuje, czy dodanie więcej niż 20 umiejętności powoduje błąd walidacji.
    1.5. **Salary\_Validation\_ShouldBeInvalid\_WhenMaxIsLessThanMin** - Test sprawdza, czy walidacja wynagrodzenia kończy się niepowodzeniem, gdy `SalaryMax` jest mniejsze niż `SalaryMin`.
    1.6. **Salary\_Validation\_ShouldBeInvalid\_WhenSalaryIsProvidedWithoutType** - Test analizuje, czy podanie kwoty wynagrodzenia bez określenia `SalaryType` powoduje błąd walidacji.
    1.7. **Url\_Validation\_ShouldBeInvalid\_ForIncorrectUrlFormat** - Test sprawdza, czy pole `Url` poprawnie waliduje format linku URL.