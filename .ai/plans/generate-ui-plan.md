Jesteś wykwalifikowanym architektem frontend, specjalizującym się w frameworku ASP .NET Core MVC którego zadaniem jest stworzenie kompleksowej architektury interfejsu użytkownika w oparciu o dokument wymagań produktu (PRD) i notatki z sesji planowania. Twoim celem jest zaprojektowanie struktury interfejsu użytkownika, która skutecznie spełnia wymagania produktu, jest zgodna z możliwościami API i zawiera spostrzeżenia z sesji planowania.

Najpierw dokładnie przejrzyj następujące dokumenty:

Dokument wymagań produktu (PRD):
<prd>
@prd
</prd>

Session Notes:
<session_notes>
<conversation_summary>
<decisions>
1.  **Przepływ parsowania AI**: Wprowadzanie danych i formularz aplikacji będą znajdować się na jednej stronie. Formularz będzie wypełniany dynamicznie przez AJAX po stronie klienta, a w trakcie przetwarzania będzie wyświetlany wskaźnik ładowania (spinner).
2.  **Zarządzanie umiejętnościami**: Zostanie zaimplementowany dynamiczny interfejs do dodawania/usuwania umiejętności (do 20) z polem tekstowym, listą wyboru poziomu i licznikiem.
3.  **Widełki płacowe**: Informacja o kwocie brutto/netto będzie przechwytywana i przechowywana. `SalaryViewModel` będzie zawierał właściwość logiczną `IsGross`.
4.  **Filtrowanie na liście aplikacji**: Filtry będą stosowane wyłącznie po kliknięciu przycisku "Zastosuj filtry". Zrezygnowano z filtrowania w czasie rzeczywistym.
5.  **Wykres na Dashboardzie**: Zakres czasowy wykresu (30/60/90 dni) będzie zmieniany za pomocą grupy przycisków, które będą odświeżać dane przez AJAX.
6.  **Dostępność**: Implementacja standardów dostępności (np. WCAG) została odłożona na okres po MVP.
7.  **Obsługa błędów**: Zostanie wdrożony globalny mechanizm obsługi błędów, wykorzystujący powiadomienia "toast" dla operacji AJAX i dedykowaną stronę błędu dla błędów serwera.
8.  **Walidacja emaila**: We wszystkich ViewModelach zawierających pole email (rejestracja, logowanie, reset hasła) będzie używany niestandardowy atrybut walidacji `[EnhancedEmailAttribute]`.
9.  **Strony autoryzacji**: Widoki logowania i rejestracji będą osobnymi stronami.
10. **Nawigacja i layout**: Aplikacja będzie korzystać z górnego, stałego paska nawigacyjnego (`navbar`) oraz responsywnego, dwukolumnowego layoutu na stronach ze szczegółami.
11. **Szybka zmiana statusu**: Zmiana statusu z poziomu listy i szczegółów aplikacji będzie realizowana za pomocą asynchronicznego zapytania AJAX, z dynamiczną aktualizacją UI i powiadomieniem "toast".
12. **Stany "puste"**: Widoki "Job Board" i "Dashboard" będą miały specjalne komunikaty i przyciski "Call to Action", gdy użytkownik nie posiada jeszcze żadnych danych.
13. **Powiadomienia "toast"**: Zostanie wdrożony spójny system powiadomień "toast" dla operacji CRUD (sukces, błąd, informacja).
</decisions>
<matched_recommendations>
1.  **Architektura dodawania aplikacji**: Użycie jednego widoku z `textarea` i formularzem, który jest dynamicznie wypełniany przez AJAX, co zapewnia płynne doświadczenie użytkownika.
2.  **Interfejs umiejętności**: Zastosowanie dynamicznego formularza z tagami do wyświetlania dodanych umiejętności i licznikiem jest intuicyjnym rozwiązaniem.
3.  **Struktura `ViewModeli`**: Zdefiniowano szczegółowe `ViewModele` (`JobApplicationViewModel`, `SalaryViewModel`, `SkillViewModel` itp.) z odpowiednimi adnotacjami danych, co zapewni solidną walidację po stronie serwera.
4.  **Layout strony szczegółów**: Dwukolumnowy układ na desktopie (treść główna + panel z kluczowymi informacjami) i jednokolumnowy na mobile, co optymalizuje czytelność.
5.  **Nawigacja**: Implementacja standardowego, górnego paska nawigacyjnego (`navbar`) zapewni intuicyjną i znaną użytkownikom nawigację po aplikacji.
6.  **Obsługa akcji asynchronicznych**: Wykorzystanie zapytań AJAX do zmiany statusu, filtrowania i aktualizacji wykresów, w połączeniu z powiadomieniami "toast", zminimalizuje przeładowania strony i poprawi responsywność interfejsu.
7.  **Zarządzanie stanami "pustymi"**: Implementacja dedykowanych komunikatów dla pustych list i dashboardów poprawi pierwsze wrażenie i poprowadzi użytkownika przez aplikację.
</matched_recommendations>
<ui_architecture_planning_summary>
### Główne wymagania dotyczące architektury UI
Architektura UI dla MVP będzie oparta na frameworku ASP.NET Core MVC z Razor Views oraz Bootstrap 5. Interfejs ma być responsywny (mobile-first), z interaktywnymi elementami zasilanymi przez JavaScript/AJAX w celu minimalizacji przeładowań strony. Kluczowe założenia to spójność, intuicyjność oraz centralizacja operacji CRUD w dedykowanych widokach. Zostanie zaimplementowany globalny system obsługi błędów oraz spójny system powiadomień "toast".

### Kluczowe widoki, ekrany i przepływy użytkownika
- **Autoryzacja**:
    - `/Auth/Register`: Osobna strona rejestracji.
    - `/Auth/Login`: Osobna strona logowania. Po pomyślnym logowaniu użytkownik jest przekierowywany na Dashboard.
- **Dodawanie Aplikacji (`/JobApplication/Create`)**:
    - Użytkownik wkleja treść ogłoszenia do `textarea`.
    - Po kliknięciu "Przetwórz z AI", dane są wysyłane przez AJAX, a formularz poniżej jest dynamicznie wypełniany. Wyświetlany jest spinner.
    - Użytkownik weryfikuje/uzupełnia dane i zapisuje aplikację. Po zapisie jest przekierowywany do widoku szczegółów nowej aplikacji.
- **Lista Aplikacji (Job Board - `/JobApplication/Index`)**:
    - Główny widok listy aplikacji w formie kart.
    - Posiada panel filtrów, które są aplikowane po kliknięciu przycisku.
    - Umożliwia szybką zmianę statusu, edycję i usuwanie aplikacji.
    - Kliknięcie na kartę prowadzi do widoku szczegółów.
- **Szczegóły Aplikacji (`/JobApplication/Details/{id}`)**:
    - Prezentuje wszystkie dane dotyczące aplikacji w czytelnym, dwukolumnowym układzie.
    - Umożliwia dostęp do akcji: Edycja, Usuń, Zmiana Statusu.
- **Edycja Aplikacji (`/JobApplication/Edit/{id}`)**:
    - Widok formularza wstępnie wypełnionego danymi aplikacji, analogiczny do widoku dodawania (bez części AI).
- **Dashboard (`/Home/Index`)**:
    - Strona główna po zalogowaniu. Prezentuje kluczowe metryki (KPI), wykres kołowy statusów oraz wykres słupkowy aplikacji w czasie (z możliwością zmiany zakresu dat przez AJAX).

### ViewModels powiązane z widokami
- **`RegisterViewModel`**: `Email` [EnhancedEmail], `Password`, `ConfirmPassword`.
- **`LoginViewModel`**: `Email` [EnhancedEmail], `Password`, `RememberMe`.
- **`JobApplicationViewModel`**: `Id`, `CompanyName`, `PositionTitle`, `JobDescription`, `Location`, `WorkMode`, `ContractType`, `ExperienceLevel`, `OfferUrl`, `Salary` (typ: `SalaryViewModel`), `Skills` (typ: `List<SkillViewModel>`).
- **`SalaryViewModel`**: `MinAmount`, `MaxAmount`, `Currency`, `Rate`, `IsGross`.
- **`SkillViewModel`**: `Name`, `Level`.
- **`JobBoardViewModel`**: `List<JobApplicationCardViewModel> JobApplications`, `JobBoardFiltersViewModel Filters`.
- **`JobApplicationCardViewModel`**: `Id`, `CompanyName`, `PositionTitle`, `Location`, `WorkMode`, `Status`, `TopSkills`, `SalaryDisplay`, `CreatedDate`.
- **`JobBoardFiltersViewModel`**: `SelectedStatuses`, `MinSalary`, `MaxSalary`, `Location`, `SelectedWorkModes`, `SelectedExperienceLevels`.

### Kwestie dotyczące responsywności i bezpieczeństwa
- **Responsywność**: Aplikacja będzie w pełni responsywna dzięki Bootstrap 5. Główne elementy, takie jak `navbar`, siatka (grid) i panele filtrów, będą się adaptować do różnych rozmiarów ekranu.
- **Bezpieczeństwo**: Uwierzytelnianie będzie zarządzane przez ASP.NET Identity. Wszystkie endpointy modyfikujące dane będą zabezpieczone przed CSRF. Walidacja danych wejściowych będzie przeprowadzana zarówno po stronie klienta, jak i serwera (przy użyciu Data Annotations w `ViewModelach`), aby zapobiec atakom.
</ui_architecture_planning_summary>
<unresolved_issues>
Brak nierozwiązanych kwestii. Wszystkie poruszone tematy zostały omówione i uzyskano na nie odpowiedzi, co pozwala na przejście do następnego etapu projektowania i implementacji.
</unresolved_issues>
</conversation_summary>
</session_notes>

Twoim zadaniem jest stworzenie szczegółowej architektury interfejsu użytkownika, która obejmuje niezbędne widoki, mapowanie podróży użytkownika, strukturę nawigacji, powiązane ViewModels wraz z atrybutami DataAnnotations i kluczowe elementy dla każdego widoku. Projekt powinien uwzględniać doświadczenie użytkownika i bezpieczeństwo.

Wykonaj następujące kroki, aby ukończyć zadanie:

1. Dokładnie przeanalizuj PRD i notatki z sesji.
2. Wyodrębnij i wypisz kluczowe wymagania z PRD.
3. Zidentyfikuj i wymień główne ViewModels
4. Utworzenie listy wszystkich niezbędnych widoków na podstawie PRD i notatek z sesji.
5. Określenie głównego celu i kluczowych informacji dla każdego widoku.
7. Zaprojektuj strukturę nawigacji.
8. Zaproponuj kluczowe elementy interfejsu użytkownika dla każdego widoku, biorąc pod uwagę UX, dostępność i bezpieczeństwo.
9. Rozważ potencjalne przypadki brzegowe lub stany błędów.
11. Przejrzenie i zmapowanie wszystkich historyjek użytkownika z PRD do architektury interfejsu użytkownika.
12. Wyraźne mapowanie wymagań na elementy interfejsu użytkownika.
13. Rozważ potencjalne punkty bólu użytkownika i sposób, w jaki interfejs użytkownika je rozwiązuje.

Dla każdego głównego kroku pracuj wewnątrz tagów <ui_architecture_planning> w bloku myślenia, aby rozbić proces myślowy przed przejściem do następnego kroku. Ta sekcja może być dość długa. To w porządku, że ta sekcja może być dość długa.

Przedstaw ostateczną architekturę interfejsu użytkownika w następującym formacie Markdown:

```markdown
# Architektura UI dla [Nazwa produktu]

## 1. Przegląd struktury UI

[Przedstaw ogólny przegląd struktury UI]

## 2. Lista widoków

[Dla każdego widoku podaj:
- Nazwa widoku
- Ścieżka widoku
- Główny cel
- Kluczowe informacje do wyświetlenia
- Kluczowe komponenty widoku
- UX, dostępność i względy bezpieczeństwa]
- Powiązany viewModel

## 3. Lista ViewModels

[Opisz ViewModels, ich właściwości oraz atrybuty walidacji]

## 4. Układ i struktura nawigacji

[Wyjaśnij, w jaki sposób użytkownicy będą poruszać się między widokami]

## 5. Kluczowe komponenty

[Wymień i krótko opisz kluczowe komponenty, które będą używane w wielu widokach].
```

Skup się wyłącznie na architekturze interfejsu użytkownika, podróży użytkownika, nawigacji i kluczowych elementach dla każdego widoku. Nie uwzględniaj szczegółów implementacji, konkretnego projektu wizualnego ani przykładów kodu, chyba że są one kluczowe dla zrozumienia architektury.

Końcowy rezultat powinien składać się wyłącznie z architektury UI w formacie Markdown w języku polskim, którą zapiszesz w pliku .ai/ui-plan.md. Nie powielaj ani nie powtarzaj żadnej pracy wykonanej w bloku myślenia.