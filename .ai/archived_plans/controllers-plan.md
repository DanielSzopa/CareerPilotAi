<prd>
@prd.md
</prd>

<tech-stack>
@tech-stack.md
</tech-stack>

Jesteś doświadczonym architektem Controllerów i punktów końcowych, którego zadaniem jest stworzenie kompleksowego planu Controllerów i punktów końcowych oraz powiązanych do nich ViewModels. Twój plan będzie oparty na podanym dokumencie wymagań produktu (PRD) i stacku technologicznym podanym powyżej. Uważnie przejrzyj dane wejściowe i wykonaj następujące kroki:

1. Przeanalizuj PRD:
   - Zidentyfikuj kluczowe cechy i funkcjonalności
   - Zwróć uwagę na konkretne wymagania dotyczące operacji na danych (pobieranie, tworzenie, aktualizacja, usuwanie)
   - Zidentyfikuj wymagania logiki biznesowej, które wykraczają poza operacje CRUD
   - Zwróć uwagę na to, że niektóre endpointy mogą przekierowywać do widoku, a niektóre są endpointami API.
   - Zidentyfikuj ViewModels powiązane z Controllerami i punktami końcowymi.

2. Rozważ stack technologiczny:
   - Upewnij się, że plan Controllerów i punktów końcowych jest zgodny z określonymi technologiami.
   - Rozważ, w jaki sposób te technologie mogą wpłynąć na projekt API
   - Zauważ, że projekt jest realizowany w ASP.NET Core MVC z Razor Views, więc musisz stworzyć plan Controllerów i punktów końcowych w tym frameworku.

3. Tworzenie kompleksowego planu interfejsu API:
   - Zdefiniowanie głównych zasobów w oparciu o wymagania PRD
   - Zaprojektowanie punktów końcowych CRUD dla każdego zasobu
   - Zaprojektuj punkty końcowe dla logiki biznesowej opisanej w PRD
   - Uwzględnienie filtrowania i sortowania dla punktów końcowych listy.
   - Zaplanuj odpowiednie użycie metod HTTP
   - Zdefiniowanie struktur ładunku żądania i odpowiedzi
   - Uwzględnienie mechanizmów uwierzytelniania i autoryzacji, jeśli wspomniano o nich w PRD
   - Rozważenie ograniczenia szybkości i innych środków bezpieczeństwa
   - Zidentyfikuj ViewModels powiązane z Controllerami i punktami końcowymi. Wymień wszystkie properties ViewModeli oraz atrybuty walidacji.

Przed dostarczeniem ostatecznego planu, pracuj wewnątrz tagów <api_analysis> w swoim bloku myślenia, aby rozbić swój proces myślowy i upewnić się, że uwzględniłeś wszystkie niezbędne aspekty. W tej sekcji:

2. Wymień kluczowe funkcje logiki biznesowej z PRD. Ponumeruj każdą funkcję i zacytuj odpowiednią część PRD.
3. Zmapuj funkcje z PRD do potencjalnych punktów końcowych API. Dla każdej funkcji rozważ co najmniej dwa możliwe projekty punktów końcowych i wyjaśnij, który z nich wybrałeś i dlaczego.
4. Rozważ i wymień wszelkie wymagania dotyczące bezpieczeństwa i wydajności. Dla każdego wymagania zacytuj część dokumentów wejściowych, która je obsługuje.
5. Wyraźnie mapuj logikę biznesową z PRD na punkty końcowe API.
6. Uwzględnienie warunków walidacji z PRD.
Ta sekcja może być dość długa.
7. Wymień wszystkie ViewModels powiązane z Controllerami i punktami końcowymi. Wymień wszystkie properties ViewModeli oraz atrybuty walidacji.

Ostateczny plan API powinien być sformatowany w markdown i zawierać następujące sekcje:

```markdown
# Controllers and Endpoints Plan

## 1. Zasoby
- Wymień każdy główny zasób

## 2. Punkty końcowe
Dla każdego zasobu podaj:
- Metoda HTTP
- Ścieżka URL
- Krótki opis
- Parametry zapytania (jeśli dotyczy)
- Struktura ładunku żądania JSON (jeśli dotyczy)
- Struktura ładunku odpowiedzi JSON (jeśli dotyczy)
- Kody i komunikaty powodzenia
- Kody i komunikaty błędów
- ViewModels powiązane z Controllerami i punktami końcowymi. Wymień wszystkie properties ViewModeli oraz atrybuty walidacji. (jeśli dotyczy)

## 3. Uwierzytelnianie i autoryzacja
- Opisz wybrany mechanizm uwierzytelniania i szczegóły implementacji

## 4. Walidacja i logika biznesowa
- Lista warunków walidacji dla każdego zasobu
- Opisz, w jaki sposób logika biznesowa jest zaimplementowana w API
```

Upewnij się, że Twój plan jest kompleksowy, dobrze skonstruowany i odnosi się do wszystkich aspektów materiałów wejściowych. Jeśli musisz przyjąć jakieś założenia z powodu niejasnych informacji wejściowych, określ je wyraźnie w swojej analizie.

Końcowy wynik powinien składać się wyłącznie z planu API w formacie markdown w języku angielskim, który zapiszesz w .ai/controllers-plan-v2.md i nie powinien powielać ani powtarzać żadnej pracy wykonanej w bloku myślenia.