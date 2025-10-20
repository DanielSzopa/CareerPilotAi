### Job Board (Przeglądanie listy aplikacji) — Plan wdrożenia

#### 1) Opis wdrażanej funkcjonalności

Funkcjonalność „Job Board” dostarcza kompletny widok listy aplikacji użytkownika z możliwością filtrowania, wyszukiwania i sortowania. Widok będzie responsywny (mobile-first), z sidebar’em filtrów (desktop) oraz off‑canvas drawer’em (mobile). Lista będzie prezentowana w formie kart (horizontal/vertical responsive cards) z quick actions: podgląd szczegółów, szybka zmiana statusu (AJAX), usunięcie (modal/confirm). Stan filtrów oraz sortowania będzie utrzymywany w URL (query string) dla łatwego udostępniania linków i obsługi nawigacji wstecz.

Wczytywanie danych listy będzie realizowane server‑side (SSR) w akcji `GET /job-applications` na podstawie parametrów w URL; po zastosowaniu filtrów/sortowania/wyszukiwania następuje pełne przeładowanie strony. Szybka zmiana statusu pozostaje w AJAX (dla lepszego UX) i korzysta z istniejącego endpointu `PATCH /job-applications/api/status/{jobApplicationId}`. Usuwanie odbywa się poprzez klasyczny POST (bez AJAX) z pełnym przeładowaniem strony po sukcesie z zachowaniem parametrów filtrów, sortowania i wyszukiwania.


#### 2) Opis UI

- Top bar (desktop i mobile):
  - Search input z debounce 300 ms (wyszukiwanie po `Company` i `Title`).
  - Sort dropdown: „Date added (newest first)” (default), „Date added (oldest first)”.
  - Results counter: „Showing X applications”.
  - Na mobile: przycisk otwierający off‑canvas filters.

- Sidebar filters (desktop) / Off‑canvas drawer (mobile):
  - Status (multi-select checkboxes): Draft, Submitted, Interview Scheduled, Waiting for offer, Received offer, Rejected, No contact.
  - Salary range: Min/Max (PLN) + dropdown Period (Monthly/Daily/Hourly/Yearly; default Monthly) + Type (Gross/Net; opcjonalny, egzekwowany jeśli salary wypełnione).
  - Location: text (partial, case-insensitive).
  - Work Mode (multi-select checkboxes): Remote, Hybrid, On-site.
  - Experience Level (multi-select checkboxes): Junior, Mid, Senior, Not specified.
  - Action buttons: „Apply Filters” (submit -> URL query) i „Clear All Filters” (reset do `/job-applications`).

- Status Summary Bar (opcjonalny pasek nad listą):
  - Kafle z licznikami: All, Draft, Submitted, Interview Scheduled, Waiting for offer, Received offer, Rejected, No contact.
  - Kolorowe kropki zgodnie z paletą statusów (Bootstrap: secondary/primary/warning/info/success/danger/dark).
  - Wyświetla wartości przeliczone dla całego zbioru użytkownika lub (konfigurowalnie) dla aktualnych wyników filtrowania; w MVP pokazujemy liczniki globalne + licznik „Showing X applications” dla wyników.

- Application Card (pojedyncza karta):
  - Header: Status badge (kolor) jako wskaźnik w prawym górnym rogu.
  - Company (bold) + Position (Title).
  - Top 3 umiejętności (badges) + „Show more” (opcjonalnie w przyszłości).
  - Location + Work Mode + Contract Type.
  - Salary (jeśli dostępne): „Min–Max PLN, [Gross/Net], [Period]”.
  - Created date (mniejszy, szary tekst) — sformatowany po stronie klienta.
  - Quick actions: „Details”, „Status” (dropdown, AJAX), „Delete” (modal/confirm → POST bez AJAX), „Edit” (opcjonalnie przez kebab menu).

- Empty states i loading:
  - Full loading spinner podczas pierwszego ładowania.
  - „No applications yet” (CTA do Create) gdy użytkownik nie ma żadnych aplikacji.
  - „No applications found matching your criteria” + „Clear Filters” gdy filtr/tekst nic nie zwróci.

- Accessibility i UX:
  - Semantic HTML, aria-labels (zwłaszcza dla dropdownów statusów i ikonowych przycisków).
  - Widoczny focus state, klawiaturowa nawigacja, responsywny układ, off‑canvas z focus trap.


#### 3) Opis filtrów (UI + kontrakt + backend)

Parametry (query string → `[FromQuery] JobApplicationFiltersViewModel`):

- searchTerm: string (max 100)
  - UI: input w top bar; debounce 300 ms; submit → aktualizacja URL i reload danych.
  - Backend: `Title` OR `Company` `ILIKE` (case-insensitive) zawiera `searchTerm`.

- statuses: string[] (wiele wystąpień `statuses=Draft&statuses=Submitted`), wartości: dokładne display names statusów (np. „Draft”, „Submitted”, „Interview Scheduled”, …).
  - UI: checkboxes (multi); zaznaczone serializowane do URL.
  - Backend: `WHERE Status IN (…selected…)`.

- minSalary: decimal?, maxSalary: decimal?
  - UI: dwa pola number + walidacje (0–999999) + placeholder.
  - Backend: jeśli oba ustawione → `SalaryMin >= minSalary` i/lub `SalaryMax <= maxSalary`; jeżeli ustawione jedno z nich → odpowiednio półprzedział.

- salaryPeriod: string? (Monthly/Daily/Hourly/Yearly); default Monthly gdy salary wypełnione bez period.
  - UI: select.
  - Backend: `WHERE SalaryPeriod == salaryPeriod` (jeśli min/max podane; w przeciwnym razie opcjonalne, nie filtruje).

- salaryType: string? (Gross/Net); opcjonalny, ale wymagany jeśli wypełniono salary (walidacja modelu po stronie serwera już istnieje dla Create; dla filtrów pozostaje opcjonalny).
  - UI: select (opcjonalny).
  - Backend: `WHERE SalaryType == salaryType` (jeśli podany).

- location: string? (max 100)
  - UI: text input.
  - Backend: `Location ILIKE %location%`.

- workModes: string[] (Remote/Hybrid/On-site)
  - UI: checkboxy (multi-select).
  - Backend: `WHERE WorkMode IN (...)`.

- experienceLevels: string[] (Junior/Mid/Senior/Not specified)
  - UI: checkboxy (multi-select).
  - Backend: `WHERE ExperienceLevel IN (...)`.

- sortBy: string? (DateAddedDesc [default] | DateAddedAsc)
  - UI: dropdown w top bar.
  - Backend: `ORDER BY CreatedAt DESC|ASC`.

Domyślne zachowanie bez parametrów: brak filtrów, sortowanie: newest first, pełna lista użytkownika.


#### 4) Opis sortowania

- Pola sortowania w MVP:
  - Date added (newest first) → `CreatedAt DESC` (default).
  - Date added (oldest first) → `CreatedAt ASC`.

- UI:
  - Dropdown w top bar z dwiema opcjami; wybrana wartość aktualizuje `sortBy` w URL, powoduje odświeżenie danych.
  - Widoczna informacja: „Showing X applications” pod top bar.


#### 5) Przepływ danych

Scenariusz ładowania strony:
1. Użytkownik otwiera `/job-applications?…query…`.
2. Backend SSR:
   - Pobiera `userId` z serwisu użytkownika,
   - Buduje zapytanie EF Core z filtrami (AsNoTracking) i sortowaniem na podstawie parametrów z URL,
   - Projektuje dane do `JobApplicationCardViewModel` i oblicza liczniki oraz `ResultCount`,
   - Zwraca widok z kompletnym modelem (karty renderowane w Razor).
3. Frontend wyświetla karty i liczniki bez dodatkowych wywołań AJAX.

Zmiana filtrów/sortowania/wyszukiwania:
1. Użytkownik zmienia wartości i klika „Apply Filters” (lub submituje formularz search/sort).
2. Formularz wysyła żądanie GET do `/job-applications` z parametrami w query → pełne przeładowanie strony i render SSR nowej listy.

Quick change status:
1. Na karcie użytkownik wybiera nowy status w dropdown.
2. JS wywołuje `PATCH /job-applications/api/status/{id}` z `{ status: "…" }`.
3. Po sukcesie: toast „Status updated to [New Status]”, lokalnie uaktualnienie badge/tekstu i, jeśli filtr wyklucza nowy status, karta jest ukrywana; następnie (opcjonalnie) odświeżenie listy przez `loadJobApplications()`.

Delete:
1. Użytkownik klika „Delete” → modal/confirm.
2. Wysyłany jest klasyczny POST do endpointu usuwania (bez AJAX).
3. Po sukcesie: redirect 302 do `/job-applications` (z zachowaniem parametrów filtrów, jeśli to możliwe) i wyświetlenie komunikatu o sukcesie.

Bezpieczeństwo:
- Formularze GET/POST zawierają `@Html.AntiForgeryToken()` tam, gdzie wymagane (POST usuwania). Żądanie AJAX pozostaje jedynie dla quick status i dołącza nagłówek `RequestVerificationToken`.
- Dostęp zabezpieczony `[Authorize]` + filtrowanie po `userId` w zapytaniu.


#### 6) View Models (nowe/rozszerzone)

Nowy: `JobApplicationFiltersViewModel` (CareerPilotAi/ViewModels/JobApplication)
- SearchTerm: string? [MaxLength(100)]
- Statuses: List<string>? (wartości display names: "Draft", …)
- MinSalary: decimal?
- MaxSalary: decimal?
- SalaryType: string? (Gross|Net)
- SalaryPeriod: string? (Monthly|Daily|Hourly|Yearly)
- Location: string? [MaxLength(100)]
- WorkModes: List<string>? (Remote|Hybrid|On-site)
- ExperienceLevels: List<string>? (Junior|Mid|Senior|Not specified)
- SortBy: string? (DateAddedDesc|DateAddedAsc)

Rozszerzenie: `JobApplicationCardViewModel`
- JobApplicationId: Guid
- Title: string
- Company: string
- Location: string
- WorkMode: string
- ContractType: string
- SalaryMin: decimal?
- SalaryMax: decimal?
- SalaryType: string?
- SalaryPeriod: string?
- SkillsTop: List<string> (max 3; opcjonalne)
- SkillsCount: int
- CreatedAt: DateTime
- Status: string (display name)

Rozszerzenie: `JobApplicationCardsViewModel`
- Cards: List<JobApplicationCardViewModel>
- ResultCount: int (liczba rekordów po filtrach)
- TotalApplications: int (całkowita liczba aplikacji użytkownika)
- DraftStatusQuantity: int
- RejectedStatusQuantity: int
- SubmittedStatusQuantity: int
- InterviewScheduledStatusQuantity: int
- WaitingForOfferStatusQuantity: int
- ReceivedOfferStatusQuantity: int
- NoContactStatusQuantity: int

Uwaga: `JobApplicationDetailsViewModel` bez zmian w tym wdrożeniu; używany tylko na stronie szczegółów.


#### 7) Kroki implementacji

Backend
1. Utwórz `JobApplicationFiltersViewModel` w `CareerPilotAi/ViewModels/JobApplication/` z właściwościami jak wyżej (DataAnnotations dla walidacji długości/liczb; brak customowych atrybutów wymagających zmian). Zapewnij kompatybilność z `[FromQuery]` (listy poprzez powtarzające się parametry).
2. Zaktualizuj `JobApplicationController.Index()` do akceptowania  modelu filtrów (dla SSR inicjalizacji UI; załaduj dane do ViewModelu). Przekaż wartości do View.
3. Pozbądź się endpointu `GET /job-applications/api/cards`  i zaimplementuj logikę w `JobApplicationController.Index()`
   - Sortował wg `CreatedAt` na podstawie `filters.SortBy`,
   - Projektował do `JobApplicationCardViewModel` (dołącz SkillsTop: `j.Skills.OrderBy(…).Take(3).Select(s => s.Name)`, `SkillsCount = j.Skills.Count`),
   - Obliczał `ResultCount` (po filtrach) i liczniki statusów (na pełnym zbiorze użytkownika lub na przefiltrowanym — rekomendacja: pełny zbiór, aby zachować kontekst użytkownika),
   - Zwracał `JobApplicationCardsViewModel` (JSON).
4. Dodaj testy jednostkowe (warstwa application/integration):
   - Filtr statusów: zwraca tylko wybrane statusy.
   - Wyszukiwanie: case-insensitive po Title/Company.
   - Filtry salary (min/max, period, type): poprawne zawężanie.
   - Sortowanie: ASC/DESC po CreatedAt.
   - Filtry wielokrotnego wyboru (workModes/experienceLevels).

Frontend (Razor + JS)
5. Rozbuduj `Views/JobApplication/Index.cshtml`:
   - Dodaj top bar (search input, sort dropdown, results counter, mobile filter toggle),
   - Dodaj sidebar/off‑canvas filter form (Bootstrap 5) z polami jak w sekcji Filtry,
   - Ustaw formularz filtrów/sort/search na `method="get"` (przeglądarka zbuduje query string),
   - Renderuj listę kart w Razor z modelu (usuń obecny kod ładowania kart przez JS),
   - Formularz „Delete” jako POST (z `@Html.AntiForgeryToken()` i hidden `returnUrl` = `Request.QueryString.Value`).
6. Minimalny JS (opcjonalny):
   - `updateApplicationStatus(id, status)` (AJAX PATCH) dla quick status,
   - Auto-submit dla niektórych kontrolek (opcjonalnie), nadal kończy się GET (pełny reload),
   - Brak JS do pobierania listy (usuwamy `loadJobApplications` i render po stronie klienta).
7. Quick Status Dropdown na karcie:
   - Dropdown Bootstrap z zaznaczonym aktualnym statusem,
   - Po wyborze: wywołanie `updateApplicationStatus` (PATCH), toast „Status updated…”, jeżeli filtr wyklucza nowy status → ukryj kartę lub reload danych.
8. Delete (confirm modal lub `confirm()` w MVP):
   - Potwierdzenie → submit formularza POST z `returnUrl`,
   - Po sukcesie: redirect 302 do `returnUrl` (lub `/job-applications`) i toast „Application deleted”.

UX/Accessibility/Bezpieczeństwo
9. Dodaj aria-labels do przycisków ikonowych i dropdownów, zapewnij focus states; off‑canvas z focus trap.
10. Zapewnij przekazanie Anti-Forgery Token do wszystkich wywołań AJAX (nagłówek `RequestVerificationToken`).
11. Zadbaj o performance: projekcja SELECT tylko potrzebnych pól; `AsNoTracking()`; brak N+1 (wczytanie `Skills` tylko do policzenia i top 3 — projekcja bez pełnego `Include`, użyj podzapytań/Select).

Wykończenie i dokumentacja
12. Zaktualizuj dokumentację w `.ai/ui-plan.md` (sekcja Job Board – zbieżność UI i kontraktów) — informacyjnie.
13. (Opcjonalnie) Wprowadź prosty system toastów (Bootstrap Alerts/Toast) zamiast `alert()`.

Kryteria akceptacji (mapowanie na PRD)
- Lista wyświetla karty z Company, Position, wybranymi metadanymi, status badge, created date.
- Filtry status/salary/location/work mode/experience level działają i utrzymują się w URL.
- Wyszukiwanie po Company/Position (case-insensitive) z debounce 300 ms.
- Sortowanie: Date added newest/oldest.
- Quick status change (AJAX) + informacja o sukcesie.
- Delete z potwierdzeniem (AJAX) + redirect do listy lub reload wyników.
- Responsywność: sidebar → off‑canvas na mobile, karty w pojedynczej kolumnie.

