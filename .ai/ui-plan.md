# Architektura UI dla CareerPilotAi

## 1. Przegląd struktury UI

Architektura interfejsu użytkownika (UI) dla aplikacji CareerPilotAi zostanie zaimplementowana w oparciu o framework ASP.NET Core MVC z widokami Razor. Wykorzystany zostanie Bootstrap 5 w podejściu mobile-first, aby zapewnić pełną responsywność na różnych urządzeniach. Kluczowym założeniem jest stworzenie płynnego i intuicyjnego doświadczenia użytkownika (UX) poprzez minimalizację przeładowań całej strony na rzecz dynamicznych aktualizacji z wykorzystaniem JavaScript (AJAX).

Struktura opiera się na kilku głównych widokach: Dashboard, Job Board (lista aplikacji), szczegóły aplikacji oraz formularze do tworzenia i edycji. Centralnym elementem jest stały, górny pasek nawigacyjny, zapewniający łatwy dostęp do kluczowych sekcji. Architektura kładzie nacisk na spójność wizualną, jasną komunikację z użytkownikiem (za pomocą powiadomień "toast" i walidacji w czasie rzeczywistym) oraz bezpieczeństwo poprzez walidację po stronie serwera i klienta oraz ochronę przed atakami CSRF.

## 2. Lista widoków

### Widoki autoryzacji

**1. Rejestracja**
- **Nazwa widoku**: `Register.cshtml`
- **Ścieżka widoku**: `/Auth/Register`
- **Główny cel**: Umożliwienie nowym użytkownikom założenia konta w systemie (US-001).
- **Kluczowe informacje do wyświetlenia**: Formularz rejestracyjny.
- **Kluczowe komponenty widoku**:
  - Pola formularza: Email, Hasło, Potwierdź hasło.
  - Przycisk "Zarejestruj się".
  - Link do strony logowania.
- **UX, dostępność i względy bezpieczeństwa**: Walidacja po stronie klienta (on-blur) i serwera. Wymagania dotyczące siły hasła. Użycie atrybutu `[EnhancedEmail]`.
- **Powiązany viewModel**: `RegisterViewModel`

**2. Logowanie**
- **Nazwa widoku**: `Login.cshtml`
- **Ścieżka widoku**: `/Auth/Login`
- **Główny cel**: Umożliwienie zarejestrowanym użytkownikom zalogowania się do aplikacji (US-002).
- **Kluczowe informacje do wyświetlenia**: Formularz logowania.
- **Kluczowe komponenty widoku**:
  - Pola formularza: Email, Hasło.
  - Checkbox "Zapamiętaj mnie".
  - Przycisk "Zaloguj się".
  - Link do strony rejestracji i resetowania hasła.
- **UX, dostępność i względy bezpieczeństwa**: Komunikat o błędzie przy niepoprawnych danych. Użycie atrybutu `[EnhancedEmail]`.
- **Powiązany viewModel**: `LoginViewModel`

**3. Resetowanie hasła (Żądanie)**
- **Nazwa widoku**: `ForgotPassword.cshtml`
- **Ścieżka widoku**: `/Auth/ForgotPassword`
- **Główny cel**: Umożliwienie użytkownikowi zainicjowania procesu resetowania hasła (US-004).
- **Kluczowe informacje do wyświetlenia**: Formularz do podania adresu email.
- **Kluczowe komponenty widoku**:
  - Pole formularza: Email.
  - Przycisk "Wyślij link do resetu hasła".
- **UX, dostępność i względy bezpieczeństwa**: Informacja zwrotna o wysłaniu maila (bez potwierdzania istnienia adresu w bazie).
- **Powiązany viewModel**: `ForgotPasswordViewModel`

**4. Resetowanie hasła (Formularz)**
- **Nazwa widoku**: `ResetPassword.cshtml`
- **Ścieżka widoku**: `/Auth/ResetPassword`
- **Główny cel**: Umożliwienie użytkownikowi ustawienia nowego hasła po kliknięciu w link z maila (US-004).
- **Kluczowe informacje do wyświetlenia**: Formularz do ustawienia nowego hasła.
- **Kluczowe komponenty widoku**:
  - Pola formularza: Hasło, Potwierdź hasło.
  - Ukryte pole z tokenem resetującym.
  - Przycisk "Ustaw nowe hasło".
- **UX, dostępność i względy bezpieczeństwa**: Walidacja siły hasła. Token jednorazowego użytku o ograniczonym czasie ważności.
- **Powiązany viewModel**: `ResetPasswordViewModel`

### Główne widoki aplikacji

**5. Dashboard**
- **Nazwa widoku**: `Index.cshtml` (w folderze `Home`)
- **Ścieżka widoku**: `/Home/Index`
- **Główny cel**: Prezentacja kluczowych metryk i podsumowania aktywności użytkownika (US-021, US-022, US-023, US-024).
- **Kluczowe informacje do wyświetlenia**: Statystyki, wykresy, ostatnie aplikacje.
- **Kluczowe komponenty widoku**:
  - Karty z kluczowymi metrykami (KPI).
  - Wykres kołowy statusów aplikacji.
  - Wykres słupkowy liczby aplikacji w czasie (z przełącznikiem 30/60/90 dni, aktualizowany przez AJAX).
  - Tabela z ostatnio dodanymi aplikacjami.
  - Stan pusty z przyciskiem "Call to Action" zachęcającym do dodania pierwszej aplikacji.
- **UX, dostępność i względy bezpieczeństwa**: Interaktywne wykresy. Responsywny layout (grid). Dane ładowane po stronie serwera.
- **Powiązany viewModel**: `DashboardViewModel`

**6. Lista aplikacji (Job Board)**
- **Nazwa widoku**: `Index.cshtml` (w folderze `JobApplication`)
- **Ścieżka widoku**: `/JobApplication/Index`
- **Główny cel**: Wyświetlanie, filtrowanie i zarządzanie wszystkimi aplikacjami o pracę (US-008, US-013 do US-020).
- **Kluczowe informacje do wyświetlenia**: Lista aplikacji w formie kart, panel filtrów.
- **Kluczowe komponenty widoku**:
  - Panel boczny z filtrami (status, wynagrodzenie, lokalizacja, etc.).
  - Pole wyszukiwania w pasku nawigacyjnym lub nad listą.
  - Lista aplikacji w formie kart w stylu "JustJoinIt".
  - Na każdej karcie: szybka zmiana statusu (dropdown), menu akcji (Edytuj, Usuń).
  - Stan pusty z przyciskiem "Call to Action" do dodania aplikacji.
- **UX, dostępność i względy bezpieczeństwa**: Filtry aplikowane po kliknięciu przycisku. Zmiana statusu przez AJAX z powiadomieniem toast. Pasek filtrów zwijany na mobile.
- **Powiązany viewModel**: `JobBoardViewModel`

**7. Dodawanie/Edycja aplikacji**
- **Nazwa widoku**: `Create.cshtml`, `Edit.cshtml`
- **Ścieżka widoku**: `/JobApplication/Create`, `/JobApplication/Edit/{id}`
- **Główny cel**: Umożliwienie użytkownikowi dodania nowej lub modyfikacji istniejącej aplikacji (US-005, US-006, US-010).
- **Kluczowe informacje do wyświetlenia**: Formularz z danymi aplikacji. W widoku `Create` dodatkowo pole do wklejenia ogłoszenia.
- **Kluczowe komponenty widoku**:
  - `Textarea` do wklejenia ogłoszenia i przycisk "Przetwórz z AI" (`Create`).
  - Wskaźnik ładowania (spinner) podczas przetwarzania AI.
  - Formularz z polami: Nazwa firmy, Stanowisko, Opis, etc.
  - Dynamiczny interfejs do dodawania/usuwania umiejętności (do 20).
  - Pola do wprowadzania widełek płacowych.
  - Przyciski "Zapisz" i "Anuluj".
- **UX, dostępność i względy bezpieczeństwa**: Wypełnianie formularza przez AI (AJAX). Walidacja on-blur. Ochrona CSRF.
- **Powiązany viewModel**: `JobApplicationViewModel`

**8. Szczegóły aplikacji**
- **Nazwa widoku**: `Details.cshtml`
- **Ścieżka widoku**: `/JobApplication/Details/{id}`
- **Główny cel**: Wyświetlenie wszystkich szczegółów konkretnej aplikacji (US-009).
- **Kluczowe informacje do wyświetlenia**: Pełne dane aplikacji.
- **Kluczowe komponenty widoku**:
  - Nagłówek z nazwą firmy i stanowiskiem.
  - Pełny opis stanowiska (z zachowaniem formatowania).
  - Pełna lista umiejętności z poziomami.
  - Wszystkie pozostałe dane (lokalizacja, wynagrodzenie etc.).
  - Przyciski akcji: "Edytuj", "Usuń" (z modalem potwierdzającym).
  - Dropdown do szybkiej zmiany statusu (AJAX).
- **UX, dostępność i względy bezpieczeństwa**: Czytelny, dwukolumnowy layout na desktopie. Modal potwierdzający usunięcie.
- **Powiązany viewModel**: `JobApplicationViewModel`

## 3. Lista ViewModels

**Modele związane z autoryzacją:**
- `RegisterViewModel`: `string Email`, `string Password`, `string ConfirmPassword`.
- `LoginViewModel`: `string Email`, `string Password`, `bool RememberMe`.
- `ForgotPasswordViewModel`: `string Email`.
- `ResetPasswordViewModel`: `string Email`, `string Password`, `string ConfirmPassword`, `string Token`.

**Modele związane z aplikacją o pracę:**
- `JobApplicationViewModel`: `Guid? Id`, `string CompanyName`, `string PositionTitle`, `string JobDescription`, `string Location`, `WorkMode WorkMode`, `ContractType ContractType`, `ExperienceLevel ExperienceLevel`, `string OfferUrl`, `ApplicationStatus Status`, `SalaryViewModel Salary`, `List<SkillViewModel> Skills`.
- `SalaryViewModel`: `decimal? MinAmount`, `decimal? MaxAmount`, `Currency Currency`, `Rate Rate`, `bool IsGross`.
- `SkillViewModel`: `string Name`, `SkillLevel Level`.

**Modele związane z widokami list i dashboardu:**
- `JobBoardViewModel`: `List<JobApplicationCardViewModel> JobApplications`, `JobBoardFiltersViewModel Filters`.
- `JobApplicationCardViewModel`: `Guid Id`, `string CompanyName`, `string PositionTitle`, `string Location`, `WorkMode WorkMode`, `ApplicationStatus Status`, `List<string> TopSkills`, `string SalaryDisplay`, `DateTime CreatedDate`.
- `JobBoardFiltersViewModel`: `string SearchTerm`, `List<ApplicationStatus> SelectedStatuses`, `decimal? MinSalary`, `decimal? MaxSalary`, `Rate? SalaryRate`, `string Location`, `List<WorkMode> SelectedWorkModes`, `List<ExperienceLevel> SelectedExperienceLevels`.
- `DashboardViewModel`: `DashboardMetricsViewModel KeyMetrics`, `StatusChartViewModel StatusDistribution`, `ApplicationsChartViewModel ApplicationsOverTime`, `List<RecentActivityViewModel> RecentActivity`.

*Uwaga: Wszystkie właściwości w ViewModelach będą posiadały odpowiednie atrybuty DataAnnotations (`[Required]`, `[StringLength]`, `[Url]`, `[EnhancedEmail]` itp.) w celu zapewnienia walidacji po stronie serwera.*

## 4. Układ i struktura nawigacji

Nawigacja w aplikacji będzie scentralizowana w stałym, górnym pasku nawigacyjnym (`navbar`), widocznym we wszystkich widokach po zalogowaniu.

**Struktura paska nawigacyjnego:**
1.  **Logo aplikacji**: Umieszczone po lewej stronie, linkuje do Dashboardu (`/Home/Index`).
2.  **Główne linki nawigacyjne**:
    - `Dashboard` (`/Home/Index`)
    - `Job Board` (`/JobApplication/Index`)
    - `Dodaj aplikację` (`/JobApplication/Create`) - przycisk "Call to Action".
3.  **Menu użytkownika**:
    - Ikona profilu z napisem "My Profile" po prawej stronie.
    - Rozwijane menu (dropdown) z opcjami:
        - `Ustawienia` (poza MVP)
        - `Wyloguj` (kończy sesję i przekierowuje do `/Auth/Login`).

**Przepływ użytkownika (User Flow):**
- **Nowy użytkownik**: `Strona logowania` -> `Strona rejestracji` -> (Po rejestracji) -> `Dashboard`.
- **Powracający użytkownik**: `Strona logowania` -> (Po logowaniu) -> `Dashboard`.
- **Zarządzanie aplikacjami**:
    - Z `Dashboard` lub `Job Board` użytkownik może przejść do `Dodaj aplikację`.
    - Z `Job Board` użytkownik klika na kartę, aby przejść do `Szczegółów aplikacji`.
    - Ze `Szczegółów aplikacji` lub z `Job Board` użytkownik może przejść do `Edycji aplikacji` lub zainicjować `Usunięcie aplikacji`.

Układ stron (z wyjątkiem stron autoryzacji) będzie oparty na głównym pliku `_Layout.cshtml`, który będzie zawierał `navbar`, główny kontener na treść oraz stopkę.

## 5. Kluczowe komponenty

Poniższe komponenty będą reużywalne w różnych częściach aplikacji w celu zapewnienia spójności i efektywności rozwoju.

1.  **Karta aplikacji (`JobApplicationCard`)**:
    - Używana w widoku `Job Board`.
    - Wyświetla skondensowane informacje o aplikacji.
    - Zawiera komponenty do szybkiej zmiany statusu i menu akcji (edycja/usuwanie).
2.  **Panel filtrów (`FiltersSidebar`)**:
    - Używany w widoku `Job Board`.
    - Zawiera wszystkie kontrolki do filtrowania listy aplikacji.
    - Responsywny (zwija się do menu "hamburger" na urządzeniach mobilnych).
3.  **Modal potwierdzenia (`ConfirmationModal`)**:
    - Używany przy akcjach nieodwracalnych, np. usuwaniu aplikacji.
    - Zawiera ostrzeżenie i przyciski do potwierdzenia lub anulowania akcji.
4.  **Powiadomienia "Toast" (`ToastNotification`)**:
    - Używane globalnie do informowania o wyniku operacji asynchronicznych (np. zmiana statusu, zapis formularza).
    - Wyświetlają komunikaty o sukcesie, błędzie lub informacyjne bez przerywania pracy użytkownika.
5.  **Dynamiczny formularz umiejętności (`SkillsInput`)**:
    - Używany w widokach `Create` i `Edit` aplikacji.
    - Umożliwia dodawanie, edycję i usuwanie umiejętności wraz z ich poziomem, z dynamicznym licznikiem.
6.  **Komponent stanu pustego (`EmptyState`)**:
    - Używany na `Dashboard` i `Job Board`, gdy brakuje danych.
    - Wyświetla informację i przycisk "Call to Action", prowadząc użytkownika do kolejnego kroku.
