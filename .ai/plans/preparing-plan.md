Jesteś doświadczonym architektem oprogramowania, którego zadaniem jest stworzenie szczegółowego planu wdrożenia nowej funkcjonalności. Twój plan poprowadzi zespół programistów w skutecznym i poprawnym wdrożeniu tej funkcjonalności.

<functionality>
US-031: Dostęp do profilu użytkownika z @prd.md
</functionality>

<prd>
@prd.md
</prd>

Zanim zaczniemy, zapoznaj się z poniższymi informacjami:

1. Layers implementation rules
<layers>
@infrastructure_layer.md
@viewmodels.md
</layers>

2. Tech stack:
<tech_stack>
@tech-stack.md
</tech_stack>

3. Implementation rules:
<implementation_rules>
@backend.mdc
@shared.mdc
@frontend.mdc
</implementation_rules>

Twoim zadaniem jest stworzenie kompleksowego planu wdrożenia nowej funkcjonalności. Przed dostarczeniem ostatecznego planu użyj znaczników <analysis>, aby przeanalizować informacje i nakreślić swoje podejście. W tej analizie upewnij się, że:

1. Utwórz nowy DataModel "TimeZones" w Persitance Layer. Będzie to tabela zawierająca listę dostępnych stref czasowych. Dodaj 5 najpopularniejszych stref czasowych. Musi być jedna dla Polski.
2. Utwórz nowy DataModel "UserSettings" w Persitance Layer. Będzie narazie tylko pole "TimeZoneId" typu string. Musi mieć relacje do "TimeZones" i "IdentityUser". Każdy użytkownik powinien mieć swoje ustawienia.
3. Utwórz nowy Service "TimeZoneService" w Application Layer. Będzie to service, który będzie używany do pobierania stref czasowych.
4. Do controllera @AuthController dodaj nową akcję "GetUserSettings" w Application Layer. Będzie to akcja, która będzie używana do pobierania ustawień użytkownika. Powinno przekierowywać do nowego widoku UserSettings.cshtml. Będzie to nowy widok, który wyświetli ustawienia użytkownika. Na razie powinien wyświetlać tylko strefę czasową użytkownika z możliwością zmiany (dropdown listą stref czasowych) oraz aktualny email (nie do zmiany, poprostu będzie to informacja jaki email jest). Powinna być opcja Save, żeby zapisać zmiany.
5. Utwórz nowy endpoint w AuthController do zmiany UserSettings. Będzie to POST endpoint, który będzie używany do zmiany ustawień użytkownika. Powinien przyjmować ViewModel "UserSettingsViewModel" w body i zapisywać zmiany w bazie danych.

Po przeprowadzeniu analizy utwórz szczegółowy plan wdrożenia w formacie markdown. Plan powinien zawierać następujące sekcje:

1. Przegląd funkcjonalności
2. Przegląd endpointów w controllerze
3. Przepływ danych
4. Zaimplementowane widoki
5. Obsługa błędów
6. Kroki implementacji

Końcowym wynikiem powinien być dobrze zorganizowany plan wdrożenia w formacie markdown.

Końcowe wyniki powinny składać się wyłącznie z planu wdrożenia w formacie markdown i nie powinny powielać ani powtarzać żadnej pracy wykonanej w sekcji analizy.

Pamiętaj, aby zapisać swój plan wdrożenia jako .ai/feature-implementation-plan.md. Upewnij się, że plan jest szczegółowy, przejrzysty i zapewnia kompleksowe wskazówki dla zespołu programistów.