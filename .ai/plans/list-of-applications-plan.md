Jesteś doświadczonym architektem oprogramowania, którego zadaniem jest stworzenie szczegółowego planu implementacji funkcjonalności "Przeglądanie listy aplikacji" aka "Job Board" na podstawie dostarczonych informacji.

Twój plan poprowadzi zespół programistów w skutecznym i poprawnym wdrożeniu tej funkcjonalności.
Zanim zaczniemy, zapoznaj się z poniższymi informacjami:

<implementation_business_details>

Zapoznaj się z @prd.md, szczególnie z sekcjami:

- ### 3.3 Job Board - lista aplikacji
- ### 5.5 Filtrowanie i wyszukiwanie
- ### 5.6 Sortowanie, 
- ### 5.9 Responsywność, 
- #### US-008: Przeglądanie listy aplikacji,
- #### US-012: Zmiana statusu aplikacji (quick change)
</implementation_business_details>


<legacy_code_explanation>
@JobApplicationController.cs - zapoznaj się z kodem kontrolera JobApplicationController.cs, który jest odpowiedzialny za funkcjonalność "Przeglądanie listy aplikacji" aka "Job Board". Zrefaktoruj kod pod podane wymagania, spróbuj użyć istniejących endpointów do usuwania i aktualizacji statusu.
</legacy_code_explanation>

<tech_stack>
@tech_stack.md
</tech_stack>

<rules>
@backend.mdc
@shared.mdc
@frontend.mdc
</rules>

<infrastructure_layer>
@infrastructure_layer.md
</infrastructure_layer>

<application_layer>
@application_layer.md
</application_layer>

<viewmodels_layer>
@viewmodels.md
</viewmodels_layer>

<ui_plan>
@ui-plan.md
</ui_plan>

<controllers_and_api_endpoints_specification>
@controllers-final-plans.md
</controllers_and_api_endpoints_specification>

Twoim zadaniem jest stworzenie kompleksowego planu wdrożenia funkcjonalności "Przeglądanie listy aplikacji" aka "Job Board" z funkcjami filtrowania, wyszukiwania oraz sortowania. Przed dostarczeniem ostatecznego planu użyj znaczników <analysis>, aby przeanalizować informacje i nakreślić swoje podejście. W tej analizie upewnij się, że:

1. Rozumiesz podane wymagania z PRD.md
2. Rozumiesz istniejący kod w JobApplicationController.cs
3. Rozumiesz tech stack
4. Rozumiesz implementation rules
5. Rozumiesz infrastructure layer
6. Rozumiesz application layer
7. Rozumiesz viewmodels layer
8. Rozumiesz ui plan
9. Rozumiesz legacy code explanation
10. Lista pytań o ile są co do implementacji lub wymagań biznesowych. Lepiej się upewnić, niż coś pominąć.

Po przeprowadzeniu analizy utwórz szczegółowy plan wdrożenia w formacie markdown. Plan powinien zawierać następujące sekcje:

1. Opis wdrażanej funkcjonalności. Dokładnie opisz co będzie nowo wdrożone i jak będzie działać.
2. Opis UI, jak będzie wyglądać. Dokładnie opisz co będzie zawierało się w karcie, jak użytkownik będzie mógł przeglądać listę aplikacji, jak będzie mógł filtrować, sortować i wyszukiwać. Ma być to szczegółowy opis planu implementacji UI.
3. Opis filtrów. Dokładnie opisz co będzie możliwe do filtrowania, jak będzie wyglądało menu filtrów, jak będzie wyglądało pole wyszukiwania, jak będzie wyglądało pole sortowania. Ma być to szczegółowy opis planu implementacji filtrów.
4. Opis sortowania. Dokładnie opisz co będzie możliwe do sortowania, jak będzie wyglądało menu sortowania, jak będzie wyglądało pole sortowania. Ma być to szczegółowy opis planu implementacji sortowania.
5. Przepływ danych.
6. View Models
7. Kroki implementacji.


Końcowe wyniki powinny składać się wyłącznie z planu wdrożenia w formacie markdown i nie powinny powielać ani powtarzać żadnej pracy wykonanej w sekcji analizy.

Pamiętaj, aby zapisać swój plan wdrożenia jako .ai/list-of-applications-final-plan.md. Upewnij się, że plan jest szczegółowy, przejrzysty i zapewnia kompleksowe wskazówki dla zespołu programistów. Nie implementuj niczego, tylko przygotuj plan implementacji.