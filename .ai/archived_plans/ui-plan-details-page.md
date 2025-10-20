Jesteś doświadczonym programistą ASP .NET CORE MVC, którego zadaniem jest stworzenie planu refaktoryzacji widoku szczegółów "Job Application Details", według podanych założeń.

Zanim zaczniemy, zapoznaj się z poniższymi informacjami, wykonaj analizę i nakreśl swoje podejście:

<current_implementation>
JobApplicationDetails.cshtml
JobApplicationController.cs
JobApplicationDetailsViewModel.cs
</current_implementation>

<ui_plan_details_page>
@ui-plan.md - zapoznaj się z planem architektury UI dla widoku szczegółów "Job Application Details".
</ui_plan_details_page>

<controllers_and_api_endpoints_specification>
@controllers-final-plans.md
</controllers_and_api_endpoints_specification>

<user_stories>
Weź zapoznaj się z informacjami dotyczącymi funkcjonalności "Wyświetlanie szczegółów aplikacji" z @prd.md
</user_stories>

<tech_stack>
@tech-stack.md
</tech_stack>

<rules>
@backend.mdc
@shared.mdc
@frontend.mdc
</rules>

<additional_information>
Zauważ, że widok szczegółów "Job Application Details" jest już istniejącym widokiem, który trzeba zrefaktoryzować, pod podane wymagania. Jest on wyświatlany z funkcjonalnością TABS, twoim zadaniem jest refaktoryzacja tylko widoku który odpowiada za Job Application Details.
</additional_information>


Po przeprowadzeniu analizy utwórz szczegółowy plan wdrożenia w formacie markdown. Plan powinien zawierać następujące sekcje:

1. Refaktoryzacja controllera.
2. Szczegóły implementacji viewModel.
3. Refaktoryzacja widoku.
4. Kroki implementacji.
5. Pytania jeśli są co do implementacji.

Końcowym wynikiem powinien być dobrze zorganizowany plan wdrożenia w formacie markdown.

Końcowe wyniki powinny składać się wyłącznie z planu wdrożenia w formacie markdown i nie powinny powielać ani powtarzać żadnej pracy wykonanej w sekcji analizy.

Pamiętaj, aby zapisać swój plan wdrożenia jako .ai/details-page-refactoring-plan.md. Upewnij się, że plan jest szczegółowy, przejrzysty i zapewnia kompleksowe wskazówki dla zespołu programistów.