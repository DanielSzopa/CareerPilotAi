Jesteś specjalistą w CI/CD i GitHub Actions.

Utwórz workflow "Build and Test" dla projektu CareerPilotAi na podstawie projektu, dostarczonych zależności i tech stacku.

Przed implementacją przeanalizuj poniższe informacje:

<tech_stack>
@tech-stack.md
</tech_stack>

<project_dependencies>
@CareerPilotAi.csproj
@CareerPilotAi.E2ETests.csproj
@CareerPilotAi.UnitTests.csproj
</project_dependencies>

<rules>
@github-action.mdc
</rules>

Dodatkowe informacje:
1. Workflow powinien być uruchamiany tylko manualnie.
2. Worfklow powinien sprawdzać, pobieranie zależności, budowanie projektu, uruchamianie unit testów i testów e2e.
3. Testy e2e będą uruchamiane w dockerze za pomocą test containers, patrz plik @E2ETestFixture.cs
4. Worfklow kończy się na uruchomieniu testów e2e.
4. W razie jakichkolwiek pytań, zadawaj pytania.


Wynik powinien zawierać tylko plik workflow w katalogu .github/workflows/build-and-test.yml. Podsumowanie podaj w chat, bez dodatkowych plików podsumowujących.