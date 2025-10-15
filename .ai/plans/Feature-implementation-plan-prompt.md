Jesteś doświadczonym programistą ASP .NET CORE, którego zadaniem jest stworzenie szczegółowego planu wdrożenia funkcji na podstawie dostarczonych informacji.
Twój plan poprowadzi zespół programistów w skutecznym i poprawnym wdrożeniu tej funkcjonalności.

Zanim zaczniemy, zapoznaj się z poniższymi informacjami:

1. Controllers and api endpoints specification:

<controllers_and_api_endpoints_specification>
@controllers-final-plans.md
</controllers_and_api_endpoints_specification>

2. User stories to implementation:

<user_stories>
Weź zapoznaj się z informacjami dotyczącymi funkcjonalności "Dodawanie nowej aplikacji" z @prd.md
</user_stories>

3. Tech stack:

<tech_stack>
@tech-stack.md
</tech_stack>

4. Implementation rules:
<rules>
@backend.mdc
@shared.mdc
@frontend.mdc
</rules>

5. Infrastructure layer:
<infrastructure_layer>
@infrastructure_layer.md
</infrastructure_layer>

6. Application layer:
<application_layer>
@application_layer.md
</application_layer>

7. Viewmodels layer:
<viewmodels_layer>
@viewmodels.md
</viewmodels_layer>

8. UI plan:
<ui_plan>
@ui-plan.md
</ui_plan>

9. Job application controller:
<job_application_controller>
@job-application-controller.md
</job_application_controller>

Twoim zadaniem jest stworzenie kompleksowego planu wdrożenia funkcjonalności "Dodawanie nowej aplikacji". Przed dostarczeniem ostatecznego planu użyj znaczników <analysis>, aby przeanalizować informacje i nakreślić swoje podejście. W tej analizie upewnij się, że:

1. Weź pod uwagę, że dodawanie nowej aplikacji jest już istniejącą funkcjonalnością, którą trzeba zrefaktoryzować, pod podane wymagania.
2. Wymień wymagane ViewModels, które będą potrzebne do wdrożenia tej funkcjonalności wraz z ich właściwościami i atrybutami walidacji.
3. Podsumuj kluczowe punkty specyfikacji endpointu w controllerze. Uwzględnij wymagania dotyczące walidacji.
4. Zaplanuj przekazanie danych z controllera do application layer z techniką Command Pattern.
5. Zaplanuj DataModel z konfiguracją entity framework core.
6. Zaplanuj logikę biznesową w application layer.
7. Nakreśl potencjalne scenariusze błędów i odpowiadające im kody stanu.
8. Zaplanuj UI dla tej funkcjonalności. Funkcjonalność posiada wywołanie do API, które zwraca JSON z danymi do wypełnienia formularza, zaplanuj zamokowanie tego endpointu, żeby przetestować tylko główną funkcjonalność, resztę zrobi się później.

Po przeprowadzeniu analizy utwórz szczegółowy plan wdrożenia w formacie markdown. Plan powinien zawierać następujące sekcje:

1. Szczegóły implementacji endpointu w controllerze.
2. Szczegóły implementacji viewModels.
3. Szczegóły implementacji application layer z commmand pattern.
4. Przepływ danych.
5. Implementacja DataModel z konfiguracją entity framework core.
6. Obsługa błędów.
7. Kroki implementacji.

Końcowym wynikiem powinien być dobrze zorganizowany plan wdrożenia w formacie markdown.

Końcowe wyniki powinny składać się wyłącznie z planu wdrożenia w formacie markdown i nie powinny powielać ani powtarzać żadnej pracy wykonanej w sekcji analizy.

Pamiętaj, aby zapisać swój plan wdrożenia jako .ai/view-implementation-plan.md. Upewnij się, że plan jest szczegółowy, przejrzysty i zapewnia kompleksowe wskazówki dla zespołu programistów.