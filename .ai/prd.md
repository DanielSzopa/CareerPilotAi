# Dokument wymagań produktu (PRD) - CareerPilotAi

## 1. Przegląd produktu

### 1.1 Wizja produktu
CareerPilotAi to aplikacja webowa wspierająca osoby w procesie poszukiwania pracy poprzez inteligentne śledzenie aplikacji o pracę z wykorzystaniem sztucznej inteligencji. Produkt automatyzuje czasochłonne zadania związane z organizacją procesu rekrutacyjnego, pozwalając użytkownikom skupić się na kluczowych aspektach poszukiwania pracy.

### 1.2 Grupa docelowa
- Grupa pierwotna: Osoby szukające pierwszej pracy (absolwenci, juniorzy)
- Grupa wtórna: Profesjonaliści rozważający zmianę kariery lub miejsca pracy

### 1.3 Propozycja wartości
W kilku prostych krokach użytkownik może dodać nową aplikację o pracę poprzez skopiowanie zawartości ogłoszenia z portalu rekrutacyjnego - AI automatycznie wypełnia formularz aplikacji, zachowując pełną kontrolę użytkownika nad finalnymi danymi. System umożliwia śledzenie wszystkich aplikacji w jednym miejscu z zaawansowanymi możliwościami filtrowania i analizy postępów.

### 1.4 Kluczowe korzyści
- Oszczędność czasu: AI parsuje ogłoszenia o pracę w kilka sekund
- Centralizacja informacji: Wszystkie aplikacje w jednym miejscu
- Śledzenie postępów: Dashboard z wizualizacją statusów i trendów
- Elastyczność: Pełna kontrola użytkownika nad danymi i statusami
- Dostępność: Responsywny design działający na wszystkich urządzeniach

## 2. Problem użytkownika

### 2.1 Główne wyzwania
Osoby poszukujące pracy napotykają następujące problemy:

1. Rozproszenie informacji - aplikacje rozsiane po różnych portalach, emailach i notatkach
2. Czasochłonne wprowadzanie danych - ręczne przepisywanie informacji z ogłoszeń
3. Brak systematycznego śledzenia - trudność w monitorowaniu statusów wielu aplikacji
4. Utrata kontekstu - zapominanie szczegółów o aplikacjach złożonych wcześniej
5. Brak wizualizacji postępów - trudność w ocenie efektywności poszukiwań

### 2.2 Obecne rozwiązania i ich ograniczenia
- Arkusze kalkulacyjne: Wymagają ręcznego wprowadzania wszystkich danych, brak automatyzacji
- Notatki: Brak struktury, trudne do przeszukiwania i filtrowania
- Portale rekrutacyjne: Ograniczone do aplikacji złożonych przez dany portal
- Aplikacje do zarządzania zadaniami: Nie są dostosowane do specyfiki procesu rekrutacyjnego

### 2.3 Rozwiązanie CareerPilotAi
System rozwiązuje powyższe problemy poprzez:
- Automatyczne parsowanie ogłoszeń przy użyciu AI
- Centralizację wszystkich aplikacji niezależnie od źródła
- Intuicyjny interfejs z zaawansowanymi filtrami
- Automatyczne śledzenie historii zmian statusów
- Dashboard z wizualizacją kluczowych metryk

## 3. Wymagania funkcjonalne

### 3.1 Zarządzanie aplikacjami

#### 3.1.1 Dodawanie nowej aplikacji
- Textarea do wklejania skopiowanej treści ogłoszenia (limit 5000 słów)
- AI parsowanie z trzema możliwymi rezultatami:
  - Sukces pełny: wszystkie pola wypełnione automatycznie
  - Sukces częściowy: część pól wymaga uzupełnienia
  - Błąd: wymagana ponowna próba z lepszymi danymi
- Formularz z następującymi polami:
  - Nazwa firmy (wymagane, 2-100 znaków)
  - Stanowisko (wymagane, 2-100 znaków)
  - Opis stanowiska (wymagane, 50-5000 słów)
  - Umiejętności (opcjonalne, max 20 pozycji)
  - Poziom doświadczenia (Junior/Mid/Senior/Not specified)
  - Lokalizacja (wymagane, 2-100 znaków)
  - Tryb pracy (wymagane, Remote/Hybrid/On-site)
  - Typ umowy (wymagane, B2B/FTE/Zlecenie/Inne)
  - Widełki płacowe (opcjonalne, PLN, brutto/netto, plus dodatkowy input w celu wskazania czy wynagrodzenie jest monthly, daily, hourly, yearly)
  - URL oferty (opcjonalne, walidowany format URL)
  - Status (domyślnie Draft)

#### 3.1.2 System umiejętności
- Text input do dodawania kolejnych umiejętności
- Każda umiejętność z poziomem: Nice to have/Regular/Advanced/Master
- Licznik pokazujący X/20 umiejętności
- AI automatycznie klasyfikuje poziomy na podstawie treści ogłoszenia

#### 3.1.3 Edycja aplikacji
- Osobny widok edycji z pre-wypełnionymi polami
- Wszystkie pola edytowalne
- Walidacja real-time przy opuszczeniu pola (on blur)
- Możliwość zmiany statusu

#### 3.1.4 Usuwanie aplikacji
- Modal potwierdzenia z ostrzeżeniem o nieodwracalności akcji
- Toast notification po pomyślnym usunięciu
- Redirect do widoku listy aplikacji z zachowaniem filtrów

### 3.2 Statusy aplikacji

#### 3.2.1 Dostępne statusy
- Draft - wersja robocza
- Submitted - aplikacja wysłana
- Interview Scheduled - umówiona rozmowa
- Waiting for offer - oczekiwanie na ofertę
- Received offer - otrzymana oferta
- Rejected - odrzucona aplikacja
- No contact - brak odpowiedzi

#### 3.2.2 Zarządzanie statusami
- Brak sztywnej mapy przejść między statusami
- Użytkownik może zmienić dowolny status na dowolny
- Quick change poprzez dropdown na karcie aplikacji
- Zmiana statusu w formularzu edycji i na stronie szczegółów

### 3.3 Job Board - lista aplikacji

#### 3.3.1 Layout
- Desktop: sidebar z filtrami + główna zawartość
- Mobile: zwijany drawer z filtrami
- Podłużne karty w stylu JustJoinIt

#### 3.3.2 Zawartość karty
- Nazwa firmy i stanowisko
- Pierwsze 3 umiejętności + "Show more"
- Lokalizacja i tryb pracy
- Status (kolorowy badge)
- Typ umowy
- Widełki płacowe
- data utworzenia aplikacji

#### 3.3.3 Quick actions
- Kliknięcie na kartę → strona szczegółów
- Dropdown statusu dla szybkiej zmiany
- Menu kebab: Edit/Delete

### 3.4 System filtrowania i wyszukiwania

#### 3.4.1 Filtry (sidebar)
- Multi-select statusów
- Slider zakresu płac (plus dropdown dla monthly, daily, hourly, yearly, (default monthly))
- Pole lokalizacji
- Checkboxy trybu pracy
- Checkboxy poziomu doświadczenia
- Przycisk "Clear all filters"
- Przycisk "Apply filters"

#### 3.4.2 Wyszukiwanie
- Pole tekstowe w top bar
- Przeszukiwanie nazwy firmy i stanowiska
- Case-insensitive

#### 3.4.3 Sortowanie
- Date added (newest first) - domyślne
- Date added (oldest first)

### 3.5 Dashboard

#### 3.5.1 Layout
- Grid 2x2 na desktop
- Pojedyncza kolumna na mobile

#### 3.5.2 Komponenty
1. Karty z kluczowymi metrykami:
   - Total Applications
   - Submitted
   - Interviews
   - Offers

2. Wykres kołowy statusów:
   - Podział procentowy
   - Kolorowa legenda

3. Wykres słupkowy aplikacji w czasie:
   - Toggle: 30/60/90 dni

4. Tabela ostatniej aktywności:
   - 5 ostatnich dodanych aplikacji
   - Quick actions: View

### 3.6 Strona szczegółów aplikacji

#### 3.6.1 Zawartość
- Nagłówek z nazwą firmy i stanowiskiem
- Pełny opis stanowiska (zachowane formatowanie)
- Lista wszystkich umiejętności z poziomami
- Informacje o lokalizacji i trybie pracy
- Typ umowy i widełki płacowe
- Link do oryginalnej oferty
- Daty utworzenia i ostatniej aktualizacji

#### 3.6.2 Akcje
- Edit - redirect do formularza edycji
- Delete - modal potwierdzenia
- Change Status - dropdown

### 3.7 Wymagania techniczne

#### 3.7.1 Frontend
- ASP.NET Core MVC z Razor Views
- Responsywny design mobile-first
- JavaScript/AJAX dla interaktywności
- Biblioteka wykresów (np. Chart.js)

#### 3.7.2 Backend
- .NET 8 z C#
- Entity Framework Core
- PostgreSQL (Docker Compose)
- OpenRouter dla integracji AI

#### 3.7.3 Bezpieczeństwo
- ASP.NET Identity dla autentykacji
- Autoryzacja na wszystkich endpoints
- Walidacja server-side i client-side
- Ochrona CSRF
- Parametryzowane zapytania SQL
- Auto-encoding XSS w Razor

## 4. Granice produktu

### 4.1 W zakresie MVP
- Dodawanie aplikacji z AI parsing
- Pełne CRUD operacje na aplikacjach
- System statusów z historią zmian
- Filtrowanie i wyszukiwanie
- Dashboard ze statystykami
- Responsywny design
- Autentykacja użytkowników

### 4.2 Poza zakresem MVP
- Generowanie CV/Resume
- Generowanie pytań rekrutacyjnych
- Generowanie listów motywacyjnych
- Paginacja listy aplikacji
- Bulk actions na wielu aplikacjach
- Archiwizowanie aplikacji
- Export danych (CSV/JSON/PDF)
- Onboarding/tutorial
- Landing page dla niezalogowanych
- Monetyzacja/płatne plany
- Pełna zgodność GDPR
- Zaawansowane metryki i analityka
- Autosave drafts
- Persystencja filtrów w URL
- Autocomplete dla technologii i lokalizacji
- Wsparcie wielu walut
- Automatyczna ekstrakcja URL z tekstu
- Historia zmian statusu. Automatyczny audit log przy każdej zmianie. Format: Data/Czas | Stary status → Nowy status. Wyświetlana jako tabela na stronie szczegółów aplikacji

### 4.3 Roadmap post-MVP
1. Priority 1: Generowanie/dostosowywanie CV pod ofertę
2. Priority 2: Generowanie przykładowych pytań rekrutacyjnych
3. Priority 3: Generowanie listów motywacyjnych

## 5. Historyjki użytkowników

### 5.1 Zarządzanie kontem

#### US-001: Rejestracja nowego użytkownika
- ID: US-001
- Tytuł: Rejestracja nowego konta
- Opis: Jako nowy użytkownik chcę móc założyć konto w aplikacji, aby rozpocząć śledzenie moich aplikacji o pracę
- Kryteria akceptacji:
  - Formularz rejestracji zawiera pola: email, hasło, potwierdzenie hasła
  - Email musi być unikalny w systemie
  - Hasło musi spełniać wymagania bezpieczeństwa (min. 8 znaków, duża litera, cyfra)
  - Po pomyślnej rejestracji użytkownik jest automatycznie zalogowany
  - Użytkownik otrzymuje email potwierdzający rejestrację

#### US-002: Logowanie do aplikacji
- ID: US-002
- Tytuł: Logowanie użytkownika
- Opis: Jako zarejestrowany użytkownik chcę móc zalogować się do aplikacji, aby uzyskać dostęp do moich danych
- Kryteria akceptacji:
  - Formularz logowania zawiera pola: email, hasło
  - Opcja "Remember me" dostępna
  - Po pomyślnym logowaniu redirect do dashboard
  - Wyświetlenie komunikatu błędu przy niepoprawnych danych
  - Możliwość resetu hasła

#### US-003: Wylogowanie z aplikacji
- ID: US-003
- Tytuł: Wylogowanie użytkownika
- Opis: Jako zalogowany użytkownik chcę móc bezpiecznie wylogować się z aplikacji
- Kryteria akceptacji:
  - Przycisk wylogowania dostępny w menu użytkownika
  - Po wylogowaniu redirect do strony logowania
  - Sesja użytkownika jest zakończona
  - Ciasteczka sesyjne są usuwane

#### US-004: Reset hasła
- ID: US-004
- Tytuł: Resetowanie zapomnianego hasła
- Opis: Jako użytkownik chcę móc zresetować zapomniane hasło
- Kryteria akceptacji:
  - Link "Forgot password?" na stronie logowania
  - Formularz z polem email
  - Email z linkiem resetującym wysyłany na podany adres
  - Link resetujący ważny przez 24 godziny
  - Nowe hasło musi spełniać wymagania bezpieczeństwa

### 5.2 Dodawanie aplikacji

#### US-005: Dodanie nowej aplikacji z AI parsing
- ID: US-005
- Tytuł: Dodanie aplikacji poprzez AI
- Opis: Jako użytkownik chcę dodać nową aplikację poprzez wklejenie tekstu ogłoszenia, aby AI automatycznie wypełniło formularz
- Kryteria akceptacji:
  - Textarea akceptuje do 5000 słów tekstu
  - Przycisk "Parse with AI" uruchamia analizę
  - Loading state z komunikatem "Analyzing job description..."
  - AI wypełnia rozpoznane pola formularza
  - Użytkownik może edytować wypełnione pola przed zapisem
  - Komunikat o sukcesie/częściowym sukcesie/błędzie

#### US-006: Ręczne dodanie aplikacji
- ID: US-006
- Tytuł: Ręczne wypełnienie formularza aplikacji
- Opis: Jako użytkownik chcę móc ręcznie wypełnić formularz aplikacji, gdy nie mam tekstu do sparsowania
- Kryteria akceptacji:
  - Wszystkie wymagane pola muszą być wypełnione
  - Walidacja on blur dla każdego pola
  - Komunikaty o błędach wyświetlane pod polami
  - Możliwość dodania do 20 umiejętności
  - Po zapisie redirect do strony szczegółów

#### US-007: Dodanie umiejętności do aplikacji
- ID: US-007
- Tytuł: Zarządzanie umiejętnościami
- Opis: Jako użytkownik chcę dodać i skategoryzować umiejętności wymagane w ofercie
- Kryteria akceptacji:
  - Możliwość dodania do 20 umiejętności
  - Każda umiejętność ma poziom (Nice to have/Regular/Advanced/Master)
  - Licznik pokazuje X/20 umiejętności
  - Możliwość usunięcia umiejętności
  - AI sugeruje poziomy na podstawie opisu

### 5.3 Przeglądanie aplikacji

#### US-008: Przeglądanie listy aplikacji
- ID: US-008
- Tytuł: Wyświetlanie wszystkich aplikacji
- Opis: Jako użytkownik chcę widzieć listę wszystkich moich aplikacji w jednym miejscu
- Kryteria akceptacji:
  - Karty wyświetlają kluczowe informacje o aplikacji
  - Responsywny layout (desktop/mobile)
  - Wszystkie aplikacje ładują się od razu (brak paginacji)
  - Kliknięcie na kartę otwiera szczegóły

#### US-009: Wyświetlanie szczegółów aplikacji
- ID: US-009
- Tytuł: Przeglądanie pełnych szczegółów
- Opis: Jako użytkownik chcę zobaczyć wszystkie szczegóły wybranej aplikacji
- Kryteria akceptacji:
  - Wszystkie informacje o aplikacji są widoczne
  - Pełny opis stanowiska z zachowanym formatowaniem
  - Lista wszystkich umiejętności z poziomami
  - Przyciski akcji (Edit/Delete)
  - Zmiana status quick action

### 5.4 Edycja i usuwanie

#### US-010: Edycja aplikacji
- ID: US-010
- Tytuł: Modyfikacja danych aplikacji
- Opis: Jako użytkownik chcę móc edytować dane zapisanej aplikacji
- Kryteria akceptacji:
  - Formularz pre-wypełniony aktualnymi danymi
  - Walidacja jak przy dodawaniu
  - Po zapisie redirect do strony szczegółów
  - Toast notification "Changes saved"

#### US-011: Usunięcie aplikacji
- ID: US-011
- Tytuł: Usuwanie aplikacji z systemu
- Opis: Jako użytkownik chcę móc trwale usunąć aplikację
- Kryteria akceptacji:
  - Modal potwierdzenia z ostrzeżeniem
  - Tekst: "Are you sure? This action cannot be undone."
  - Przyciski Cancel i Delete (czerwony)
  - Toast notification po usunięciu
  - Redirect do listy aplikacji

#### US-012: Zmiana statusu aplikacji (quick change)
- ID: US-012
- Tytuł: Szybka zmiana statusu
- Opis: Jako użytkownik chcę móc szybko zmienić status aplikacji bez wchodzenia w edycję
- Kryteria akceptacji:
  - Dropdown ze statusami na karcie aplikacji
  - Dropdown w szczegółach aplikacji
  - Natychmiastowa aktualizacja po zmianie
  - Zapis w historii zmian
  - Brak ograniczeń przejść między statusami

### 5.5 Filtrowanie i wyszukiwanie

#### US-013: Filtrowanie po statusie
- ID: US-013
- Tytuł: Filtrowanie aplikacji po statusie
- Opis: Jako użytkownik chcę filtrować aplikacje według ich statusu
- Kryteria akceptacji:
  - Multi-select checkboxy dla wszystkich statusów
  - Możliwość wyboru wielu statusów jednocześnie
  - Licznik wyników po zastosowaniu filtra

#### US-014: Filtrowanie po wynagrodzeniu
- ID: US-014
- Tytuł: Filtrowanie po widełkach płacowych
- Opis: Jako użytkownik chcę filtrować aplikacje według zakresu wynagrodzeń
- Kryteria akceptacji:
  - Slider z min i max wartością
  - Wartości w PLN
  - Dropdown dla monthly, daily, hourly, yearly
  - Filtrowanie uwzględnia brutto/netto


#### US-015: Filtrowanie po lokalizacji
- ID: US-015
- Tytuł: Filtrowanie po miejscu pracy
- Opis: Jako użytkownik chcę filtrować aplikacje według lokalizacji
- Kryteria akceptacji:
  - Pole tekstowe dla lokalizacji
  - Partial match (zawiera frazę)
  - Case-insensitive
  - Możliwość wyczyszczenia filtra

#### US-016: Filtrowanie po trybie pracy
- ID: US-016
- Tytuł: Filtrowanie po trybie pracy
- Opis: Jako użytkownik chcę filtrować aplikacje według trybu pracy
- Kryteria akceptacji:
  - Checkboxy: Remote, Hybrid, On-site
  - Możliwość wyboru wielu trybów
  - Filtrowanie w czasie rzeczywistym

#### US-017: Filtrowanie po poziomie doświadczenia
- ID: US-017
- Tytuł: Filtrowanie po poziomie stanowiska
- Opis: Jako użytkownik chcę filtrować aplikacje według wymaganego doświadczenia
- Kryteria akceptacji:
  - Checkboxy: Junior, Mid, Senior, Not specified
  - Możliwość wyboru wielu poziomów
  - Filtrowanie w czasie rzeczywistym

#### US-018: Wyszukiwanie tekstowe
- ID: US-018
- Tytuł: Wyszukiwanie po nazwie firmy i stanowisku
- Opis: Jako użytkownik chcę wyszukiwać aplikacje po nazwie firmy lub stanowiska
- Kryteria akceptacji:
  - Pole wyszukiwania w top bar
  - Przeszukiwanie nazwy firmy i stanowiska
  - Case-insensitive
  - Wyszukiwanie w czasie rzeczywistym (po 300ms delay)

#### US-019: Czyszczenie filtrów
- ID: US-019
- Tytuł: Reset wszystkich filtrów
- Opis: Jako użytkownik chcę móc szybko wyczyścić wszystkie zastosowane filtry
- Kryteria akceptacji:
  - Przycisk "Clear all filters"
  - Resetuje wszystkie filtry do stanu domyślnego
  - Wyświetla wszystkie aplikacje
  - Dostępny gdy jakikolwiek filtr jest aktywny

### 5.6 Sortowanie

#### US-020: Sortowanie po dacie dodania
- ID: US-020
- Tytuł: Sortowanie aplikacji chronologicznie
- Opis: Jako użytkownik chcę sortować aplikacje według daty dodania
- Kryteria akceptacji:
  - Dropdown z opcjami sortowania
  - "Date added (newest first)" - domyślne
  - "Date added (oldest first)"
  - Natychmiastowa zmiana kolejności

### 5.7 Dashboard i statystyki

#### US-021: Przeglądanie podsumowania aplikacji
- ID: US-021
- Tytuł: Wyświetlanie kluczowych metryk
- Opis: Jako użytkownik chcę widzieć podsumowanie moich aplikacji
- Kryteria akceptacji:
  - Karty z liczbami: Total, Submitted, Interviews, Offers
  - Wartości aktualizowane pod odświeżeniu strony
  - Responsywny layout (grid 2x2 / kolumna)

#### US-022: Przeglądanie rozkładu statusów
- ID: US-022
- Tytuł: Wizualizacja statusów aplikacji
- Opis: Jako użytkownik chcę widzieć procentowy rozkład statusów moich aplikacji
- Kryteria akceptacji:
  - Wykres kołowy/donut z podziałem na statusy
  - Legenda z nazwami i procentami
  - Kolorowe segmenty dla każdego statusu
  - Interaktywny (hover pokazuje wartości)

#### US-023: Przeglądanie aplikacji w czasie
- ID: US-023
- Tytuł: Trend aplikacji w czasie
- Opis: Jako użytkownik chcę widzieć ile aplikacji dodałem w ostatnim czasie
- Kryteria akceptacji:
  - Wykres słupkowy pokazujący liczbę aplikacji
  - Toggle: ostatnie 30/60/90 dni
  - Oś X: dni/tygodnie
  - Oś Y: liczba aplikacji

#### US-024: Przeglądanie ostatniej aktywności
- ID: US-024
- Tytuł: Lista ostatnich aplikacji
- Opis: Jako użytkownik chcę widzieć swoje najnowsze aplikacje
- Kryteria akceptacji:
  - Tabela z 5 ostatnimi aplikacjami
  - Kolumny: Company, Position, Status, Date
  - Quick actions: View, Edit
  - Sortowanie po dacie (najnowsze pierwsze)

### 5.8 Obsługa błędów

#### US-025: Obsługa błędu parsowania AI
- ID: US-025
- Tytuł: Komunikat o błędzie AI
- Opis: Jako użytkownik chcę otrzymać jasną informację gdy AI nie może sparsować tekstu
- Kryteria akceptacji:
  - Czerwony alert z opisem problemu
  - Wskazówki: "Ensure text contains: company, position, requirements"
  - Przycisk "Retry" do ponownej próby
  - Możliwość ręcznego wypełnienia formularza

#### US-026: Obsługa częściowego sukcesu AI
- ID: US-026
- Tytuł: Informacja o niekompletnym parsowaniu
- Opis: Jako użytkownik chcę wiedzieć które pola wymagają uzupełnienia po AI parsowaniu
- Kryteria akceptacji:
  - Pomarańczowy komunikat z listą brakujących pól
  - Pola do uzupełnienia oznaczone wizualnie
  - Możliwość kontynuacji wypełniania formularza

#### US-027: Walidacja formularzy
- ID: US-027
- Tytuł: Komunikaty o błędach walidacji
- Opis: Jako użytkownik chcę otrzymywać jasne komunikaty o błędach w formularzach
- Kryteria akceptacji:
  - Błędy wyświetlane pod odpowiednimi polami
  - Czerwony border dla pól z błędami
  - Walidacja on blur (po opuszczeniu pola)
  - Jasne komunikaty (np. "Field is required", "Must be 2-100 characters")

### 5.9 Responsywność

#### US-028: Korzystanie z aplikacji na urządzeniu mobilnym
- ID: US-028
- Tytuł: Mobilna wersja aplikacji
- Opis: Jako użytkownik chcę korzystać z aplikacji na smartfonie
- Kryteria akceptacji:
  - Wszystkie funkcje dostępne na mobile
  - Collapsible sidebar z filtrami
  - Karty aplikacji w pojedynczej kolumnie
  - Dashboard w układzie kolumnowym
  - Dotykowe interakcje (swipe, tap)

#### US-029: Korzystanie z aplikacji na tablecie
- ID: US-029
- Tytuł: Wersja aplikacji na tablet
- Opis: Jako użytkownik chcę korzystać z aplikacji na tablecie
- Kryteria akceptacji:
  - Adaptacyjny layout między mobile a desktop
  - Optymalne wykorzystanie przestrzeni ekranu
  - Wszystkie funkcje dostępne
  - Obsługa orientacji poziomej i pionowej

### 5.10 Nawigacja

#### US-030: Nawigacja między widokami
- ID: US-030
- Tytuł: Poruszanie się po aplikacji
- Opis: Jako użytkownik chcę łatwo nawigować między różnymi sekcjami aplikacji
- Kryteria akceptacji:
  - Menu nawigacyjne zawsze widoczne
  - Linki do: Dashboard, Job Board, Add Application
  - Breadcrumbs na podstronach
  - Przycisk "Back" gdzie potrzebny

#### US-031: Dostęp do profilu użytkownika
- ID: US-031
- Tytuł: Menu użytkownika
- Opis: Jako użytkownik chcę mieć dostęp do opcji konta
- Kryteria akceptacji:
  - Dropdown menu z inicjałami, na przykład, John Smith = JS
  - Opcje: Profile, Logout

## 6. Metryki sukcesu

### 6.1 Metryki MVP

#### 6.1.1 Metryki użytkowania
- Liczba zarejestrowanych użytkowników
- Liczba utworzonych aplikacji per user
- Średnia liczba aplikacji na użytkownika
- Procent użytkowników z min. 5 aplikacjami

#### 6.1.2 Metryki funkcjonalności
- Success rate AI parsing:
  - Full success: > 60%
  - Partial success: > 80%
  - Total failure: < 20%

#### 6.1.3 Metryki zaangażowania
- Daily Active Users (DAU)
- Weekly Active Users (WAU)
- User retention:
  - Po 7 dniach: > 40%
  - Po 30 dniach: > 20%
- Średni czas spędzony w aplikacji per sesja