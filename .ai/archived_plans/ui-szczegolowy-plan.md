Jesteś wykwalifikowanym architektem frontend, którego zadaniem jest stworzenie kompleksowej architektury interfejsu użytkownika w oparciu o dokument wymagań produktu (PRD), plan API i notatki z sesji planowania. Twoim celem jest zaprojektowanie struktury interfejsu użytkownika, która skutecznie spełnia wymagania produktu, jest zgodna z możliwościami API i zawiera spostrzeżenia z sesji planowania.

Najpierw dokładnie przejrzyj następujące dokumenty:

Dokument wymagań produktu (PRD):
<prd>
@prd.md
</prd>

Plan API:
<api_plan>
@controllers-plan-v2.md
</api_plan>

Session Notes:
<session_notes>
<conversation_summary>
<decisions>
Przyjęto architekturę UI opartą na stałym, górnym pasku nawigacyjnym (navbar) z linkami do Dashboard, Job Board, przyciskiem "Add New Application" oraz menu profilu użytkownika.
Filtrowanie na Job Boardzie będzie powodować przeładowanie całej strony, a stan filtrów będzie zarządzany przez parametry w URL (query parameters). Wskaźniki ładowania dla tej akcji nie będą implementowane w MVP.
Proces parsowania AI będzie obsługiwany asynchronicznie (AJAX) z wyraźnym sygnalizowaniem statusu operacji (ładowanie, sukces, błąd) i dynamicznym wypełnianiem formularza.
"Szybkie akcje" na kartach aplikacji będą zaimplementowane następująco: zmiana statusu przez AJAX, usuwanie przez modal potwierdzający i standardowy POST, edycja jako link.
Wszystkie dane dla Dashboardu będą ładowane po stronie serwera i renderowane razem z widokiem w ramach MVP. Wykresy będą posiadały interaktywne tooltipy z dodatkowymi informacjami.
Zostanie wdrożona globalna strategia obsługi błędów: dedykowana strona błędu dla krytycznych błędów nawigacji oraz powiadomienia "toast" lub komunikaty inline dla błędów z operacji AJAX.
Sekcja filtrów będzie responsywna: boczny panel (sidebar) na desktopie, wysuwany panel (off-canvas) na urządzeniach mobilnych.
Walidacja formularzy po stronie klienta będzie realizowana za pomocą wbudowanych mechanizmów ASP.NET Core "unobtrusive validation".
Widok szczegółów aplikacji będzie miał układ dwukolumnowy na desktopie i jednokolumnowy na mobile. Formatowanie tekstu w opisie stanowiska zostanie zachowane przy użyciu tagu <pre>.
Formularze tworzenia i edycji aplikacji będą współdzieliły kod poprzez wykorzystanie częściowego widoku Razor (_JobApplicationForm.cshtml).
Zostaną zaimplementowane dedykowane widoki dla "stanów pustych" (np. brak wyników wyszukiwania, pusty dashboard dla nowego użytkownika).
Zarządzanie umiejętnościami w formularzu będzie odbywać się poprzez dynamiczne dodawanie tagów z możliwością wyboru poziomu i usunięcia.
Zostanie przyjęta zdefiniowana paleta kolorów oparta o Bootstrap do wizualnego rozróżniania statusów aplikacji.
Powiadomienia "toast" będą pojawiać się w prawym górnym rogu i znikać automatycznie po 5 sekundach.
Podstawowe standardy dostępności (WCAG 2.1 AA) zostaną wdrożone w MVP.
</decisions>
<matched_recommendations>
Nawigacja: Zastosowanie stałego, górnego paska nawigacyjnego (navbar) z linkami do "Dashboard", "Job Board", przyciskiem CTA "Add New Application" oraz menu profilu użytkownika.
Filtrowanie: Pełne przeładowanie strony po zastosowaniu filtrów, z ich stanem odzwierciedlonym w parametrach URL, co jest zgodne ze strukturą endpointu GET /JobApplication/Index.
Parsowanie AI: Użycie JavaScript (AJAX) do wywołania endpointu /JobApplication/ParseJobDescription, z blokowaniem UI i wyświetlaniem wskaźnika ładowania podczas operacji, a następnie dynamicznym wypełnieniem formularza i wyświetleniem komunikatu o rezultacie.
Szybkie akcje: Zmiana statusu poprzez asynchroniczne wywołanie /JobApplication/UpdateStatus, usuwanie przez modal potwierdzający i POST do /JobApplication/Delete/{id}, a edycja jako standardowy link.
Formularze: Stworzenie jednej, współdzielonej częściowego widoku Razor (_JobApplicationForm.cshtml) dla formularzy tworzenia i edycji w celu maksymalizacji reużywalności kodu.
Responsywność: Implementacja panelu filtrów jako stałego sidebara na desktopie i wysuwanego panelu "off-canvas" na urządzeniach mobilnych.
Walidacja: Wykorzystanie wbudowanych w ASP.NET Core mechanizmów "unobtrusive validation" do walidacji po stronie klienta, co zapewnia spójność z regułami na backendzie.
Stany puste: Zaprojektowanie dedykowanych komunikatów i widoków dla sytuacji braku danych (np. "No applications found matching your criteria" na Job Boardzie).
Zarządzanie umiejętnościami: Implementacja interfejsu do dynamicznego dodawania umiejętności jako "tagi" z dropdownem do wyboru poziomu i opcją usunięcia.
Dostępność: Skupienie się na podstawach WCAG 2.1 AA: dostępność z klawiatury, atrybuty alt dla obrazów, semantyczny HTML, odpowiedni kontrast kolorów i poprawne etykietowanie formularzy.
</matched_recommendations>
<ui_architecture_planning_summary>
a. Główne wymagania dotyczące architektury UI
Architektura UI dla MVP będzie oparta na serwerowo renderowanych widokach (ASP.NET Core Razor Views) z selektywnym użyciem JavaScript (AJAX) do poprawy interaktywności. Główna nawigacja zostanie zaimplementowana jako stały, górny pasek (navbar) zapewniający dostęp do kluczowych sekcji: Dashboard, Job Board oraz akcji dodawania nowej aplikacji. Architektura będzie promować reużywalność kodu, głównie poprzez zastosowanie częściowego widoku dla formularzy. Interfejs będzie wizualnie komunikował stan aplikacji za pomocą zdefiniowanej palety kolorów i dostarczał informację zwrotną poprzez powiadomienia "toast".
b. Kluczowe widoki, ekrany i przepływy użytkownika
Dashboard: Punkt startowy dla zalogowanego użytkownika, prezentujący kluczowe metryki, wykres kołowy statusów, wykres słupkowy aplikacji w czasie oraz listę ostatnio dodanych aplikacji. Dane będą ładowane po stronie serwera.
Job Board (Lista Aplikacji): Centralny widok do przeglądania wszystkich aplikacji w formie kart. Umożliwi filtrowanie (przez przeładowanie strony), sortowanie oraz wykonywanie szybkich akcji (zmiana statusu, edycja, usunięcie).
Tworzenie/Edycja Aplikacji: Widok formularza. Kluczowy przepływ to opcjonalne użycie parsowania AI: użytkownik wkleja tekst ogłoszenia, UI wysyła go asynchronicznie do API, a następnie formularz jest dynamicznie wypełniany. Użytkownik weryfikuje dane i zapisuje aplikację.
Szczegóły Aplikacji: Widok prezentujący wszystkie informacje o pojedynczej aplikacji, z zachowaniem formatowania opisu stanowiska i opcjami do zarządzania nią.
c. Strategia integracji z API i zarządzania stanem
Zarządzanie stanem: Stan aplikacji będzie głównie zarządzany po stronie serwera. Stan filtrów na Job Boardzie będzie utrwalany w parametrach URL, co zapewni możliwość linkowania do przefiltrowanych widoków.
Integracja z API:
Synchroniczna: Większość nawigacji i operacji (np. filtrowanie, zapis formularza, usuwanie) będzie powodować pełne przeładowanie strony i renderowanie nowego widoku przez serwer.
Asynchroniczna (AJAX): Zostanie użyta w dwóch kluczowych miejscach dla poprawy UX: 1) do parsowania ogłoszenia za pomocą AI bez przeładowywania strony tworzenia aplikacji; 2) do szybkiej zmiany statusu aplikacji bezpośrednio z widoku listy.
Walidacja: Będzie dwuetapowa, z walidacją po stronie klienta (unobtrusive validation) zapewniającą natychmiastowy feedback i walidacją po stronie serwera jako ostatecznym zabezpieczeniem.
d. Kwestie dotyczące responsywności, dostępności i bezpieczeństwa
Responsywność: Aplikacja zostanie zbudowana w podejściu "mobile-first". Kluczowe elementy adaptacyjne to panel filtrów (sidebar na desktopie, off-canvas na mobile) oraz układ dwukolumnowy przechodzący w jednokolumnowy na mniejszych ekranach (np. w szczegółach aplikacji).
Dostępność: MVP będzie spełniać podstawowe kryteria WCAG 2.1 na poziomie AA, w tym nawigację klawiaturą, odpowiedni kontrast, semantyczny HTML i poprawne etykietowanie formularzy.
Bezpieczeństwo: Aspekty bezpieczeństwa, takie jak uwierzytelnianie, autoryzacja i ochrona przed atakami (CSRF, XSS), są głównie odpowiedzialnością backendu (ASP.NET Core Identity). UI będzie integrować się z tymi mechanizmami (np. przez przesyłanie tokenów anty-CSRF w formularzach).
</ui_architecture_planning_summary>
<unresolved_issues>
Brak zidentyfikowanych nierozwiązanych kwestii dla zakresu MVP. Wszystkie przedstawione rekomendacje zostały zaakceptowane lub podjęto w ich sprawie klarowną decyzję.
</unresolved_issues>
</conversation_summary>
</session_notes>

Twoim zadaniem jest stworzenie szczegółowej architektury interfejsu użytkownika, która obejmuje niezbędne widoki, mapowanie podróży użytkownika, strukturę nawigacji i kluczowe elementy dla każdego widoku. Projekt powinien uwzględniać doświadczenie użytkownika, dostępność i bezpieczeństwo.

Wykonaj następujące kroki, aby ukończyć zadanie:

1. Dokładnie przeanalizuj PRD, plan API i notatki z sesji.
2. Wyodrębnij i wypisz kluczowe wymagania z PRD.
3. Zidentyfikuj i wymień główne punkty końcowe API i ich cele.
4. Utworzenie listy wszystkich niezbędnych widoków na podstawie PRD, planu API i notatek z sesji.
5. Określenie głównego celu i kluczowych informacji dla każdego widoku.
6. Zaplanuj podróż użytkownika między widokami, w tym podział krok po kroku dla głównego przypadku użycia.
7. Zaprojektuj strukturę nawigacji.
8. Zaproponuj kluczowe elementy interfejsu użytkownika dla każdego widoku, biorąc pod uwagę UX, dostępność i bezpieczeństwo.
9. Rozważ potencjalne przypadki brzegowe lub stany błędów.
10. Upewnij się, że architektura interfejsu użytkownika jest zgodna z planem API.
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

## 3. Mapa podróży użytkownika

[Opisz przepływ między widokami i kluczowymi interakcjami użytkownika]

## 4. Układ i struktura nawigacji

[Wyjaśnij, w jaki sposób użytkownicy będą poruszać się między widokami]

## 5. Kluczowe komponenty

[Wymień i krótko opisz kluczowe komponenty, które będą używane w wielu widokach].
```

Skup się wyłącznie na architekturze interfejsu użytkownika, podróży użytkownika, nawigacji i kluczowych elementach dla każdego widoku. Nie uwzględniaj szczegółów implementacji, konkretnego projektu wizualnego ani przykładów kodu, chyba że są one kluczowe dla zrozumienia architektury.

Końcowy rezultat powinien składać się wyłącznie z architektury UI w formacie Markdown w języku polskim, którą zapiszesz w pliku .ai/ui-plan.md. Nie powielaj ani nie powtarzaj żadnej pracy wykonanej w bloku myślenia.