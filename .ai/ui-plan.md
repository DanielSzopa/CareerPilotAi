# Architektura UI dla CareerPilotAi

## 1. Przegląd struktury UI

### 1.1 Podejście architektoniczne
CareerPilotAi wykorzystuje architekturę opartą na serwerowo renderowanych widokach (ASP.NET Core Razor Views) z selektywnym użyciem JavaScript dla poprawy doświadczenia użytkownika. Kluczowe operacje (nawigacja, CRUD) powodują pełne przeładowanie strony, podczas gdy parsowanie AI i szybka zmiana statusu wykorzystują AJAX dla lepszej responsywności.

### 1.2 Główne założenia
- **Rendering**: Server-Side Rendering (SSR) z selektywnym wykorzystaniem AJAX
- **Framework CSS**: Bootstrap 5 dla responsywności i spójności wizualnej
- **Walidacja**: ASP.NET Core Unobtrusive Validation (client-side) + server-side
- **Zarządzanie stanem**: Po stronie serwera, stan filtrów w URL query parameters
- **Feedback użytkownika**: Toast notifications (prawym górnym rogu, 5s auto-hide)
- **Responsywność**: Mobile-first approach

### 1.3 Kluczowe wzorce UI
1. **Nawigacja**: Stały górny navbar z linkami do głównych sekcji
2. **Formularze**: Współdzielony partial view (_JobApplicationForm.cshtml) dla reużywalności
3. **Listy**: Karty aplikacji w stylu podłużnym (JustJoinIt-like)
4. **Filtrowanie**: Sidebar (desktop) / Off-canvas drawer (mobile)
5. **Akcje**: Quick actions (status AJAX, delete modal, edit link)
6. **Stany puste**: Dedykowane komunikaty dla braku danych

## 2. Lista widoków

### 2.1 Widoki autentykacji (Auth)

#### 2.1.1 Register - Rejestracja
- **Ścieżka**: `/Auth/Register`
- **Dostęp**: Anonymous
- **Główny cel**: Umożliwienie nowym użytkownikom założenia konta
- **Kluczowe informacje**:
  - Formularz rejestracji (email, hasło, potwierdzenie hasła)
  - Wymagania hasła (min. 8 znaków, duża litera, cyfra)
  - Komunikaty walidacji
- **Kluczowe komponenty**:
  - Formularz rejestracji z walidacją real-time (on blur)
  - Przycisk Submit "Create Account"
  - Link do logowania "Already have an account? Sign in"
  - Wymagania bezpieczeństwa hasła (lista z checkmarkami)
- **UX/Dostępność**:
  - Aria-labels dla pól formularza
  - Pokazywanie/ukrywanie hasła (toggle)
  - Wyraźne komunikaty błędów pod polami
  - Auto-focus na pierwszym polu
  - Disabled state przycisku podczas submitu
- **Bezpieczeństwo**:
  - Anti-forgery token
  - Walidacja siły hasła client-side i server-side
  - Email uniqueness check

#### 2.1.2 Login - Logowanie
- **Ścieżka**: `/Auth/Login`
- **Dostęp**: Anonymous
- **Główny cel**: Autentykacja użytkowników
- **Kluczowe informacje**:
  - Formularz logowania (email, hasło)
  - Checkbox "Remember me"
  - Link do resetu hasła
- **Kluczowe komponenty**:
  - Formularz logowania
  - Przycisk Submit "Sign In"
  - Link "Forgot password?"
  - Link do rejestracji "Don't have an account? Sign up"
  - Alert dla błędów autentykacji
- **UX/Dostępność**:
  - Friendly error messages (bez ujawniania szczegółów bezpieczeństwa)
  - Auto-focus na email input
  - Enter key submits form
  - Loading state podczas logowania
- **Bezpieczeństwo**:
  - Anti-forgery token
  - Rate limiting (backend)
  - Redirect po sukcesie do Dashboard lub returnUrl

#### 2.1.3 ForgotPassword - Reset hasła
- **Ścieżka**: `/Auth/ForgotPassword`
- **Dostęp**: Anonymous
- **Główny cel**: Inicjacja procesu resetu hasła
- **Kluczowe informacje**:
  - Formularz z polem email
  - Instrukcje procesu
- **Kluczowe komponenty**:
  - Email input field
  - Przycisk "Send Reset Link"
  - Success message po wysłaniu
  - Link powrotny do logowania
- **UX/Dostępność**:
  - Jasne instrukcje co się stanie
  - Success message nawet jeśli email nie istnieje (security)
  - Informacja o ważności linku (24h)

#### 2.1.4 ResetPassword - Ustawienie nowego hasła
- **Ścieżka**: `/Auth/ResetPassword?token={token}&email={email}`
- **Dostęp**: Anonymous (z ważnym tokenem)
- **Główny cel**: Ustawienie nowego hasła
- **Kluczowe informacje**:
  - Formularz z nowym hasłem i potwierdzeniem
  - Komunikat o wymaganiach hasła
- **Kluczowe komponenty**:
  - Password inputs (nowe hasło, potwierdzenie)
  - Przycisk "Reset Password"
  - Wymagania bezpieczeństwa hasła
  - Success/error alerts
- **UX/Dostępność**:
  - Walidacja zgodności haseł
  - Pokazywanie/ukrywanie hasła
  - Komunikat o wygaśnięciu tokenu
  - Auto-redirect do logowania po sukcesie

#### 2.1.5 ConfirmEmail - Potwierdzenie emaila
- **Ścieżka**: `/Auth/ConfirmEmail?userId={id}&token={token}`
- **Dostęp**: Anonymous (z ważnym tokenem)
- **Główny cel**: Potwierdzenie adresu email użytkownika
- **Kluczowe informacje**:
  - Status potwierdzenia (sukces/błąd)
  - Komunikat z następnymi krokami
- **Kluczowe komponenty**:
  - Success/error message
  - Przycisk "Go to Login" lub "Go to Dashboard"
- **UX/Dostępność**:
  - Jasna informacja o sukcesie/porażce
  - Auto-redirect po 5s (z możliwością skip)

### 2.2 Widoki główne aplikacji

#### 2.2.1 Dashboard - Panel główny
- **Ścieżka**: `/Home/Dashboard` (lub `/` dla zalogowanych)
- **Dostęp**: Authenticated
- **Główny cel**: Prezentacja kluczowych metryk i aktywności użytkownika
- **Kluczowe informacje**:
  - Metryki: Total Applications, Submitted, Interviews, Offers
  - Wykres kołowy rozkładu statusów
  - Wykres słupkowy aplikacji w czasie
  - Tabela 5 ostatnich aplikacji
- **Kluczowe komponenty**:
  - **Metric Cards** (grid 2x2 na desktop, kolumna na mobile):
    - Total Applications (niebieska ikona)
    - Submitted (zielona ikona)
    - Interviews (pomarańczowa ikona)
    - Offers (złota ikona)
  - **Status Distribution Chart** (wykres kołowy/donut):
    - Kolorowe segmenty dla każdego statusu
    - Legenda z nazwami i procentami
    - Interactive tooltips przy hover
  - **Timeline Chart** (wykres słupkowy):
    - Toggle buttons: 30/60/90 dni
    - Oś X: dni/tygodnie, Oś Y: liczba aplikacji
    - Interactive tooltips
  - **Recent Activity Table**:
    - Kolumny: Company, Position, Status, Date
    - Quick actions: View, Edit (ikony)
    - Status badge z kolorem
  - **Empty State** (dla nowych użytkowników):
    - Ilustracja
    - Tekst: "No applications yet. Start tracking your job search!"
    - CTA button: "Add Your First Application"
- **UX/Dostępność**:
  - Loading skeleton podczas ładowania danych
  - Aria-labels dla wykresów
  - Keyboard navigation dla toggles
  - Tooltips z dodatkowymi informacjami
  - Responsive grid layout
- **Interakcje**:
  - Click na Recent Application → redirect do Details
  - Toggle timeframe → reload z query param `?timeframe=60`
  - Click na Edit → redirect do Edit view

#### 2.2.2 Job Board - Lista aplikacji
- **Ścieżka**: `/JobApplication/Index`
- **Dostęp**: Authenticated
- **Główny cel**: Przeglądanie, filtrowanie i zarządzanie wszystkimi aplikacjami
- **Kluczowe informacje**:
  - Lista wszystkich aplikacji użytkownika w formie kart
  - Aktywne filtry i wyniki wyszukiwania
  - Licznik wyników
- **Layout**:
  - **Desktop**: Sidebar (filtry) + główna zawartość (karty)
  - **Mobile**: Off-canvas drawer (filtry) + główna zawartość
- **Kluczowe komponenty**:
  
  **A. Top Bar**:
  - Search input (placeholder: "Search by company or position...")
  - Sort dropdown (Date added: newest/oldest)
  - Results counter ("Showing X applications")
  - Filter toggle button (mobile only)
  
  **B. Sidebar / Off-canvas Filters**:
  - **Status Filter** (multi-select checkboxes):
    - Draft, Submitted, Interview Scheduled, Waiting for offer, Received offer, Rejected, No contact
  - **Salary Range Slider**:
    - Min/Max inputs
    - Dropdown: Monthly/Daily/Hourly/Yearly (default: Monthly)
  - **Location** (text input):
    - Partial match, case-insensitive
  - **Work Mode** (checkboxes):
    - Remote, Hybrid, On-site
  - **Experience Level** (checkboxes):
    - Junior, Mid, Senior, Not specified
  - **Action Buttons**:
    - "Apply Filters" (primary)
    - "Clear All Filters" (secondary, disabled gdy brak filtrów)
  
  **C. Application Cards** (główna zawartość):
  - Każda karta zawiera:
    - Company Name (bold, większy font)
    - Position (regular)
    - Top 3 skills (badges) + "Show more" link jeśli > 3
    - Location + Work Mode (ikona + tekst)
    - Contract Type (badge)
    - Salary range (jeśli dostępne)
    - Status (kolorowy badge w prawym górnym rogu)
    - Created date (mniejszy font, szary)
  - **Quick Actions** (pojawiają się przy hover lub zawsze na mobile):
    - Status dropdown (AJAX update)
    - Kebab menu (Edit, Delete)
  
  **D. Empty States**:
  - **No applications**: 
    - Ilustracja
    - "You haven't added any applications yet"
    - CTA: "Add Your First Application"
  - **No results**:
    - Ikona lupy
    - "No applications found matching your criteria"
    - "Try adjusting your filters or search term"
    - Button: "Clear Filters"

- **UX/Dostępność**:
  - Cards są klikalny (cała powierzchnia) → Details
  - Keyboard navigation dla kart (Tab, Enter)
  - Focus states dla wszystkich interaktywnych elementów
  - Aria-labels dla quick actions
  - Loading state dla kart podczas ładowania
  - Sticky sidebar na desktop
  - Smooth scroll do góry po zastosowaniu filtrów
  
- **Interakcje**:
  - **Search**: 300ms debounce → submit form → reload z query param
  - **Filter Apply**: Submit form → pełne przeładowanie strony z URL params
  - **Status Change**: AJAX POST do `/JobApplication/UpdateStatus/{id}` → update UI bez reload
  - **Delete**: Modal confirmation → POST do `/JobApplication/Delete/{id}` → redirect z toast
  - **Edit**: Link do `/JobApplication/Edit/{id}`
  - **Card Click**: Redirect do `/JobApplication/Details/{id}`

#### 2.2.3 Application Details - Szczegóły aplikacji
- **Ścieżka**: `/JobApplication/Details/{id}`
- **Dostęp**: Authenticated (owner only)
- **Główny cel**: Wyświetlenie wszystkich informacji o pojedynczej aplikacji
- **Kluczowe informacje**:
  - Wszystkie pola aplikacji
  - Formatowany opis stanowiska
  - Pełna lista umiejętności z poziomami
  - Daty utworzenia i aktualizacji
- **Layout**:
  - **Desktop**: Układ dwukolumnowy (info główne | dodatkowe info)
  - **Mobile**: Jednokolumnowy
- **Kluczowe komponenty**:
  
  **A. Header Section**:
  - Company Name (h1)
  - Position (h2)
  - Status badge (kolorowy, duży)
  - Action buttons bar:
    - "Edit" (primary button)
    - "Change Status" (dropdown)
    - "Delete" (danger button, outline)
  
  **B. Main Content (lewa kolumna na desktop)**:
  - **Job Description Section**:
    - Label: "Job Description"
    - Zawartość w `<pre>` tag dla zachowania formatowania
    - Scrollable jeśli bardzo długi
  - **Skills Section**:
    - Label: "Required Skills"
    - Lista wszystkich umiejętności jako badges z poziomami
    - Kolor badge zależny od poziomu:
      - Nice to have: szary
      - Regular: niebieski
      - Advanced: pomarańczowy
      - Master: złoty
  
  **C. Sidebar (prawa kolumna na desktop)**:
  - **Key Information Card**:
    - Experience Level (ikona + tekst)
    - Location (ikona + tekst)
    - Work Mode (ikona + tekst)
    - Contract Type (ikona + tekst)
  - **Salary Information Card** (jeśli dostępne):
    - Min - Max PLN
    - Gross/Net
    - Period (Monthly/Daily/Hourly/Yearly)
  - **Additional Information Card**:
    - Job URL (link, opens in new tab)
    - Created: [date]
    - Last updated: [date]
  
  **D. Empty States**:
  - Brak umiejętności: "No skills specified"
  - Brak salary: "Salary not specified"
  - Brak URL: Link nie wyświetlany

- **UX/Dostępność**:
  - Breadcrumb: Job Board > [Company Name]
  - Back button (opcjonalnie)
  - External link icon dla Job URL
  - Target="_blank" z rel="noopener noreferrer"
  - Responsive breakpoint dla zmiany layoutu
  - Copy to clipboard dla URL (opcjonalnie)
  
- **Interakcje**:
  - **Edit Click**: Redirect do `/JobApplication/Edit/{id}`
  - **Delete Click**: Otwiera modal potwierdzenia
  - **Status Change**: AJAX update + toast notification
  - **Delete Confirm**: POST → redirect do Job Board z toast "Application deleted"

#### 2.2.4 Create Application - Dodawanie aplikacji
- **Ścieżka**: `/JobApplication/Create`
- **Dostęp**: Authenticated
- **Główny cel**: Dodanie nowej aplikacji przez parsowanie AI lub ręcznie
- **Kluczowe informacje**:
  - Formularz tworzenia aplikacji
  - Opcja parsowania AI
  - Wszystkie wymagane pola

- **Layout**: Single column centered form (max-width: 800px)

- **Kluczowe komponenty**:
  
  **A. AI Parsing Section** (opcjonalna, na górze):
  - **Collapsed State** (domyślnie):
    - Button: "Parse Job Description with AI" (accordion toggle)
  - **Expanded State**:
    - Textarea (placeholder: "Paste job description here...")
    - Character/word counter (X / 5000 words)
    - Button: "Parse with AI" (primary, disabled jeśli puste)
    - Button: "Cancel" (secondary)
  - **Loading State** (podczas parsowania):
    - Disabled textarea
    - Loading spinner na przycisku
    - Tekst: "Analyzing job description..."
    - Blokada całego UI
  - **Success States**:
    - **Full Success** (zielony alert):
      - "Job description parsed successfully!"
      - Auto-collapse sekcji
      - Formularz wypełniony danymi
    - **Partial Success** (pomarańczowy alert):
      - "Job description partially parsed. Please fill in missing fields:"
      - Lista brakujących pól
      - Auto-collapse sekcji
      - Formularz częściowo wypełniony
      - Brakujące pola highlighted (żółte tło?)
  - **Error State** (czerwony alert):
    - "Unable to parse job description. Please ensure it contains: company name, position, and requirements."
    - Button: "Retry"
    - Button: "Fill Manually"
  
  **B. Application Form** (shared partial view):
  - Wykorzystuje `_JobApplicationForm.cshtml`
  - **Basic Information Section**:
    - Company Name* (input, 2-100 chars)
    - Position* (input, 2-100 chars)
    - Job Description* (textarea, 50-5000 words, counter)
  - **Skills Section**:
    - Label: "Skills (max 20)"
    - Skill input group:
      - Text input: "Add skill..."
      - Dropdown: Level (Nice to have/Regular/Advanced/Master)
      - Button: "Add" (plus icon)
    - Skills list (tags):
      - Każdy tag z nazwą, poziomem (badge) i X do usunięcia
      - Counter: "X / 20 skills"
  - **Job Details Section**:
    - Experience Level* (dropdown)
    - Location* (input, 2-100 chars)
    - Work Mode* (radio buttons lub dropdown)
    - Contract Type* (dropdown)
  - **Salary Section** (opcjonalna):
    - Checkbox: "Specify salary range"
    - (jeśli checked):
      - Min Salary (number input, 0-999999)
      - Max Salary (number input, 0-999999)
      - Salary Type (radio: Gross/Net)
      - Salary Period (dropdown: Monthly/Daily/Hourly/Yearly)
  - **Additional Information Section**:
    - Job URL (input, optional, URL validation)
    - Status (dropdown, default: Draft)
  
  **C. Form Actions**:
  - Button: "Create Application" (primary, submit)
  - Button: "Cancel" (secondary, redirect do Job Board)

- **UX/Dostępność**:
  - Validation on blur dla każdego pola
  - Inline error messages pod polami (czerwony tekst)
  - Red border dla invalid fields
  - Auto-save draft (poza MVP)
  - Confirmation modal jeśli user opuszcza stronę z niezapisanymi zmianami
  - Clear all form button (opcjonalnie)
  - Disabled submit button podczas walidacji lub submitu
  - Loading state na submit button
  
- **Walidacja**:
  - Client-side: Unobtrusive validation
  - Server-side: ModelState validation
  - Wyświetlanie wszystkich błędów naraz (validation summary na górze)
  - Scroll do pierwszego błędu

- **Interakcje**:
  - **Parse AI**: AJAX POST do `/JobApplication/ParseJobDescription` → populate form
  - **Add Skill**: JavaScript add tag do listy (max 20)
  - **Remove Skill**: JavaScript remove tag
  - **Submit**: POST do `/JobApplication/Create` → redirect do Details z toast

#### 2.2.5 Edit Application - Edycja aplikacji
- **Ścieżka**: `/JobApplication/Edit/{id}`
- **Dostęp**: Authenticated (owner only)
- **Główny cel**: Modyfikacja istniejącej aplikacji
- **Kluczowe informacje**:
  - Pre-wypełniony formularz aktualnymi danymi
  - Wszystkie pola edytowalne

- **Layout**: Identyczny jak Create (single column)

- **Kluczowe komponenty**:
  - **Brak sekcji AI Parsing** (tylko dla Create)
  - **Application Form** (shared partial view):
    - Wykorzystuje `_JobApplicationForm.cshtml`
    - Wszystkie pola pre-wypełnione
    - Hidden field: ApplicationId
  - **Form Actions**:
    - Button: "Save Changes" (primary, submit)
    - Button: "Cancel" (secondary, redirect do Details)

- **UX/Dostępność**:
  - Breadcrumb: Job Board > Details > Edit
  - Identyczna walidacja jak Create
  - Confirmation jeśli user opuszcza stronę z niezapisanymi zmianami
  - Indication of unsaved changes (dirty form)
  
- **Interakcje**:
  - **Submit**: POST do `/JobApplication/Edit/{id}` → redirect do Details z toast "Changes saved"
  - **Cancel**: Redirect do Details (z confirmation jeśli są zmiany)

### 2.3 Widoki pomocnicze

#### 2.3.1 Landing Page - Strona główna dla niezalogowanych
- **Ścieżka**: `/` lub `/Home/Index`
- **Dostęp**: Anonymous
- **Główny cel**: Prezentacja produktu i zachęta do rejestracji
- **Kluczowe komponenty**:
  - Hero section z value proposition
  - CTA buttons: "Get Started" (Register), "Sign In"
  - Features section (key benefits)
  - Simple how-it-works section
- **UX**: Clean, minimal design, focus na CTA

#### 2.3.2 Privacy Policy
- **Ścieżka**: `/Home/Privacy`
- **Dostęp**: Anonymous
- **Główny cel**: Wyświetlenie polityki prywatności
- **Kluczowe komponenty**:
  - Tekst polityki prywatności
  - Link w footer

#### 2.3.3 Error Page
- **Ścieżka**: `/Home/Error`
- **Dostęp**: Anonymous/Authenticated
- **Główny cel**: Obsługa nieoczekiwanych błędów
- **Kluczowe komponenty**:
  - Error icon/illustration
  - Error message: "Oops! Something went wrong"
  - Request ID (w dev mode)
  - Button: "Go to Dashboard" lub "Go to Home"
  - Support email/link

#### 2.3.4 Access Denied
- **Ścieżka**: `/Auth/AccessDenied`
- **Dostęp**: Authenticated
- **Główny cel**: Informacja o braku uprawnień
- **Kluczowe komponenty**:
  - 403 icon/illustration
  - Message: "You don't have permission to access this resource"
  - Button: "Go to Dashboard"

#### 2.3.5 Not Found (404)
- **Ścieżka**: `/Home/NotFound` lub catch-all route
- **Dostęp**: Anonymous/Authenticated
- **Główny cel**: Obsługa nieistniejących stron
- **Kluczowe komponenty**:
  - 404 icon/illustration
  - Message: "Page not found"
  - Search bar (opcjonalnie)
  - Button: "Go to Dashboard" lub "Go to Home"

### 2.4 Modalne i komponenty globalne

#### 2.4.1 Delete Confirmation Modal
- **Trigger**: Click na Delete button (Job Board, Details)
- **Zawartość**:
  - Header: "Delete Application?"
  - Body: 
    - "Are you sure you want to delete the application for [Position] at [Company]?"
    - "This action cannot be undone."
  - Actions:
    - "Cancel" (secondary)
    - "Delete" (danger, primary)
- **Interakcja**: Confirm → POST do `/JobApplication/Delete/{id}`

#### 2.4.2 Toast Notifications
- **Pozycja**: Prawy górny róg (fixed)
- **Auto-hide**: 5 sekund
- **Typy**:
  - Success (zielony): ✓ icon
  - Error (czerwony): ✗ icon
  - Warning (pomarańczowy): ⚠ icon
  - Info (niebieski): ℹ icon
- **Zawartość**: Icon + Message + Close button
- **Animacje**: Slide in from right, fade out
- **Przykładowe komunikaty**:
  - "Application created successfully"
  - "Changes saved"
  - "Application deleted"
  - "Status updated to [New Status]"
  - "Unable to save changes. Please try again."

#### 2.4.3 Loading Indicators
- **Full page loader**:
  - Spinner z overlay (semi-transparent background)
  - Używany podczas AI parsing
- **Button loader**:
  - Spinner w przycisku + disabled state
  - Używany podczas submit formularza
- **Skeleton screens**:
  - Dashboard cards podczas ładowania
  - Application cards podczas ładowania

## 3. Mapa podróży użytkownika

### 3.1 Główna podróż: Od rejestracji do pierwszej aplikacji

**Krok 1: Rejestracja**
```
Landing Page → Click "Get Started" → Register Form → Submit → Email Sent Confirmation → 
Check Email → Click Confirmation Link → Email Confirmed → Auto-login → Dashboard
```

**Krok 2: Dodanie pierwszej aplikacji**
```
Dashboard (empty state) → Click "Add Your First Application" → Create Application View →
Option A (AI): Paste job text → Click "Parse with AI" → Form auto-filled → Review/Edit → Submit
Option B (Manual): Fill form manually → Submit
→ Redirect to Application Details → Toast: "Application created successfully"
```

**Krok 3: Przeglądanie aplikacji**
```
Click "Job Board" (navbar) → Job Board View → See application card → 
Click card → Application Details View
```

**Krok 4: Zarządzanie aplikacją**
```
Application Details → 
Option A: Click "Edit" → Edit Form → Make changes → Save → Back to Details
Option B: Change Status (dropdown) → AJAX update → Toast notification
Option C: Click "Delete" → Confirmation Modal → Confirm → Redirect to Job Board → Toast
```

### 3.2 Typowa sesja użytkownika (returning user)

**Flow 1: Dodanie nowej aplikacji**
```
Login → Dashboard → Click "Add New Application" (navbar) → Create Application → 
Parse with AI or Manual → Submit → View Details → Go to Job Board (to see all)
```

**Flow 2: Przeglądanie i filtrowanie**
```
Login → Dashboard → Click "Job Board" (navbar) → 
Apply filters (status, salary, location) → Click "Apply Filters" → 
View filtered results → Click on application → View Details
```

**Flow 3: Aktualizacja statusu**
```
Login → Dashboard → See recent applications → Quick change status → Toast confirmation
OR
Job Board → Hover on card → Change status from dropdown → AJAX update → Toast
```

**Flow 4: Analiza postępów**
```
Login → Dashboard → Review metrics (Total, Submitted, Interviews, Offers) →
View status distribution chart → Toggle timeline (30/60/90 days) → 
Review recent activity → Click on application to see details
```

### 3.3 Podróż obsługi błędów

**Scenariusz 1: Błąd parsowania AI**
```
Create Application → Paste poor quality text → Parse with AI → Error alert →
Option A: Click "Retry" → Paste better text → Parse again
Option B: Click "Fill Manually" → Collapse AI section → Fill form manually
```

**Scenariusz 2: Błędy walidacji**
```
Create/Edit Application → Fill form with invalid data → Submit → 
Validation errors displayed → Scroll to first error → Fix errors → Submit again
```

**Scenariusz 3: Brak wyników wyszukiwania**
```
Job Board → Apply strict filters → No results → Empty state displayed →
Click "Clear Filters" → See all applications again
```

**Scenariusz 4: Próba dostępu do cudzej aplikacji**
```
Manual URL manipulation → Try to access /JobApplication/Details/{other-user-id} →
404 Not Found page → Click "Go to Dashboard"
```

### 3.4 Flow autentykacji

**Rejestracja i potwierdzenie email**
```
Landing → Register → Submit → Check Email Page →
(Email) Click confirmation link → Email Confirmed Page → Auto-login → Dashboard
```

**Logowanie**
```
Landing/Login → Enter credentials → Submit →
Success → Dashboard
Error → Login page with error message
```

**Reset hasła**
```
Login → Click "Forgot password?" → Enter email → Submit → Check Email Page →
(Email) Click reset link → Reset Password Form → Submit → Success → Login Page →
Login with new password → Dashboard
```

**Wylogowanie**
```
Any authenticated page → Click Profile Menu (navbar) → Click "Logout" →
Logout → Redirect to Landing/Login Page
```

## 4. Układ i struktura nawigacji

### 4.1 Navbar - Główna nawigacja (zawsze widoczna dla zalogowanych)

**Layout**: Fixed top, full width, Bootstrap navbar

**Struktura (Desktop)**:
```
+----------------------------------------------------------------+
| [Logo/Brand]  Dashboard  Job Board  [+ Add New]  [Profile ▼] |
+----------------------------------------------------------------+
```

**Elementy**:
- **Logo/Brand** (lewa strona):
  - "CareerPilotAi" text lub logo
  - Link do Dashboard (gdy zalogowany) lub Landing (gdy niezalogowany)
  
- **Navigation Links** (centrum/lewa):
  - "Dashboard" → `/Home/Dashboard`
  - "Job Board" → `/JobApplication/Index`
  
- **Primary CTA** (środek-prawa):
  - Button: "+ Add New Application" (primary button, prominent)
  - Link do `/JobApplication/Create`
  
- **Profile Menu** (prawa strona, dropdown):
  - Trigger: Avatar icon + username + dropdown arrow
  - Menu items:
    - "My Profile" (opcjonalnie, poza MVP)
    - "Settings" (opcjonalnie, poza MVP)
    - Divider
    - "Logout" → POST `/Auth/Logout`

**Mobile (< 768px)**:
```
+----------------------------------+
| [☰] CareerPilotAi    [Profile ▼] |
+----------------------------------+
```
- Hamburger menu toggle
- Collapsed menu items:
  - Dashboard
  - Job Board
  - Add New Application (jako zwykły link, nie button)
  - Divider
  - Logout

**States**:
- Active state: Current page highlighted (bolder, underline, lub background color)
- Hover state: Subtle background color change
- Focus state: Outline dla keyboard navigation

### 4.2 Navbar dla niezalogowanych użytkowników

**Desktop**:
```
+------------------------------------------------------+
| [Logo/Brand]                    [Sign In]  [Sign Up] |
+------------------------------------------------------+
```

**Elementy**:
- Logo → Landing Page
- "Sign In" (secondary button) → `/Auth/Login`
- "Sign Up" (primary button) → `/Auth/Register`

**Mobile**: Hamburger menu z Sign In i Sign Up jako linki

### 4.3 Breadcrumbs (opcjonalne, dla lepszej orientacji)

Wyświetlane na wybranych stronach pod navbarem:

- **Application Details**: 
  ```
  Home > Job Board > [Company Name]
  ```

- **Edit Application**:
  ```
  Home > Job Board > [Company Name] > Edit
  ```

- **Create Application**:
  ```
  Home > Add New Application
  ```

**UX**: Każdy element klikalny (poza ostatnim)

### 4.4 Footer (opcjonalny)

**Layout**: Full width, sticky bottom (jeśli content nie wypełnia viewport)

**Zawartość**:
- Copyright © 2025 CareerPilotAi
- Link: Privacy Policy
- Link: Terms of Service (opcjonalnie)
- Contact/Support email

### 4.5 Sidebar Navigation (tylko Job Board)

**Desktop**: Fixed sidebar (sticky podczas scroll)
- Zawiera filtry (opisane w Job Board view)
- Width: ~280-320px
- Collapsible (opcjonalnie)

**Mobile**: Off-canvas drawer
- Slide in from left
- Overlay background (semi-transparent)
- Close button (X) w prawym górnym rogu
- Same filters jak desktop

### 4.6 Nawigacja klawiaturą

**Wymagania dostępności**:
- Tab order: logiczny (top-to-bottom, left-to-right)
- Skip to main content link (ukryty, widoczny przy focus)
- Focus indicators: wyraźny outline (nie usuwać `outline: none`)
- Enter/Space: aktywacja przycisków i linków
- Escape: zamykanie modali i dropdownów
- Arrow keys: nawigacja w dropdownach i menu

## 5. Kluczowe komponenty

### 5.1 Komponenty formularzy

#### 5.1.1 _JobApplicationForm.cshtml (Partial View)
- **Cel**: Reużywalny formularz dla Create i Edit
- **Props/Parameters**:
  - Model: CreateJobApplicationViewModel lub EditJobApplicationViewModel
  - IsEditMode: bool (true dla Edit, false dla Create)
- **Zawartość**:
  - Wszystkie pola formularza (opisane w Create Application)
  - Validation summary
  - Validation messages dla każdego pola
- **Walidacja**: Unobtrusive validation scripts
- **Używany przez**: Create.cshtml, Edit.cshtml

#### 5.1.2 Input Components (standardowe Bootstrap)
- **Text Input**: z label, validation message, error state
- **Textarea**: z character/word counter
- **Dropdown/Select**: z placeholder option
- **Radio buttons**: z labels
- **Checkboxes**: z labels
- **Number input**: z min/max constraints

#### 5.1.3 Skills Manager Component
- **Cel**: Zarządzanie listą umiejętności (dodawanie, usuwanie)
- **UI**:
  - Input field + Level dropdown + Add button
  - Lista tagów z nazwą, poziomem (colored badge), Remove button (X)
  - Counter: "X / 20 skills"
- **JavaScript**:
  - Dodawanie: walidacja (unique, max 20), dodanie do listy, clear input
  - Usuwanie: usunięcie tagu, update countera
  - Hidden inputs dla submitu formularza (array)

### 5.2 Komponenty wyświetlania danych

#### 5.2.1 Application Card Component
- **Cel**: Kompaktowa prezentacja aplikacji na Job Board
- **Layout**: Podłużna karta (horizontal card)
- **Zawartość**:
  - Company (bold) + Position
  - Skills badges (top 3 + "Show more")
  - Location + Work Mode (icons)
  - Contract Type badge
  - Salary range (jeśli dostępne)
  - Status badge (kolorowy, top-right corner)
  - Created date (bottom, small, gray)
  - Quick actions (hover/always on mobile)
- **Responsive**: Stack vertically na very small screens
- **Accessibility**: Całość jako link (lub aria-label na całej karcie)

#### 5.2.2 Metric Card Component
- **Cel**: Wyświetlanie pojedynczej metryki na Dashboard
- **Layout**: Card z ikoną, wartością, etykietą
- **Zawartość**:
  - Icon (kolorowa, top-left)
  - Value (duża liczba, bold)
  - Label (mniejszy tekst, gray)
- **Kolory**:
  - Total: niebieski
  - Submitted: zielony
  - Interviews: pomarańżowy
  - Offers: złoty
- **Responsive**: 2 kolumny na mobile, 4 na desktop

#### 5.2.3 Status Badge Component
- **Cel**: Wizualne przedstawienie statusu aplikacji
- **UI**: Bootstrap badge z odpowiednim kolorem
- **Kolory** (zgodne z Bootstrap):
  - Draft: secondary (gray)
  - Submitted: primary (blue)
  - Interview Scheduled: warning (orange/yellow)
  - Waiting for Offer: info (light blue)
  - Received Offer: success (green)
  - Rejected: danger (red)
  - No Contact: dark (dark gray)
- **Tekst**: Display name statusu (z spacjami)

#### 5.2.4 Skill Badge Component
- **Cel**: Wyświetlanie umiejętności z poziomem
- **UI**: Badge z nazwą + mniejszy badge z poziomem (lub kolor tła)
- **Kolory poziomów**:
  - Nice to Have: secondary (gray)
  - Regular: primary (blue)
  - Advanced: warning (orange)
  - Master: custom golden/yellow
- **Tekst**: Skill name + level abbreviation (np. "Python (Adv)")

### 5.3 Komponenty nawigacyjne i interakcyjne

#### 5.3.1 Status Dropdown (Quick Action)
- **Cel**: Szybka zmiana statusu bez pełnej edycji
- **UI**: Bootstrap dropdown
- **Items**: Wszystkie statusy jako opcje
- **Current status**: Checked/highlighted
- **Interakcja**: 
  - Select → AJAX POST → Success: update badge, toast → Error: toast error
- **Loading state**: Disabled dropdown podczas AJAX

#### 5.3.2 Kebab Menu (Card Actions)
- **Cel**: Dodatkowe akcje na karcie aplikacji
- **UI**: Vertical three-dots icon → dropdown menu
- **Items**:
  - "Edit" → redirect do Edit
  - "Delete" → otwiera modal
- **Accessibility**: Aria-label "Actions for [Company] - [Position]"

#### 5.3.3 Pagination Component (poza MVP)
- Przygotowanie na przyszłość
- Bootstrap pagination
- Previous, numbered pages, Next
- Info: "Showing X-Y of Z applications"

### 5.4 Komponenty wykresów (Dashboard)

#### 5.4.1 Status Distribution Chart (Pie/Donut Chart)
- **Biblioteka**: Chart.js
- **Type**: Doughnut chart
- **Data**: Status name, count, percentage
- **Colors**: Zgodne z Status Badge colors
- **Legend**: Po prawej stronie (desktop) lub na dole (mobile)
- **Tooltips**: "{Status}: {Count} applications ({Percentage}%)"
- **Accessibility**: Canvas + hidden table z danymi dla screen readers

#### 5.4.2 Application Timeline Chart (Bar Chart)
- **Biblioteka**: Chart.js
- **Type**: Bar chart
- **Data**: Date (X) vs Count (Y)
- **Timeframe toggle**: Buttons (30/60/90 days) nad wykresem
- **Tooltips**: "{Date}: {Count} applications added"
- **Responsive**: Reduce bars na mobile (aggregate weeks instead of days)
- **Accessibility**: Canvas + hidden table

### 5.5 Komponenty feedbacku

#### 5.5.1 Alert Component
- **Typy**: Success, Error, Warning, Info
- **UI**: Bootstrap alert (dismissible)
- **Używane do**:
  - Validation summary (błędy formularza)
  - AI parsing results
  - General info messages
- **Pozycja**: Na górze contentu strony
- **Icons**: Odpowiedni icon dla każdego typu

#### 5.5.2 Loading Spinner
- **UI**: Bootstrap spinner
- **Sizes**: Small (w przyciskach), medium (inline), large (full page)
- **Full Page Loader**:
  - Fixed overlay (z-index wysoki)
  - Semi-transparent dark background
  - Centered spinner + "Loading..." tekst

#### 5.5.3 Empty State Component
- **Cel**: Informacja o braku danych
- **Typy**:
  - No applications (dashboard, job board)
  - No search results
  - No data dla wykresu
- **UI**:
  - Icon/illustration (SVG, centered)
  - Heading (np. "No applications yet")
  - Description text
  - CTA button (jeśli applicable)
- **Tone**: Przyjazny, zachęcający

#### 5.5.4 Validation Error Component
- **UI**: Czerwony tekst pod invalid field
- **Format**: Small text, icon (optional)
- **Trigger**: On blur lub on submit
- **Clear**: On valid input

### 5.6 Komponenty modalnych

#### 5.6.1 Generic Modal Component
- **UI**: Bootstrap modal
- **Parts**:
  - Header: Title + Close button (X)
  - Body: Content
  - Footer: Action buttons
- **Backdrop**: Click to close (opcjonalnie disable)
- **Keyboard**: Escape to close
- **Focus management**: Focus trap w modal

#### 5.6.2 Confirmation Modal
- **Extends**: Generic Modal
- **Props**: Title, Message, Confirm button text, Cancel button text
- **Confirm button**: Danger color dla destructive actions
- **Used for**: Delete confirmation

### 5.7 Utility Components

#### 5.7.1 Breadcrumb Component
- **UI**: Bootstrap breadcrumb
- **Data**: Array of {text, url}
- **Current page**: No link, aria-current="page"

#### 5.7.2 Tooltip Component
- **UI**: Bootstrap tooltip
- **Trigger**: Hover lub focus
- **Używane dla**: Icons, abbreviated info, additional context

#### 5.7.3 Back Button
- **UI**: Button z left arrow icon
- **Action**: Browser history back lub specific URL
- **Pozycja**: Top-left content area (pod breadcrumb jeśli istnieje)

## 6. Specyfikacja stanów i interakcji

### 6.1 Stany interaktywne

#### 6.1.1 Stany przycisków
- **Default**: Normal state
- **Hover**: Subtle background/border color change
- **Focus**: Outline (keyboard navigation)
- **Active**: Pressed state (darker background)
- **Disabled**: Gray, low opacity, cursor not-allowed
- **Loading**: Spinner + disabled + "Loading..." text

#### 6.1.2 Stany pól formularza
- **Default**: Normal border
- **Focus**: Highlight border (usually blue)
- **Valid**: Green checkmark (opcjonalnie)
- **Invalid**: Red border + error message below
- **Disabled**: Gray background, cursor not-allowed

#### 6.1.3 Stany kart aplikacji
- **Default**: White background, subtle shadow
- **Hover**: Elevated shadow, subtle background change
- **Active/Clicked**: Brief scale animation
- **Selected**: Highlighted border (jeśli bulk actions)

### 6.2 Animacje i transitions

**Zasady**:
- Subtle, fast (200-300ms)
- Use `ease-in-out` timing
- Respect `prefers-reduced-motion` media query

**Gdzie stosować**:
- Toast slide-in/fade-out
- Modal fade-in/fade-out
- Dropdown expand/collapse
- Button hover/active states
- Card hover elevation
- Skeleton shimmer effect

### 6.3 Responsive breakpoints (Bootstrap 5)

- **xs**: < 576px (mobile portrait)
- **sm**: ≥ 576px (mobile landscape)
- **md**: ≥ 768px (tablet)
- **lg**: ≥ 992px (small desktop)
- **xl**: ≥ 1200px (desktop)
- **xxl**: ≥ 1400px (large desktop)

**Kluczowe zmiany layoutu**:
- **< md**: 
  - Navbar collapsed (hamburger)
  - Filters off-canvas
  - Single column layouts
  - Stacked form fields
  - Charts full width
- **≥ md**:
  - Navbar expanded
  - Filters sidebar
  - Multi-column layouts (Job Board, Dashboard)
  - Form fields inline (gdzie sensowne)

### 6.4 Obsługa błędów UI

#### 6.4.1 Validation Errors
- **Client-side**: Inline messages, red borders, disabled submit
- **Server-side**: Validation summary + inline messages
- **UX**: Auto-scroll do pierwszego błędu, focus na invalid field

#### 6.4.2 AJAX Errors
- **Network error**: Toast error + "Please check your connection and try again"
- **Server error (500)**: Toast error + "Something went wrong. Please try again later"
- **Validation error (400)**: Display specific errors
- **Auth error (401)**: Redirect do login z returnUrl

#### 6.4.3 Form Submit Errors
- **Success**: Redirect + toast (lub redirect do success page)
- **Validation errors**: Stay on page, show errors
- **Server errors**: Stay on page, show error alert, enable retry

## 7. Mapowanie wymagań PRD do UI

### 7.1 Wymagania funkcjonalne → Widoki

| Wymaganie PRD | Widok/Komponent | Implementacja |
|---------------|-----------------|---------------|
| **3.1.1 Dodawanie nowej aplikacji** | Create Application View | AI Parsing Section + _JobApplicationForm |
| **3.1.2 System umiejętności** | Skills Manager Component | Input + dropdown + tags list, max 20 |
| **3.1.3 Edycja aplikacji** | Edit Application View | _JobApplicationForm pre-filled |
| **3.1.4 Usuwanie aplikacji** | Delete Confirmation Modal | Modal w Job Board i Details |
| **3.2 Statusy aplikacji** | Status Badge + Status Dropdown | 7 statusów, kolorowe badges |
| **3.3 Job Board** | Job Board View | Cards layout + Sidebar filters |
| **3.4 Filtrowanie i wyszukiwanie** | Filter Sidebar | Multi-select, sliders, text inputs |
| **3.5 Dashboard** | Dashboard View | Metric cards + charts + recent table |
| **3.6 Strona szczegółów** | Application Details View | Two-column layout, all info |
| **US-001 Rejestracja** | Register View | Form z email, password, confirmation |
| **US-002 Logowanie** | Login View | Form z email, password, remember me |
| **US-003 Wylogowanie** | Profile Menu → Logout | POST do /Auth/Logout |
| **US-004 Reset hasła** | ForgotPassword + ResetPassword | Email form + new password form |

### 7.2 User Stories → Przepływy UI

| User Story | UI Flow | Kluczowe elementy |
|------------|---------|-------------------|
| **US-005 Dodanie z AI** | Create → Paste → Parse → Review → Submit | AI Parsing Section, AJAX feedback |
| **US-006 Ręczne dodanie** | Create → Fill manually → Submit | _JobApplicationForm |
| **US-008 Przeglądanie listy** | Job Board → View cards | Application Cards |
| **US-009 Szczegóły** | Job Board → Click card → Details | Application Details View |
| **US-010 Edycja** | Details → Edit → Save | Edit View |
| **US-011 Usunięcie** | Details/Job Board → Delete → Confirm | Delete Modal |
| **US-012 Quick status** | Job Board/Details → Status dropdown → Update | Status Dropdown (AJAX) |
| **US-013-017 Filtrowanie** | Job Board → Sidebar filters → Apply | Filter Sidebar |
| **US-018 Wyszukiwanie** | Job Board → Search input → Submit | Search bar |
| **US-021-024 Dashboard** | Dashboard → View metrics/charts | Metric Cards, Charts |
| **US-028 Mobile** | All views → Responsive layouts | Off-canvas filters, stacked layouts |

### 7.3 Kluczowe korzyści PRD → Realizacja UI

| Korzyść | Realizacja w UI |
|---------|-----------------|
| **Oszczędność czasu** | AI Parsing Section z AJAX, auto-fill formularza w sekundach |
| **Centralizacja** | Job Board - wszystkie aplikacje w jednym miejscu |
| **Śledzenie postępów** | Dashboard z metrykami i wykresami |
| **Elastyczność** | Status Dropdown - zmiana statusu bez ograniczeń |
| **Dostępność** | Responsive design, mobile-first, WCAG 2.1 AA |

## 8. Specyfikacja dostępności (WCAG 2.1 AA)

### 8.1 Kluczowe wymagania

#### 8.1.1 Keyboard Navigation
- Wszystkie interaktywne elementy dostępne z klawiatury (Tab, Enter, Space, Arrow keys)
- Logiczny tab order (visual order = DOM order)
- Visible focus indicators (nie usuwać outline)
- Skip to main content link
- Keyboard shortcuts (opcjonalnie): `Alt+N` dla New Application, `Alt+D` dla Dashboard

#### 8.1.2 Screen Reader Support
- Semantyczny HTML (nav, main, aside, section, article, header, footer)
- ARIA labels gdzie potrzebne:
  - `aria-label` dla icon-only buttons
  - `aria-labelledby` dla form sections
  - `aria-describedby` dla help text
  - `aria-live` regions dla dynamic updates (toast, AJAX updates)
  - `aria-current="page"` dla aktywnej strony w navbar
- Alt text dla wszystkich obrazów (informacyjnych)
- Decorative images: `alt=""` lub CSS background

#### 8.1.3 Kontrast kolorów
- Normalny tekst: minimum 4.5:1
- Duży tekst (≥18pt lub ≥14pt bold): minimum 3:1
- UI components i graphics: minimum 3:1
- Bootstrap colors generally compliant, verify custom colors

#### 8.1.4 Formularze
- Label dla każdego input (explicit `<label for="id">`)
- Grouping z `<fieldset>` i `<legend>` dla radio/checkbox groups
- Error messages powiązane z polami (`aria-describedby`)
- Required fields oznaczone (`required` attribute + visual indicator)

#### 8.1.5 Responsywność i Zoom
- Text może być powiększony do 200% bez utraty funkcjonalności
- No horizontal scrolling przy 320px viewport width
- Touch targets minimum 44x44px (mobile)

### 8.2 Testowanie dostępności

**Narzędzia** (do użycia podczas development):
- Browser extensions: axe DevTools, WAVE
- Keyboard-only testing (odłącz mysz)
- Screen reader testing: NVDA (Windows), VoiceOver (Mac)
- Lighthouse Accessibility audit

## 9. Specyfikacja bezpieczeństwa UI

### 9.1 Ochrona przed atakami

#### 9.1.1 CSRF Protection
- Wszystkie formularze POST/PUT/DELETE: `@Html.AntiForgeryToken()`
- AJAX requests: Include anti-forgery token w headers

#### 9.1.2 XSS Prevention
- Razor automatycznie enkoduje output (`@Model.Property`)
- Raw HTML tylko gdy niezbędne (`@Html.Raw`) i po sanityzacji
- User-generated content: zawsze enkodowany
- CSP headers (backend configuration)

#### 9.1.3 Sensitive Data
- Hasła: `type="password"` (no autocomplete dla new passwords)
- No logging sensitive data w console
- Tokens w URL: expire quickly, single-use

### 9.2 UI Security Best Practices

#### 9.2.1 Authentication UI
- Clear feedback na success/failure (bez ujawniania czy email istnieje)
- Account lockout info (bez szczegółów)
- HTTPS only (enforced w backend)

#### 9.2.2 Authorization UI
- Sensitive actions: confirmation modals
- Delete: clear warning o irreversibility
- No exposed IDs (GUIDs are fine, sequential integers risky)

#### 9.2.3 Input Validation
- Client-side: convenience i UX
- Server-side: security (never trust client)
- Sanitization: backend responsibility
- Length limits: prevent DoS (textarea max length)

## 10. Paleta kolorów i stylizacja

### 10.1 Kolory główne (Bootstrap 5 based)

#### 10.1.1 Brand Colors
- **Primary**: Bootstrap Blue (#0d6efd) - CTA buttons, links, primary elements
- **Secondary**: Bootstrap Gray (#6c757d) - secondary buttons, subtle elements
- **Success**: Bootstrap Green (#198754) - success states, offers
- **Warning**: Bootstrap Orange/Yellow (#ffc107) - warnings, interviews
- **Danger**: Bootstrap Red (#dc3545) - errors, rejected, delete
- **Info**: Bootstrap Light Blue (#0dcaf0) - info messages
- **Light**: Bootstrap Light Gray (#f8f9fa) - backgrounds
- **Dark**: Bootstrap Dark (#212529) - text, dark elements

#### 10.1.2 Status Colors (mapped to Bootstrap)
- Draft: Secondary (gray)
- Submitted: Primary (blue)
- Interview Scheduled: Warning (orange/yellow)
- Waiting for Offer: Info (light blue)
- Received Offer: Success (green)
- Rejected: Danger (red)
- No Contact: Dark (dark gray)

#### 10.1.3 Skill Level Colors
- Nice to Have: Secondary (gray)
- Regular: Primary (blue)
- Advanced: Warning (orange)
- Master: Custom golden (#ffd700 lub podobny)

### 10.2 Typography

**Font Stack**: Bootstrap default (system fonts)
```
-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif
```

**Hierarchy**:
- H1: 2.5rem, bold (page titles)
- H2: 2rem, bold (section titles)
- H3: 1.75rem, semi-bold (sub-sections)
- H4: 1.5rem, semi-bold (card titles)
- Body: 1rem, normal (16px base)
- Small: 0.875rem (14px) (meta info, captions)

### 10.3 Spacing

**Bootstrap spacing scale** (używaj utility classes):
- 0: 0
- 1: 0.25rem (4px)
- 2: 0.5rem (8px)
- 3: 1rem (16px)
- 4: 1.5rem (24px)
- 5: 3rem (48px)

**Margins/Padding**:
- Sections: `mb-4` lub `mb-5`
- Cards: `p-3` lub `p-4`
- Form groups: `mb-3`
- Buttons: `px-4 py-2`

### 10.4 Shadows i Borders

**Shadows** (Bootstrap utilities):
- Cards default: `shadow-sm`
- Cards hover: `shadow`
- Modals: `shadow-lg`
- Navbar: `shadow-sm` (optional)

**Borders**:
- Default: 1px solid rgba(0,0,0,.125)
- Radius: 0.25rem (Bootstrap default)
- Badges: rounded-pill (opcjonalnie)

## 11. Podsumowanie architektury

### 11.1 Kluczowe decyzje architektoniczne

1. **Server-Side Rendering + Selective AJAX**: Większość operacji to pełne przeładowania strony (prostsza implementacja, lepsza dla SEO i accessibility), z AJAX tylko dla AI parsing i quick status updates.

2. **Shared Partial Views**: Formularz aplikacji (`_JobApplicationForm.cshtml`) współdzielony między Create i Edit dla maksymalnej reużywalności i spójności.

3. **URL-based Filter State**: Stan filtrów w query parameters umożliwia linkowanie do przefiltrowanych widoków i łatwiejszy back button behavior.

4. **Component-based Approach**: Mimo server-side rendering, myślenie o UI w kategoriach reużywalnych komponentów (partial views, view components) dla łatwiejszego maintenance.

5. **Bootstrap 5 Foundation**: Wykorzystanie Bootstrap dla responsywności, komponentów i accessibility best practices out of the box.

6. **Progressive Enhancement**: Podstawowa funkcjonalność działa bez JavaScript (formularze, nawigacja), JavaScript dodaje lepsze UX (AJAX, real-time walidacja).

### 11.2 Kluczowe przepływy

**Najważniejsze ścieżki użytkownika**:
1. Register → Email Confirmation → Dashboard (onboarding)
2. Dashboard → Add Application → AI Parse → Submit → Details (dodawanie)
3. Job Board → Filter/Search → View Results → Details (przeglądanie)
4. Details → Quick Status Change (zarządzanie)
5. Dashboard → Analyze Metrics → Job Board (monitorowanie postępów)

### 11.3 Główne widoki (hierarchia ważności)

**Tier 1** (kluczowe dla MVP):
- Job Board (główny widok pracy)
- Create Application (główna akcja użytkownika)
- Dashboard (punkt startowy)
- Application Details (szczegóły)

**Tier 2** (ważne):
- Edit Application
- Login/Register
- Email Confirmation

**Tier 3** (pomocnicze):
- Landing Page
- Reset Password
- Error/404 pages

### 11.4 Kluczowe komponenty do implementacji

**Must-have dla MVP**:
1. _JobApplicationForm.cshtml (partial)
2. Application Card Component
3. Status Badge Component
4. Skills Manager Component
5. Toast Notification System
6. Delete Confirmation Modal
7. Status Dropdown (AJAX)
8. Filter Sidebar (responsive)
9. Chart Components (Dashboard)
10. Empty State Components

### 11.5 Następne kroki implementacji

**Rekomendowana kolejność**:
1. **Setup infrastructure**: Layout.cshtml, navbar, footer, Bootstrap, validation scripts
2. **Authentication views**: Register, Login, Logout (podstawa dostępu)
3. **Core CRUD**: Create, Details, Edit, Delete (fundament funkcjonalności)
4. **Job Board**: Lista + filtry (główny widok)
5. **Dashboard**: Metryki + wykresy (user engagement)
6. **Polish**: Toast notifications, modals, loading states, empty states
7. **Accessibility audit**: Keyboard nav, screen reader, contrast
8. **Responsive testing**: Mobile, tablet, desktop
9. **UX testing**: User flows, edge cases, error handling

---

## Załącznik A: Checklist dla każdego widoku

Przy implementacji każdego widoku, zweryfikuj:

- [ ] Responsive layout (mobile, tablet, desktop)
- [ ] Keyboard navigation (tab order, focus states)
- [ ] Screen reader support (semantic HTML, ARIA labels)
- [ ] Form validation (client + server)
- [ ] Error handling (validation errors, AJAX errors, server errors)
- [ ] Loading states (buttons, full page, AJAX)
- [ ] Empty states (brak danych)
- [ ] Success feedback (toast notifications, success messages)
- [ ] Security (CSRF tokens, input validation, authorization checks)
- [ ] Accessibility (kontrast, labels, alt text)
- [ ] UX polish (animations, transitions, hover states)
- [ ] Cross-browser testing (Chrome, Firefox, Safari, Edge)

## Załącznik B: Konwencje nazewnictwa

### B.1 Pliki widoków
- **Views**: PascalCase, np. `Dashboard.cshtml`, `JobBoard.cshtml`
- **Partial Views**: Prefix `_`, np. `_JobApplicationForm.cshtml`, `_StatusBadge.cshtml`
- **Layouts**: `_Layout.cshtml`

### B.2 CSS Classes
- **Bootstrap**: Użyj utility classes gdzie możliwe
- **Custom**: BEM notation, np. `application-card`, `application-card__title`, `application-card--featured`
- **JavaScript hooks**: Prefix `js-`, np. `js-delete-confirm`, `js-status-dropdown`

### B.3 JavaScript
- **Functions**: camelCase, np. `parseJobDescription()`, `updateApplicationStatus()`
- **Event handlers**: prefix `handle`, np. `handleDeleteClick()`, `handleFormSubmit()`
- **DOM elements**: prefix `$`, np. `$submitButton`, `$statusDropdown`

### B.4 IDs i Names
- **IDs**: camelCase, unique per page, np. `applicationForm`, `statusDropdown`
- **Names**: match model property, np. `CompanyName`, `SalaryMin`
- **ARIA IDs**: descriptive, np. `application-form-errors`, `status-dropdown-menu`

