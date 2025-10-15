# Architektura UI dla CareerPilotAi

## 1. Przegląd struktury UI

Aplikacja CareerPilotAi wykorzystuje ASP.NET Core MVC (Razor Views) z Bootstrap 5, JavaScript (AJAX) i Chart.js. Interfejs jest responsywny (mobile‑first), z górnym paskiem nawigacyjnym i spójnymi wzorcami interakcji dla operacji CRUD. Dane są ładowane i aktualizowane asynchronicznie wszędzie tam, gdzie to poprawia doświadczenie (statusy, karty, wykresy, parsing AI). Formularze używają walidacji po stronie klienta (unobtrusive validation) i serwera (DataAnnotations). Wszystkie operacje modyfikujące wymagają autoryzacji i ochrony CSRF.

## 2. Lista widoków

### Widok: Dashboard
- Ścieżka widoku: GET `/Home/Index`
- Główny cel: Prezentacja kluczowych metryk i trendów, „start here” po zalogowaniu.
- Kluczowe informacje do wyświetlenia:
  - Karty KPI: Total Applications, Submitted, Interviews, Offers.
  - Wykres kołowy rozkładu statusów.
  - Wykres słupkowy liczby aplikacji w czasie z przełącznikiem 30/60/90 dni.
  - Ostatnia aktywność (ostatnie 5 aplikacji) – po MVP opcjonalnie.
- Kluczowe komponenty widoku:
  - `KpiCards` (kafelki), `StatusDonutChart`, `ApplicationsBarChart`, `RangeToggle (30/60/90)`.
  - „Pusty stan” z CTA do dodania pierwszej aplikacji.
- UX, dostępność i względy bezpieczeństwa:
  - Jasna hierarchia informacji; przy braku danych pokazuj pusty stan.
  - Przełącznik zakresu czasu odświeża dane przez AJAX.
  - Autoryzacja wymagana; brak danych nie ujawnia metadanych o innych użytkownikach.
- Powiązany viewModel:
  - Proponowany: `DashboardViewModel { Total, Submitted, Interviews, Offers, StatusDistribution[], Trend[] }` (dane ładowane AJAX-em).
  - API pomocnicze: `GET /job-applications/api/cards` (wykorzystywane do KPI i rozkładu statusów). Dodatkowy endpoint trendu (proponowany): `GET /dashboard/api/trend?range=30|60|90`.

### Widok: Job Board (lista aplikacji)
- Ścieżka widoku: GET `/job-applications` (Controller: `JobApplicationController.Index`)
- Główny cel: Przegląd wszystkich aplikacji w formie kart z szybkim dostępem do akcji.
- Kluczowe informacje do wyświetlenia:
  - Karty: nazwa firmy, stanowisko, status, data utworzenia; (po MVP: widełki płacowe, lokalizacja, top umiejętności).
  - Panel filtrów: statusy, wynagrodzenie (po MVP), lokalizacja, tryb pracy, poziom doświadczenia.
  - Sortowanie: data dodania (najnowsze/najstarsze).
- Kluczowe komponenty widoku:
  - `FiltersPanel` (aplikacja filtrów po kliknięciu „Zastosuj filtry”).
  - `JobApplicationCard` lista (z Quick Actions: View, Edit, Delete, StatusDropdown).
  - `StatusBadge`, `KebabMenu`, `ConfirmationModal` (usuwanie), `Toast`.
  - „Pusty stan” z CTA do dodania pierwszej aplikacji.
- UX, dostępność i względy bezpieczeństwa:
  - Filtry nie stosują się w czasie rzeczywistym – tylko po „Zastosuj filtry”.
  - Zmiana statusu i usuwanie odbywają się AJAX-em, z natychmiastową aktualizacją UI i toastami.
  - Linki klikalne na całej karcie prowadzą do szczegółów.
  - Autoryzacja wymagana.
- Powiązany viewModel:
  - Widok: lekki (dane ładowane AJAX). Proponowany: `JobBoardViewModel { Filters: JobBoardFiltersViewModel }`.
  - AJAX: `JobApplicationCardsViewModel { Cards[], TotalApplications, *StatusQuantities }` z `GET /job-applications/api/cards` (docelowo akceptuje parametry filtrów i sortowania: `statuses[]`, `minSalary`, `maxSalary`, `location`, `workModes[]`, `experienceLevels[]`, `sort`).

### Widok: Dodawanie aplikacji (AI + ręcznie)
- Ścieżka widoku: 
  - GET `/job-applications/entry-job-details`
  - POST `/job-applications/entry-job-details`
  - AJAX: POST `/job-applications/api/enhance-job-description` (parsowanie/ulepszanie opisu)
  - AJAX: POST `/job-applications/api/update-job-description` (aktualizacja opisu po akceptacji)
- Główny cel: Dodanie nowej aplikacji przez wklejenie ogłoszenia i ręczne doprecyzowanie.
- Kluczowe informacje do wyświetlenia:
  - Textarea (do 5000 słów) na treść ogłoszenia.
  - Formularz aplikacji (wypełniany dynamicznie przez AJAX) – tytuł, firma, opis; (po MVP: umiejętności, widełki, lokalizacja, tryb pracy, typ umowy, poziom doświadczenia, URL oferty, status/domyślnie Draft).
- Kluczowe komponenty widoku:
  - `Textarea + ParseWithAI Button + Spinner`.
  - Formularz z walidacją on blur.
  - `SkillsInput` (dynamiczne dodawanie/usuwanie, poziom, licznik do 20) – po MVP.
  - `SalaryInput` (kwota min/max + waluta + rate + `IsGross`) – po MVP.
- UX, dostępność i względy bezpieczeństwa:
  - Wyraźny stan ładowania podczas parsowania (spinner, disable controls).
  - Komunikaty o pełnym/ częściowym sukcesie i błędzie z toastami.
  - Walidacja po stronie serwera i klienta; ochrona CSRF (formularz i nagłówki AJAX z tokenem).
- Powiązany viewModel:
  - `JobOfferEntryDetailsViewModel { JobDescription [Required, MaxWords(5000)], Title [2–100], Company [2–200], URL [Url?], JobApplicationId? }`.
  - `EnhanceJobDescriptionViewModel { JobDescriptionText }` (AJAX).
  - Po MVP (proponowane): `SkillViewModel`, `SalaryViewModel`, `JobApplicationViewModel` (kompletny formularz po parsingu).

### Widok: Szczegóły aplikacji
- Ścieżka widoku: GET `/job-applications/details/{jobApplicationId:guid}`
- Główny cel: Prezentacja pełnych informacji o aplikacji i szybkie akcje.
- Kluczowe informacje do wyświetlenia:
  - Nagłówek: firma + stanowisko.
  - Pełny opis stanowiska (zachowane formatowanie).
  - Status (badge + dropdown do zmiany).
  - (Po MVP: umiejętności z poziomami, widełki, lokalizacja, tryb pracy, typ umowy, link do oferty, daty utw./aktualizacji).
- Kluczowe komponenty widoku:
  - `StatusBadge`, `StatusDropdown (AJAX PATCH)`, `Toast`.
  - Akcje: `Edit`, `Delete` (z `ConfirmationModal`).
- UX, dostępność i względy bezpieczeństwa:
  - Zmiana statusu i usuwanie przez AJAX; natychmiastowa aktualizacja UI.
  - Autoryzacja wymagana.
- Powiązany viewModel:
  - `JobApplicationDetailsViewModel { JobApplicationId?, CompanyName [Required, MaxLength(200)], JobTitle [Required, MaxLength(200)], JobDescription [MaxWords(5000)], Status [Required] }`.
  - AJAX: `UpdateJobApplicationStatusViewModel { Status [Required, AllowedValues(Statusy)] }` → `PATCH /job-applications/api/status/{id}`.

### Widok: Edycja aplikacji (po MVP lub zgodnie z PRD)
- Ścieżka widoku: GET `/job-applications/edit/{jobApplicationId:guid}`, POST `/job-applications/edit/{jobApplicationId:guid}`
- Główny cel: Modyfikacja danych istniejącej aplikacji analogiczna do dodawania, bez części AI.
- Kluczowe informacje do wyświetlenia:
  - Formularz pre‑wypełniony wszystkimi polami.
- Kluczowe komponenty widoku:
  - Formularz z walidacją on blur; `SkillsInput`, `SalaryInput`, `Toast`.
- UX, dostępność i względy bezpieczeństwa:
  - Walidacja serwer/klient, CSRF.
- Powiązany viewModel:
  - Proponowany: `JobApplicationEditViewModel` (jak `JobApplicationViewModel`).

### Widoki autoryzacji
- Rejestracja: GET/POST `/auth/register` → `RegisterViewModel`.
- Rejestracja – potwierdzenie: GET `/auth/register-confirmation`.
- Potwierdzenie emaila: GET `/auth/confirm-email` (przekierowania wg wyniku).
- Logowanie: GET/POST `/auth/login` → `LoginViewModel` (po logowaniu redirect do Dashboard lub Job Board).
- Wylogowanie: POST `/auth/logout` + GET `/auth/logout` (potwierdzenie).
- Reset hasła: `ForgotPassword` (GET/POST), `ForgotPasswordConfirmation` (GET), `ResetPassword` (GET/POST), `ResetPasswordConfirmation` (GET).
- Access denied: GET `/auth/access-denied`.
- UX/Bezpieczeństwo:
  - `[EnhancedEmail]` we wszystkich polach email.
  - Komunikaty błędów nienaruszające prywatności (brak enumeracji kont) – już zaimplementowane w kontrolerze.
  - Po rejestracji automatyczne logowanie po potwierdzeniu email i redirect na Dashboard.

### Widok: Ustawienia użytkownika
- Ścieżka widoku: GET/POST `/auth/user-settings`
- Główny cel: Zmiana strefy czasowej i ustawień profilu.
- Kluczowe informacje do wyświetlenia:
  - Email (readonly), selektor strefy czasowej.
- Kluczowe komponenty widoku:
  - `Select(TimeZone)`, `Toast`/komunikat sukcesu.
- UX, dostępność i względy bezpieczeństwa:
  - CSRF, walidacja serwer/klient.
- Powiązany viewModel:
  - `UserSettingsViewModel { Email, TimeZoneId [Required], TimeZones (IEnumerable<SelectListItem>) }`.

### Widoki systemowe
- Błąd: GET `/Home/Error` (globalny mechanizm + strona błędu).
- Prywatność: GET `/Home/Privacy`.

## 3. Lista ViewModels

Istniejące (w kodzie):
- `RegisterViewModel`
  - Email: `[Required][EnhancedEmail][MaxLength(254)]`
  - Password: `[Required][StringLength(100, MinimumLength=8)][DataType(Password)]`
  - ConfirmPassword: `[DataType(Password)][Compare("Password")]`
- `LoginViewModel`
  - Email: `[Required][EnhancedEmail][MaxLength(254)]`
  - Password: `[Required][DataType(Password)]`
  - RememberMe: `bool`
  - ReturnUrl: `string?`, Message: `string?`
- `ForgotPasswordViewModel`
  - Email: `[Required][EnhancedEmail][MaxLength(254)]`
- `ResendConfirmationViewModel`
  - Email: `[Required][EnhancedEmail][MaxLength(254)]`
- `ResetPasswordViewModel`
  - UserId: `[Required]`
  - Password: `[Required][StringLength(100, MinimumLength=8)][DataType(Password)]`
  - ConfirmPassword: `[DataType(Password)][Compare("Password")]`
  - Code: `[Required]`
- `UserSettingsViewModel`
  - Email: `Display("Email")`
  - TimeZoneId: `[Required][Display("Time Zone")]`
  - TimeZones: `IEnumerable<SelectListItem>`
- `JobOfferEntryDetailsViewModel`
  - JobDescription: `[Required][MaxWords(5000)]`
  - Title: `[Required][MaxLength(100)][MinLength(1)]`
  - Company: `[Required][MaxLength(200)]`
  - URL: `[Url][MaxLength(2000)]?`
  - JobApplicationId: `Guid?`
- `EnhanceJobDescriptionViewModel`
  - JobDescriptionText: `string`
- `UpdateJobDescriptionViewModel`
  - JobApplicationId: `[Required] Guid`
  - JobDescription: `[Required][MaxWords(5000)]`
- `JobApplicationDetailsViewModel`
  - JobApplicationId: `Guid?`
  - CompanyName: `[Required][MaxLength(200)]`
  - JobTitle: `[Required][MaxLength(200)]`
  - JobDescription: `[MaxWords(5000)]`
  - Status: `[Required]`
- `JobApplicationCardsViewModel`
  - Cards: `List<JobApplicationCardViewModel>`
  - TotalApplications + liczniki statusów: `int`
- `JobApplicationCardViewModel`
  - JobApplicationId: `Guid`
  - Title, Company: `string`
  - CreatedAt: `DateTime`
  - Status: `string`
- `UpdateJobApplicationStatusViewModel`
  - Status: `[Required]` (proponowane: `[AllowedValues(ApplicationStatus.ValidStatuses...)]`)

Proponowane (uzupełniające PRD/plan):
- `JobBoardFiltersViewModel`
  - SelectedStatuses: `List<string>` (opcje z `ApplicationStatus.ValidStatuses`)
  - MinSalary, MaxSalary: `decimal?`
  - Currency: `string?` (np. „PLN”)
  - Rate: `string?` (monthly/daily/hourly/yearly)
  - IsGross: `bool?`
  - Location: `string?`
  - SelectedWorkModes: `List<string>` (Remote/Hybrid/On‑site)
  - SelectedExperienceLevels: `List<string>` (Junior/Mid/Senior/Not specified)
- `JobBoardViewModel`
  - JobApplications: `List<JobApplicationCardViewModel>` (opcjonalnie – zwykle ładowane AJAX)
  - Filters: `JobBoardFiltersViewModel`
- `SalaryViewModel`
  - MinAmount, MaxAmount: `[Range(0, double.MaxValue)] decimal?`
  - Currency: `[Required] string`
  - Rate: `[Required, AllowedValues("monthly","daily","hourly","yearly")] string`
  - IsGross: `bool`
- `SkillViewModel`
  - Name: `[Required][MaxLength(100)]`
  - Level: `[Required, AllowedValues("Nice to have","Regular","Advanced","Master")]`
- `JobApplicationViewModel` (kompletny formularz):
  - CompanyName `[Required, 2–100]`
  - PositionTitle `[Required, 2–100]`
  - JobDescription `[Required, 50–5000 słów]`
  - Skills: `List<SkillViewModel>` (≤20)
  - ExperienceLevel: `AllowedValues(Junior/Mid/Senior/Not specified)`
  - Location `[Required, 2–100]`
  - WorkMode `[Required, AllowedValues(Remote/Hybrid/On-site)]`
  - ContractType `[Required, AllowedValues(B2B/FTE/Zlecenie/Inne)]`
  - Salary: `SalaryViewModel?`
  - OfferUrl: `[Url]?`
  - Status: `string` (domyślnie Draft)
- `DashboardViewModel`
  - Total, Submitted, Interviews, Offers: `int`
  - StatusDistribution: `IEnumerable<{Status:string, Count:int}>`
  - Trend: `IEnumerable<{Date:DateOnly, Count:int}>`

## 4. Układ i struktura nawigacji

- Top `navbar` (zawsze widoczny):
  - Logo (link do Dashboard lub Job Board, wg decyzji produktu).
  - Linki: Dashboard (`/Home/Index`), Job Board (`/job-applications`), Add Application (`/job-applications/entry-job-details`).
  - Prawy segment: `My Profile` (dropdown: Settings, Logout) oraz status zalogowania.
- Breadcrumbs na podstronach szczegółów/edycji.
- Mobile: hamburger menu, panel filtrów jako off‑canvas/drawer.
- Nawigacja po akcjach:
  - Karta → kliknięcie otwiera `/job-applications/details/{id}`.
  - Status Dropdown (AJAX) i Delete (AJAX) bez opuszczania listy.
- Stany puste kierują użytkownika do kluczowych akcji (CTA „Add Application”).

## 5. Kluczowe komponenty

- `Navbar`: globalna nawigacja z dostępem do głównych sekcji i profilu.
- `ToastNotifications`: spójne powiadomienia dla CRUD i AJAX (sukces/błąd/info).
- `ConfirmationModal`: potwierdzenie operacji nieodwracalnych (Delete), tekst wg PRD.
- `JobApplicationCard`: karta aplikacji z kluczami (firma, stanowisko, status, data) + quick actions.
- `StatusBadge` + `StatusDropdown (AJAX)`: kolorowy badge + dropdown do szybkiej zmiany statusu.
- `FiltersPanel`: panel filtrów z przyciskami „Apply filters” i „Clear all”.
- `SkillsInput`: dodawanie/usuwanie tagów umiejętności, wybór poziomu, licznik X/20.
- `SalaryInput`: kwoty min/max, waluta, rate, `IsGross`.
- `LoadingSpinner/Overlay`: stany ładowania (parsowanie AI, zmiany AJAX).
- `EmptyState`: dedykowane komunikaty i CTA dla Dashboard i Job Board.
- `Charts (Chart.js)`: donut dla statusów, bar dla trendów; przełącznik zakresu czasu.
- `ValidationSummary`/`FieldValidation`: komponenty walidacji klienckiej.

## 6. Przypadki brzegowe i obsługa błędów

- Parsowanie AI: 
  - Pełny sukces – auto‑wypełnienie.
  - Częściowy sukces – lista braków + oznaczenie pól.
  - Błąd – czerwony alert i wskazówki, przycisk „Retry”.
- Puste listy/dane: 
  - Dashboard/Job Board – komunikaty „empty state” + CTA.
- Walidacja: 
  - Przekroczenie limitu 5000 słów, niepoprawny URL, nieprawidłowy status.
- Błędy serwera/AJAX: 
  - Globalny handler + strona błędu, toasty z przyjaznymi komunikatami.
- Uprawnienia i ID: 
  - Brak dostępu/nieprawidłowe ID → przekierowanie, bez ujawniania szczegółów.
- Usuwanie: 
  - Modal z ostrzeżeniem o nieodwracalności; optymistyczna aktualizacja listy.

## 7. Mapowanie historyjek użytkownika (PRD → UI)

- US‑001 Rejestracja: `/auth/register` (formularz + walidacja, email unikalny, auto‑login po potwierdzeniu).
- US‑002 Logowanie: `/auth/login` (Remember me, redirect do Dashboard, błędy widoczne).
- US‑003 Wylogowanie: POST `/auth/logout`, redirect do `/auth/logout`.
- US‑004 Reset hasła: `/auth/forgot-password` → email; `/auth/reset-password`.
- US‑005 Dodanie z AI: `/job-applications/entry-job-details` + AJAX `/api/enhance-job-description` + spinner.
- US‑006 Ręczne dodanie: ten sam widok; walidacja on blur, redirect do szczegółów.
- US‑007 Umiejętności: `SkillsInput` (po MVP), licznik X/20, poziomy, AI sugestie (po MVP).
- US‑008 Lista aplikacji: `/job-applications`, karty, responsywny layout, klik → szczegóły.
- US‑009 Szczegóły: `/job-applications/details/{id}` pełny opis, lista umiejętności (po MVP), akcje.
- US‑010 Edycja: `/job-applications/edit/{id}` (po MVP lub zgodnie z PRD), walidacja, toasty.
- US‑011 Usunięcie: z listy/szczegółów, modal + toast + redirect/listy aktualizacja.
- US‑012 Szybka zmiana statusu: dropdown na karcie i w szczegółach (AJAX patch) + historia (po MVP).
- US‑013–019 Filtrowanie/Wyszukiwarka: `FiltersPanel` + Apply/Clear, top‑bar search (po MVP: wyszukiwanie 300ms debounce; obecnie – po kliknięciu Apply).
- US‑020 Sortowanie: dropdown (najnowsze/najstarsze), natychmiastowa zmiana kolejności (klient/serwer).
- US‑021 KPI: Karty na Dashboard.
- US‑022 Statusy: Donut chart.
- US‑023 Trend: Bar chart + przełącznik zakresu.
- US‑024 Ostatnia aktywność: tabela 5 pozycji (po MVP).
- US‑025 Błąd AI: czerwony alert + Retry.
- US‑026 Częściowy sukces AI: lista braków + oznaczenie pól.
- US‑027 Walidacja formularzy: komunikaty pod polami, czerwony border, on blur.
- US‑028–029 Responsywność: layouty mobile/tablet/desktop Bootstrap 5.
- US‑030 Nawigacja: stały navbar, breadcrumbs, linki.
- US‑031 Profil: dropdown „My Profile” (Settings, Logout).

## 8. Mapowanie wymagań → elementy UI

- „Jedna strona dodawania” z AI: textarea + AJAX + spinner + dynamiczny formularz.
- „Szybka zmiana statusu”: StatusDropdown (AJAX) w kartach i szczegółach + Toast.
- „Filtry tylko po kliknięciu”: `FiltersPanel` z przyciskiem „Apply filters”; brak live filtering.
- „Wykres 30/60/90”: `RangeToggle` + AJAX reload danych dla wykresów.
- „Puste stany”: dedykowane komunikaty + CTA w Dashboard i Job Board.
- „Globalne toasty”: sukces/błąd/info dla wszystkich operacji AJAX/CRUD.
- „Walidacja emaila”: `[EnhancedEmail]` w VM autoryzacji.
- „Bezpieczeństwo”: CSRF (formularze i AJAX), autoryzacja endpointów, defensywne komunikaty.

## 9. Potencjalne punkty bólu i rozwiązania UX

- Czasochłonne wprowadzanie danych → Parsowanie AI + pre‑wypełnianie, walidacja on blur, klarowne braki.
- Rozproszenie informacji → Job Board z kartami + filtry + szybkie akcje.
- Śledzenie postępów → Dashboard z KPI, donutem statusów, trendem.
- Brak wizualizacji → Chart.js z prostymi, kontrastowymi kolorami i legendą.
- Błędy i niepewność → Spójne toasty i globalna strona błędu; jasne komunikaty.
- Mobile użyteczność → Drawer filtrów, karty w kolumnie, większe targety dotykowe.


