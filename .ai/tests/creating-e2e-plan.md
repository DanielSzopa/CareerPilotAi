Stwórz plan implementacji testów e2e dla logowania dla podanych test casów.

<testcases>
@test-cases.md
</testcases>

<rules>
@e2e.mdc
<rules>

<implementation_details>
@AuthController.cs
@IdentityExtensions.cs
@auth-flows.md
@auth-flows-test-ids.md
<implementation_details>

<test_plan>
@test-plan.md
</test_plan>

W pierwszej kolejności przeprowadź analizę:

1. Przeanalizuj jakie test cases należy zaimplementować z <testcases>.
2. Przeanalizuj, żeby wiedzieć jakie reguły należy zastosować do implementacji testów z <rules>.
3. Przeanalizuj żeby zobaczyć jak należy pisać testy e2e<test_plan>
4. Przeanalizuj które test casy mogą być zaimplementowane w jednym teście, dzięki temu będziesz mógł zmniejszyć liczbę testów. Wykorzystaj technikę parametryzacji.
5. Zastanów się jaki PageObject stworzyć.
6. Zastnaów się czy masz jakies pytania do mnie do wyjaśnieniam jeżeli masz to zapytaj, przed stworzeniem planu.

Stwórz plan implementacji testów e2e dla logowania dla podanych test casów i zapisz go w pliku e2e-test-login-plan.md. Pamiętaj, że ten plan ma służyć programiście do implementacji testów e2e. Nie implementuj nic, bez mojej zgody na implementację.

output zapisz w pliku markdown e2e-test-login-plan.