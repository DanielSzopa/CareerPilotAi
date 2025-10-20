Jesteś doświadczonym programistą ASP .NET CORE, którego zadaniem jest stworzenie szczegółowego planu wdrożenia funkcji na podstawie dostarczonych informacji.
Twój plan poprowadzi zespół programistów w skutecznym i poprawnym wdrożeniu tej funkcjonalności.

Zanim zaczniemy, zapoznaj się z poniższymi informacjami:

1. Controllers and api endpoints specification:

<controllers_and_api_endpoints_specification>
@controllers-final-plans.md
</controllers_and_api_endpoints_specification>

2. User stories to implementation:

<user_stories>
Weź zapoznaj się z informacjami dotyczącymi funkcjonalności "Parsowanie ogłoszeń przy użyciu AI" z @prd.md
</user_stories>

3. Tech stack:

<tech_stack>
@tech-stack.md
</tech_stack>

4. Implementation rules:
<rules>
@backend.mdc
@shared.mdc
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

8. UI&UX:
<ui>
@ui.md
</ui>

9. Prompt architecture:
<prompt_architecture>
@prompt-architecture.md
</prompt_architecture>

10. Aktualna implementacja:
<actual_implementation>
ParseJobDescription
</actual_implementation>

Twoim zadaniem jest stworzenie kompleksowego planu wdrożenia funkcjonalności "Parsowanie ogłoszeń przy użyciu AI". Przed dostarczeniem ostatecznego planu użyj znaczników <analysis>, aby przeanalizować informacje i nakreślić swoje podejście. W tej analizie upewnij się, że:

1. Weź pod uwagę jak powininen być zaprojektwany nowy prompt według <prompt_architecture>, tak aby był zgodny z architekturą promptów w aplikacji CareerPilotAi.
2. Zaprojektu prompt input model i output model, tak żeby był zgodny z architekturą promptów w aplikacji CareerPilotAi.
3. Pozbądź się zmockowanych danych, które są na tą chwilę zaimplementowane w ParseJobDescription, dostosuj pod prawdziwą implementację.
4. Upewnij się, że obsługa błędów jest poprawna i zgodna z architekturą aplikacji i wymagań PRD.


Końcowym wynikiem powinien być dobrze zorganizowany plan wdrożenia w formacie markdown.

Końcowe wyniki powinny składać się wyłącznie z planu wdrożenia w formacie markdown i nie powinny powielać ani powtarzać żadnej pracy wykonanej w sekcji analizy.

Pamiętaj, aby zapisać swój plan wdrożenia jako .ai/plans/prompt-plan-creation.md. Upewnij się, że plan jest szczegółowy, przejrzysty i zapewnia kompleksowe wskazówki dla zespołu programistów.