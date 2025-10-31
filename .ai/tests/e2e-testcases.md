# E2E Test Cases - Authentication Flows (CareerPilotAI)

> **Uwaga:** Wszystkie testy zakładają, że feature flag `ConfirmRegistration` jest wyłączony (środowisko E2E), co oznacza automatyczne logowanie po rejestracji bez weryfikacji email.

---

## 1. Registration Flow

### 1.1 Registration - Positive Tests

#### Test Case 1.1.1: Successful Registration with Immediate Sign-In
**Priorytet:** High  
**Cel:** Weryfikacja poprawnej rejestracji nowego użytkownika z automatycznym logowaniem

**Kroki:**
1. Przejdź do strony `/auth/register`
2. Wprowadź poprawny email: `newuser@example.com`
3. Wprowadź hasło spełniające wymagania: `Test1234!@#`
4. Wprowadź identyczne hasło w polu potwierdzenia
5. Kliknij przycisk "Register"

**Oczekiwany rezultat:**
- Użytkownik zostaje automatycznie zalogowany (brak weryfikacji email)
- Przekierowanie do `/job-applications`
- Menu użytkownika jest widoczne (potwierdzenie zalogowania)
- Możliwość dostępu do chronionych zasobów (np. tworzenie aplikacji)

---

#### Test Case 1.1.2: Registration with Maximum Length Email (254 characters)
**Priorytet:** Medium  
**Cel:** Weryfikacja rejestracji z emailem o maksymalnej dozwolonej długości

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email o długości 254 znaków (np. `a` * 240 + `@example.com`)
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Rejestracja przebiegła pomyślnie
- Użytkownik zalogowany automatycznie
- Przekierowanie do `/job-applications`
- Menu użytkownika widoczne

---

#### Test Case 1.1.3: Registration with Special Characters in Email
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi specjalnych znaków RFC 5322 w adresie email

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email ze specjalnymi znakami: `user+test123@example.com`
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Rejestracja pomyślna
- Użytkownik automatycznie zalogowany
- Przekierowanie do `/job-applications`
- Menu użytkownika widoczne

---

#### Test Case 1.1.4: Registration with Minimum Password Requirements
**Priorytet:** High  
**Cel:** Weryfikacja akceptacji hasła spełniającego minimalne wymagania

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `minpass@example.com`
3. Wprowadź hasło o minimalnej długości (8 znaków) z wymaganymi typami znaków: `Abcd123!`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Rejestracja pomyślna
- Użytkownik zalogowany
- Przekierowanie do `/job-applications`
- Menu użytkownika widoczne

---

### 1.2 Registration - Negative Tests

#### Test Case 1.2.1: Registration with Already Registered Email
**Priorytet:** High  
**Cel:** Weryfikacja obsługi próby rejestracji z istniejącym emailem

**Prerekwizyty:** Użytkownik `existing@example.com` już istnieje w systemie (zarejestrowany wcześniej w tym samym teście lub setup)

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź istniejący email: `existing@example.com`
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Pozostanie na stronie `/auth/register`
- Wyświetlony błąd: "An account with this email already exists. Please log in or reset your password."
- Menu użytkownika nie jest widoczne (użytkownik nie został zalogowany)

---

#### Test Case 1.2.2: Registration with Invalid Email Format
**Priorytet:** High  
**Cel:** Weryfikacja walidacji nieprawidłowego formatu email

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź nieprawidłowy email: `notanemail` (bez @)
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Pozostanie na stronie `/auth/register`
- Wyświetlony błąd walidacji email (client-side lub server-side)
- Formularz nie został wysłany lub zwrócony z błędem
- Menu użytkownika nie jest widoczne

---

#### Test Case 1.2.3: Registration with Multiple @ Symbols in Email
**Priorytet:** Medium  
**Cel:** Weryfikacja odrzucenia nieprawidłowego formatu email

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `user@@example.com`
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji email wyświetlony na stronie
- Rejestracja nieudana (pozostanie na `/auth/register`)

---

#### Test Case 1.2.4: Registration with Email Without Domain Extension
**Priorytet:** Medium  
**Cel:** Weryfikacja wymagania domeny TLD

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `user@example`
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji email wyświetlony na stronie
- Rejestracja nieudana

---

#### Test Case 1.2.5: Registration with Email Exceeding 254 Characters
**Priorytet:** Medium  
**Cel:** Weryfikacja limitu długości email

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email o długości 255+ znaków
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji: email zbyt długi
- Rejestracja nieudana

---

#### Test Case 1.2.6: Registration with Password Too Short
**Priorytet:** High  
**Cel:** Weryfikacja minimalnej długości hasła (8 znaków)

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `shortpass@example.com`
3. Wprowadź hasło: `Test1!` (7 znaków)
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony na stronie: hasło zbyt krótkie
- Komunikat: hasło musi mieć minimum 8 znaków
- Rejestracja nieudana

---

#### Test Case 1.2.7: Registration with Password Missing Uppercase
**Priorytet:** High  
**Cel:** Weryfikacja wymagania wielkiej litery w haśle

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `noupper@example.com`
3. Wprowadź hasło: `test1234!` (brak wielkiej litery)
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony na stronie: hasło musi zawierać wielką literę
- Rejestracja nieudana

---

#### Test Case 1.2.8: Registration with Password Missing Lowercase
**Priorytet:** High  
**Cel:** Weryfikacja wymagania małej litery w haśle

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `nolower@example.com`
3. Wprowadź hasło: `TEST1234!` (brak małej litery)
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony na stronie: hasło musi zawierać małą literę
- Rejestracja nieudana

---

#### Test Case 1.2.9: Registration with Password Missing Digit
**Priorytet:** High  
**Cel:** Weryfikacja wymagania cyfry w haśle

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `nodigit@example.com`
3. Wprowadź hasło: `TestTest!` (brak cyfry)
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony na stronie: hasło musi zawierać cyfrę
- Rejestracja nieudana

---

#### Test Case 1.2.10: Registration with Password Missing Special Character
**Priorytet:** High  
**Cel:** Weryfikacja wymagania znaku specjalnego w haśle

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `nospecial@example.com`
3. Wprowadź hasło: `Test1234` (brak znaku specjalnego)
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony na stronie: hasło musi zawierać znak specjalny
- Rejestracja nieudana

---

#### Test Case 1.2.11: Registration with Mismatched Passwords
**Priorytet:** High  
**Cel:** Weryfikacja zgodności hasła i potwierdzenia

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `mismatch@example.com`
3. Wprowadź hasło: `Test1234!@#`
4. Wprowadź inne hasło w potwierdzeniu: `Different123!`
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony na stronie: hasła nie są identyczne
- Rejestracja nieudana

---

#### Test Case 1.2.12: Registration with Empty Email Field
**Priorytet:** High  
**Cel:** Weryfikacja wymagalności pola email

**Kroki:**
1. Przejdź do `/auth/register`
2. Zostaw pole email puste
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: email jest wymagany
- Formularz nie zostaje wysłany lub zwrócony z błędem
- Rejestracja nieudana

---

#### Test Case 1.2.13: Registration with Empty Password Field
**Priorytet:** High  
**Cel:** Weryfikacja wymagalności pola hasła

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `emptypass@example.com`
3. Zostaw pola hasła puste
4. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: hasło jest wymagane
- Rejestracja nieudana

---

#### Test Case 1.2.14: Registration with Email Containing Spaces
**Priorytet:** Medium  
**Cel:** Weryfikacja odrzucenia emaila ze spacjami

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `user name@example.com` (spacja w nazwie)
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Błąd walidacji email wyświetlony na stronie
- Rejestracja nieudana

---

### 1.3 Registration - Edge Cases

#### Test Case 1.3.1: Registration with Email in Mixed Case
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi wielkości liter w emailu

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `MixedCase@Example.COM`
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Rejestracja pomyślna
- Użytkownik zalogowany automatycznie
- Przekierowanie do `/job-applications`

---

#### Test Case 1.3.2: Registration with Leading/Trailing Spaces in Email
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi białych znaków w emailu

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: ` spaceemail@example.com ` (spacje na początku i końcu)
3. Wprowadź poprawne hasło: `Test1234!@#`
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Email zostaje automatycznie oczyszczony z białych znaków (trim) i rejestracja pomyślna ALBO
- Błąd walidacji jeśli trim nie jest zastosowany

---

#### Test Case 1.3.3: Registration and Verify User Can Access Protected Resources
**Priorytet:** High  
**Cel:** Weryfikacja pełnego przepływu po rejestracji

**Kroki:**
1. Przejdź do `/auth/register`
2. Zarejestruj nowego użytkownika z poprawnymi danymi
3. Po przekierowaniu sprawdź URL (powinien być `/job-applications`)
4. Zweryfikuj widoczność menu użytkownika
5. Spróbuj przejść do `/job-applications/entry-job-details` (lub innej chronionej strony)

**Oczekiwany rezultat:**
- Przekierowanie do `/job-applications`
- Menu użytkownika widoczne
- Chronione strony są dostępne (brak przekierowania do logowania)
- Użytkownik może wykonywać akcje wymagające autoryzacji

---

#### Test Case 1.3.4: Registration with Very Long Password (100+ characters)
**Priorytet:** Low  
**Cel:** Weryfikacja obsługi bardzo długich haseł

**Kroki:**
1. Przejdź do `/auth/register`
2. Wprowadź email: `longpass@example.com`
3. Wprowadź bardzo długie hasło (100+ znaków) spełniające wymagania
4. Potwierdź hasło
5. Kliknij "Register"

**Oczekiwany rezultat:**
- Rejestracja pomyślna (jeśli brak górnego limitu) ALBO
- Błąd walidacji z informacją o maksymalnej długości

---

#### Test Case 1.3.6: Verify New User Can Login After Registration
**Priorytet:** High  
**Cel:** Weryfikacja że nowo utworzone konto jest w pełni funkcjonalne

**Kroki:**
1. Zarejestruj nowego użytkownika `logintest@example.com` / `Test1234!@#`
2. Wyloguj się
3. Przejdź do `/auth/login`
4. Zaloguj się używając tych samych danych
5. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie pomyślne
- Przekierowanie do `/job-applications`
- Sesja aktywna, menu użytkownika widoczne

---

## 2. Login Flow

### 2.1 Login - Positive Tests

#### Test Case 2.1.1: Successful Login with Valid Credentials
**Priorytet:** High  
**Cel:** Weryfikacja poprawnego logowania użytkownika

**Prerekwizyty:** Użytkownik `testuser@example.com` / `Test1234!@#` istnieje (utworzony w setup)

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email: `testuser@example.com`
3. Wprowadź hasło: `Test1234!@#`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie pomyślne
- Przekierowanie do `/job-applications`
- Menu użytkownika widoczne (np. nazwa użytkownika, przycisk Logout)
- Możliwość dostępu do chronionych zasobów

---

#### Test Case 2.1.2: Login with "Remember Me" Enabled
**Priorytet:** Medium  
**Cel:** Weryfikacja funkcji "Zapamiętaj mnie"

**Prerekwizyty:** Zarejestrowany użytkownik

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź poprawne dane logowania
3. Zaznacz opcję "Remember Me"
4. Kliknij "Login"
5. Zamknij przeglądarkę (lub zakończ sesję)
6. Otwórz przeglądarkę ponownie
7. Przejdź do `/job-applications`

**Oczekiwany rezultat:**
- Po ponownym otwarciu przeglądarki użytkownik nadal jest zalogowany
- Nie ma potrzeby ponownego logowania
- Dostęp do chronionych stron bez przekierowania do logowania

---

#### Test Case 2.1.3: Login with "Remember Me" Disabled
**Priorytet:** Medium  
**Cel:** Weryfikacja zachowania bez "Zapamiętaj mnie"

**Prerekwizyty:** Zarejestrowany użytkownik

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź poprawne dane logowania
3. NIE zaznaczaj "Remember Me"
4. Kliknij "Login"
5. Zamknij przeglądarkę
6. Otwórz przeglądarkę ponownie
7. Spróbuj przejść do `/job-applications`

**Oczekiwany rezultat:**
- Po ponownym otwarciu przeglądarki użytkownik NIE jest zalogowany
- Przekierowanie do `/auth/login`
- Konieczność ponownego logowania

---

#### Test Case 2.1.4: Login with ReturnUrl Parameter
**Priorytet:** Medium  
**Cel:** Weryfikacja przekierowania do wcześniej żądanej strony

**Prerekwizyty:** Zarejestrowany użytkownik

**Kroki:**
1. Będąc niezalogowanym, spróbuj wejść na `/job-applications/entry-job-details`
2. Zostaniesz przekierowany do `/auth/login?returnUrl=...`
3. Zweryfikuj obecność parametru `returnUrl` w URL
4. Wprowadź poprawne dane logowania
5. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie pomyślne
- Przekierowanie do pierwotnie żądanej strony (`/job-applications/entry-job-details`)
- Nie przekierowuje do domyślnego `/job-applications`

---

#### Test Case 2.1.5: Login with Email in Different Case
**Priorytet:** Medium  
**Cel:** Weryfikacja case-insensitive logowania emailem

**Prerekwizyty:** Użytkownik zarejestrowany jako `testuser@example.com`

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email: `TESTUSER@EXAMPLE.COM` (wszystkie wielkie litery)
3. Wprowadź poprawne hasło
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie pomyślne
- Email jest rozpoznany bez względu na wielkość liter
- Przekierowanie do `/job-applications`

---

### 2.2 Login - Negative Tests

#### Test Case 2.2.1: Login with Non-Existent User
**Priorytet:** High  
**Cel:** Weryfikacja obsługi próby logowania nieistniejącego użytkownika

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email: `nonexistent@example.com`
3. Wprowadź hasło: `Test1234!@#`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Pozostanie na stronie `/auth/login`
- Komunikat błędu: "Login failed. Please check your email and password."
- Generyczny komunikat (bez ujawniania czy user istnieje - security best practice)
- Menu użytkownika nie jest widoczne

---

#### Test Case 2.2.2: Login with Correct Email but Wrong Password
**Priorytet:** High  
**Cel:** Weryfikacja odrzucenia nieprawidłowego hasła

**Prerekwizyty:** Użytkownik `testuser@example.com` istnieje z hasłem `Test1234!@#`

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email: `testuser@example.com`
3. Wprowadź nieprawidłowe hasło: `WrongPassword123!`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Pozostanie na stronie `/auth/login`
- Komunikat błędu: "Login failed. Please check your email and password."
- Menu użytkownika nie jest widoczne
- Brak informacji czy email jest poprawny (security)

---

#### Test Case 2.2.3: Login with Empty Email Field
**Priorytet:** High  
**Cel:** Weryfikacja walidacji wymaganego pola email

**Kroki:**
1. Przejdź do `/auth/login`
2. Zostaw pole email puste
3. Wprowadź hasło: `Test1234!@#`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: email jest wymagany
- Formularz nie zostaje wysłany lub zwrócony z błędem
- Pozostanie na stronie logowania

---

#### Test Case 2.2.4: Login with Empty Password Field
**Priorytet:** High  
**Cel:** Weryfikacja walidacji wymaganego pola hasła

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email: `testuser@example.com`
3. Zostaw pole hasła puste
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: hasło jest wymagane
- Formularz nie zostaje wysłany
- Pozostanie na stronie logowania

---

#### Test Case 2.2.5: Login with All Empty Fields
**Priorytet:** Medium  
**Cel:** Weryfikacja walidacji wszystkich wymaganych pól

**Kroki:**
1. Przejdź do `/auth/login`
2. Zostaw wszystkie pola puste
3. Kliknij "Login"

**Oczekiwany rezultat:**
- Błędy walidacji dla obu pól wyświetlone
- Formularz nie zostaje wysłany
- Komunikaty o wymaganych polach

---

#### Test Case 2.2.6: Login with Invalid Email Format
**Priorytet:** Medium  
**Cel:** Weryfikacja walidacji formatu email przy logowaniu

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź nieprawidłowy email: `notanemail`
3. Wprowadź hasło: `Test1234!@#`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Błąd walidacji formatu email ALBO
- Generyczny komunikat o błędnym logowaniu
- Pozostanie na stronie logowania

---

#### Test Case 2.2.7: Multiple Failed Login Attempts
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi wielokrotnych nieudanych prób logowania

**Prerekwizyty:** Użytkownik `testuser@example.com` istnieje z hasłem `Test1234!@#`

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź poprawny email: `testuser@example.com`
3. Wprowadź nieprawidłowe hasło: `WrongPass123!`
4. Kliknij "Login"
5. Powtórz kroki 3-4 jeszcze 4 razy (łącznie 5 prób)
6. Sprawdź czy pojawił się komunikat o blokowaniu/CAPTCHA
7. Spróbuj zalogować się z poprawnym hasłem

**Oczekiwany rezultat:**
- Każda próba zwraca komunikat o błędnym logowaniu
- Po przekroczeniu limitu możliwe scenariusze:
  - Komunikat o zbyt wielu próbach
  - Tymczasowa blokada (użytkownik nie może się zalogować nawet z poprawnym hasłem)
  - Wyświetlenie CAPTCHA (jeśli zaimplementowana)

---

### 2.3 Login - Edge Cases

#### Test Case 2.3.1: Login and Verify Session Persistence Across Pages
**Priorytet:** Medium  
**Cel:** Weryfikacja trwałości sesji w ramach jednej sesji przeglądarki

**Prerekwizyty:** Zarejestrowany użytkownik

**Kroki:**
1. Zaloguj się poprawnie
2. Nawiguj do różnych stron aplikacji (`/job-applications`, `/auth/user-settings`, itp.)
3. Odśwież stronę kilkukrotnie (F5)
4. Sprawdź czy menu użytkownika jest nadal widoczne
5. Sprawdź czy strony chronione są dostępne

**Oczekiwany rezultat:**
- Sesja pozostaje aktywna podczas nawigacji
- Menu użytkownika widoczne na każdej stronie
- Brak potrzeby ponownego logowania
- Brak przekierowań do `/auth/login`

---

#### Test Case 2.3.2: Login from Already Logged-In State
**Priorytet:** Low  
**Cel:** Weryfikacja zachowania przy próbie logowania już zalogowanego użytkownika

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Będąc zalogowanym, przejdź bezpośrednio do `/auth/login`
2. Sprawdź co się wyświetla
3. Jeśli formularz jest widoczny, spróbuj zalogować się ponownie

**Oczekiwany rezultat:**
- Możliwe scenariusze (w zależności od implementacji):
  - Przekierowanie do `/job-applications` (już zalogowany)
  - Wyświetlenie formularza logowania (możliwość zmiany konta)
- Brak błędów
- Zachowanie jest spójne

---

#### Test Case 2.3.3: Login with Special Characters in Password
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi znaków specjalnych w haśle

**Prerekwizyty:** 
1. Zarejestruj użytkownika z hasłem zawierającym znaki specjalne: `P@$$w0rd!#%&*`
2. Wyloguj się

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email użytkownika
3. Wprowadź hasło ze znakami specjalnymi: `P@$$w0rd!#%&*`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie pomyślne
- Znaki specjalne prawidłowo obsłużone
- Przekierowanie do `/job-applications`

---

#### Test Case 2.3.4: Login Navigation - "Forgot Password" Link
**Priorytet:** Low  
**Cel:** Weryfikacja nawigacji do resetowania hasła

**Kroki:**
1. Przejdź do `/auth/login`
2. Kliknij link "Forgot password?"
3. Zweryfikuj przekierowanie

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/forgot-password`
- Formularz resetowania hasła widoczny

---

#### Test Case 2.3.5: Login Navigation - "Sign Up" Link
**Priorytet:** Low  
**Cel:** Weryfikacja nawigacji do rejestracji

**Kroki:**
1. Przejdź do `/auth/login`
2. Kliknij link "Sign up"
3. Zweryfikuj przekierowanie

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/register`
- Formularz rejestracji widoczny

---

#### Test Case 2.3.6: Login with SQL Injection Attempt in Email
**Priorytet:** High (Security)  
**Cel:** Weryfikacja zabezpieczeń przed SQL Injection

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź w pole email: `' OR '1'='1' --`
3. Wprowadź hasło: `anything`
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie nieudane
- Błąd walidacji lub generyczny komunikat o błędzie logowania
- Pozostanie na stronie `/auth/login`
- Użytkownik nie został zalogowany (menu użytkownika nie widoczne)

---

#### Test Case 2.3.7: Login with XSS Attempt in Email Field
**Priorytet:** High (Security)  
**Cel:** Weryfikacja zabezpieczeń przed XSS

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź w pole email: `<script>alert('XSS')</script>@test.com`
3. Wprowadź hasło: `Test1234!@#`
4. Kliknij "Login"
5. Obserwuj czy skrypt JavaScript został wykonany

**Oczekiwany rezultat:**
- Logowanie nieudane (invalid email)
- Skrypt JavaScript NIE jest wykonywany (brak alertu)
- Wartość jest prawidłowo sanityzowana/escaped
- Pozostanie na stronie logowania

---

## 3. Logout Flow

### 3.1 Logout - Positive Tests

#### Test Case 3.1.1: Successful Logout
**Priorytet:** High  
**Cel:** Weryfikacja poprawnego wylogowania użytkownika

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Będąc zalogowanym na `/job-applications`
2. Kliknij przycisk/link "Logout" w menu użytkownika
3. Obserwuj przekierowanie

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/logout` - strona potwierdzenia wylogowania
- Wyświetlenie widoku `LogoutConfirmation.cshtml`
- Menu użytkownika nie jest już widoczne
- Przyciski/linki nawigacyjne wskazują na bycie wylogowanym

---

#### Test Case 3.1.2: Logout and Verify No Access to Protected Resources
**Priorytet:** High  
**Cel:** Weryfikacja braku dostępu do chronionych zasobów po wylogowaniu

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Wyloguj się
3. Spróbuj przejść do `/job-applications`
4. Spróbuj przejść do `/job-applications/entry-job-details`
5. Spróbuj przejść do `/auth/user-settings`

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login` z każdej chronionej strony
- Możliwe ustawienie `returnUrl` w query string
- Menu użytkownika nie jest widoczne

---

#### Test Case 3.1.3: Logout Confirmation Page Navigation
**Priorytet:** Medium  
**Cel:** Weryfikacja nawigacji ze strony potwierdzenia wylogowania

**Prerekwizyty:** Użytkownik się wylogował i jest na `/auth/logout`

**Kroki:**
1. Będąc na stronie potwierdzenia wylogowania
2. Zweryfikuj widoczność linków:
   - "Log Back In"
   - "Go to Homepage"
3. Kliknij "Log Back In"
4. Zweryfikuj przekierowanie do `/auth/login`
5. Wróć do `/auth/logout` i kliknij "Go to Homepage"
6. Zweryfikuj przekierowanie do `/`

**Oczekiwany rezultat:**
- Wszystkie linki są widoczne i funkcjonalne
- "Log Back In" przekierowuje do `/auth/login`
- "Go to Homepage" przekierowuje do `/`
- Nawigacja działa poprawnie

---

### 3.2 Logout - Edge Cases

#### Test Case 3.2.1: Multiple Logout Attempts
**Priorytet:** Low  
**Cel:** Weryfikacja obsługi wielokrotnego wylogowania

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Wyloguj się pierwszy raz
3. Będąc na `/auth/logout` (GET - confirmation page), spróbuj ponownie wywołać wylogowanie (jeśli możliwe)

**Oczekiwany rezultat:**
- Brak błędów przy ponownym wylogowaniu
- Graceful handling (komunikat lub po prostu pozostanie na stronie potwierdzenia)
- Aplikacja nie ulega awarii

---

#### Test Case 3.2.2: Logout - Back Button Behavior
**Priorytet:** Medium  
**Cel:** Weryfikacja bezpieczeństwa po wylogowaniu przy użyciu przycisku wstecz

**Prerekwizyty:** Użytkownik był zalogowany

**Kroki:**
1. Zaloguj się
2. Przejdź do `/job-applications`
3. Wyloguj się
4. Kliknij przycisk wstecz przeglądarki (próba powrotu do `/job-applications`)

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Brak dostępu do chronionej strony
- Sesja jest faktycznie zakończona (nie tylko przekierowanie)

---

#### Test Case 3.2.3: Logout and Login with Different User
**Priorytet:** Medium  
**Cel:** Weryfikacja możliwości zalogowania innym użytkownikiem

**Prerekwizyty:** Dwóch użytkowników: `user1@example.com` i `user2@example.com`

**Kroki:**
1. Zaloguj się jako `user1@example.com`
2. Wyloguj się
3. Zaloguj się jako `user2@example.com`
4. Zweryfikuj że jesteś zalogowany jako user2

**Oczekiwany rezultat:**
- Wylogowanie user1 przebiegło pomyślnie
- Logowanie user2 przebiegło pomyślnie
- Menu użytkownika pokazuje dane user2 (jeśli wyświetlane)
- Brak mieszania sesji między użytkownikami

---

## 4. Forgot Password Flow

### 4.1 Forgot Password - Positive Tests

#### Test Case 4.1.1: Request Password Reset for Valid User
**Priorytet:** High  
**Cel:** Weryfikacja wysłania emaila resetującego hasło

**Prerekwizyty:** Użytkownik `resetuser@example.com` istnieje (utworzony w setup)

**Kroki:**
1. Przejdź do `/auth/login`
2. Kliknij "Forgot password?"
3. Zweryfikuj przekierowanie do `/auth/forgot-password`
4. Wprowadź email: `resetuser@example.com`
5. Kliknij "Submit"

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/forgot-password-confirmation`
- Wyświetlenie komunikatu o wysłaniu emaila
- Komunikat nie ujawnia czy email istnieje w systemie (security)

---

#### Test Case 4.1.2: Forgot Password Confirmation Page
**Priorytet:** Medium  
**Cel:** Weryfikacja wyświetlania strony potwierdzenia

**Prerekwizyty:** Użytkownik przeszedł przez formularz "Forgot Password"

**Kroki:**
1. Znajduj się na `/auth/forgot-password-confirmation`
2. Zweryfikuj treść komunikatu (informuje o wysłaniu emaila)
3. Zweryfikuj widoczność linku "Return to Login"
4. Kliknij "Return to Login"

**Oczekiwany rezultat:**
- Komunikat informuje o wysłaniu emaila (jeśli konto istnieje)
- Link "Return to Login" przekierowuje do `/auth/login`
- Strona jest przyjazna użytkownikowi

---

### 4.2 Forgot Password - Negative Tests

#### Test Case 4.2.1: Forgot Password with Non-Existent Email
**Priorytet:** High  
**Cel:** Weryfikacja braku enumeracji użytkowników

**Kroki:**
1. Przejdź do `/auth/forgot-password`
2. Wprowadź nieistniejący email: `nonexistent999@example.com`
3. Kliknij "Submit"

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/forgot-password-confirmation` (identycznie jak dla istniejącego usera)
- Ten sam komunikat wyświetlony (security best practice - brak enumeracji)
- Brak możliwości określenia czy email istnieje w systemie

---

#### Test Case 4.2.2: Forgot Password with Empty Email
**Priorytet:** Medium  
**Cel:** Weryfikacja walidacji wymaganego pola

**Kroki:**
1. Przejdź do `/auth/forgot-password`
2. Zostaw pole email puste
3. Kliknij "Submit"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: email jest wymagany
- Pozostanie na stronie `/auth/forgot-password`
- Formularz nie zostaje wysłany

---

#### Test Case 4.2.3: Forgot Password with Invalid Email Format
**Priorytet:** Medium  
**Cel:** Weryfikacja walidacji formatu email

**Kroki:**
1. Przejdź do `/auth/forgot-password`
2. Wprowadź nieprawidłowy email: `notanemail`
3. Kliknij "Submit"

**Oczekiwany rezultat:**
- Błąd walidacji formatu email wyświetlony
- Pozostanie na stronie `/auth/forgot-password`

---

### 4.3 Forgot Password - Edge Cases

#### Test Case 4.3.1: Multiple Password Reset Requests
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi wielokrotnych żądań resetowania

**Prerekwizyty:** Użytkownik istnieje

**Kroki:**
1. Wyślij żądanie resetowania hasła dla `user@example.com`
2. Natychmiast wyślij kolejne żądanie dla tego samego emaila
3. Opcjonalnie wyślij 3-5 żądań w krótkim czasie
4. Sprawdź czy wszystkie żądania zostały przyjęte

**Oczekiwany rezultat:**
- Wszystkie żądania obsłużone bez błędów
- Możliwe scenariusze:
  - Każde żądanie pokazuje confirmation page
  - Rate limiting: komunikat o limicie żądań w określonym czasie
- System nie ulega awarii

---

#### Test Case 4.3.2: Forgot Password Navigation - Back to Login
**Priorytet:** Low  
**Cel:** Weryfikacja nawigacji z formularza resetowania hasła

**Kroki:**
1. Przejdź do `/auth/forgot-password`
2. Kliknij link "Back to Login"
3. Zweryfikuj przekierowanie

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Formularz logowania widoczny

---

## 5. Reset Password Flow

### 5.1 Reset Password - Positive Tests

#### Test Case 5.1.1: Reset Password with Valid Token
**Priorytet:** High  
**Cel:** Weryfikacja pomyślnego resetowania hasła

**Prerekwizyty:** 
- Użytkownik `resetuser@example.com` z hasłem `OldPassword123!` istnieje
- Token resetujący został wygenerowany (przez forgot password flow lub bezpośrednio w setup)

**Kroki:**
1. Wyślij żądanie resetowania hasła dla `resetuser@example.com`
2. Pobierz link resetujący z logów aplikacji/testowego email service (format: `/auth/reset-password?userId={id}&token={token}`)
3. Przejdź do linku
4. Zweryfikuj wyświetlenie formularza resetowania hasła
5. Wprowadź nowe hasło: `NewPassword123!`
6. Potwierdź nowe hasło: `NewPassword123!`
7. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/reset-password-confirmation`
- Komunikat o pomyślnej zmianie hasła
- Link "Continue to Login" jest widoczny

---

#### Test Case 5.1.2: Reset Password Confirmation and Login with New Password
**Priorytet:** High  
**Cel:** Weryfikacja możliwości zalogowania po zmianie hasła

**Prerekwizyty:** Hasło zostało zresetowane w poprzednim teście (user: `resetuser@example.com`, nowe hasło: `NewPassword123!`)

**Kroki:**
1. Będąc na `/auth/reset-password-confirmation`
2. Kliknij "Continue to Login"
3. Na stronie logowania wprowadź email: `resetuser@example.com`
4. Wprowadź NOWE hasło: `NewPassword123!`
5. Kliknij "Login"

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Logowanie z nowym hasłem działa
- Przekierowanie do `/job-applications`
- Menu użytkownika widoczne (użytkownik zalogowany)

---

#### Test Case 5.1.3: Old Password Invalid After Reset
**Priorytet:** High  
**Cel:** Weryfikacja że stare hasło nie działa po resecie

**Prerekwizyty:** Hasło użytkownika `resetuser@example.com` zostało zresetowane z `OldPassword123!` na `NewPassword123!`

**Kroki:**
1. Przejdź do `/auth/login`
2. Wprowadź email: `resetuser@example.com`
3. Wprowadź STARE hasło: `OldPassword123!` (sprzed resetu)
4. Kliknij "Login"

**Oczekiwany rezultat:**
- Logowanie nieudane
- Komunikat: "Login failed. Please check your email and password."
- Stare hasło nie jest akceptowane
- Użytkownik nie został zalogowany

---

### 5.2 Reset Password - Negative Tests

#### Test Case 5.2.1: Reset Password with Invalid Token
**Priorytet:** High  
**Cel:** Weryfikacja odrzucenia nieprawidłowego tokenu

**Prerekwizyty:** Użytkownik istnieje

**Kroki:**
1. Utwórz URL z prawidłowym userId ale nieprawidłowym tokenem:
   `/auth/reset-password?userId={validUserId}&token=InvalidTokenXYZ123`
2. Przejdź do tego URL
3. Wprowadź nowe hasło: `NewPassword123!`
4. Potwierdź hasło
5. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Pozostanie na stronie `/auth/reset-password` ALBO przekierowanie do błędu
- Komunikat błędu: "Password reset link may have expired or is invalid. Please try again or request a new one."
- Możliwość ponownego żądania linku

---

#### Test Case 5.2.2: Reset Password with Missing Token Parameter
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi brakujących parametrów

**Kroki:**
1. Przejdź do `/auth/reset-password?userId={validUserId}` (brak parametru token)

**Oczekiwany rezultat:**
- Przekierowanie do `/Home/Error` LUB
- Wyświetlenie komunikatu o błędzie/brakujących parametrach
- Brak możliwości resetowania hasła

---

#### Test Case 5.2.3: Reset Password with Missing UserId Parameter
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi brakującego userId

**Kroki:**
1. Przejdź do `/auth/reset-password?token={someToken}` (brak parametru userId)

**Oczekiwany rezultat:**
- Przekierowanie do `/Home/Error` LUB
- Wyświetlenie komunikatu o błędzie
- Brak możliwości resetowania hasła

---

#### Test Case 5.2.4: Reset Password with Non-Existent UserId
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi nieistniejącego użytkownika

**Kroki:**
1. Przejdź do `/auth/reset-password?userId=00000000-0000-0000-0000-000000000000&token=sometoken`
2. Obserwuj co się wyświetla

**Oczekiwany rezultat:**
- Komunikat błędu: "Invalid password reset request." ALBO
- Przekierowanie do strony błędu

---

#### Test Case 5.2.5: Reset Password - New Passwords Don't Match
**Priorytet:** High  
**Cel:** Weryfikacja walidacji zgodności haseł

**Prerekwizyty:** Prawidłowy token resetujący

**Kroki:**
1. Przejdź do prawidłowego linku resetującego
2. Wprowadź nowe hasło: `NewPassword123!`
3. Wprowadź inne hasło w potwierdzeniu: `DifferentPass123!`
4. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: hasła nie są zgodne
- Pozostanie na stronie `/auth/reset-password`
- Formularz wyświetla błąd

---

#### Test Case 5.2.6: Reset Password - New Password Too Short
**Priorytet:** High  
**Cel:** Weryfikacja wymagań minimalnej długości hasła

**Prerekwizyty:** Prawidłowy token resetujący

**Kroki:**
1. Przejdź do prawidłowego linku resetującego
2. Wprowadź krótkie hasło: `Test1!` (7 znaków)
3. Potwierdź hasło
4. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: hasło zbyt krótkie (minimum 8 znaków)
- Pozostanie na stronie formularza

---

#### Test Case 5.2.7: Reset Password - Missing Password Complexity Requirements
**Priorytet:** High  
**Cel:** Weryfikacja wymagań złożoności hasła przy resecie

**Prerekwizyty:** Prawidłowy token resetujący

**Kroki:**
1. Przejdź do prawidłowego linku resetującego
2. Wprowadź hasło bez wymaganych elementów: `password` (brak wielkiej litery, cyfry, znaku specjalnego)
3. Potwierdź hasło
4. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Błędy walidacji wyświetlone (wymagania hasła: wielka litera, cyfra, znak specjalny)
- Pozostanie na stronie formularza

---

#### Test Case 5.2.8: Reset Password with Empty Fields
**Priorytet:** Medium  
**Cel:** Weryfikacja walidacji pustych pól

**Prerekwizyty:** Prawidłowy token resetujący

**Kroki:**
1. Przejdź do prawidłowego linku resetującego
2. Zostaw pola hasła puste
3. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Błędy walidacji wyświetlone: hasło jest wymagane
- Formularz nie zostaje wysłany lub zwrócony z błędem

---

### 5.3 Reset Password - Edge Cases

#### Test Case 5.3.1: Reuse Reset Token After Successful Reset
**Priorytet:** High  
**Cel:** Weryfikacja że token nie może być użyty ponownie

**Prerekwizyty:** Token został już użyty do resetu hasła

**Kroki:**
1. Zresetuj hasło używając tokenu (test 5.1.1)
2. Zapisz ten sam link/token
3. Spróbuj użyć tego samego linku/tokenu ponownie
4. Wprowadź nowe hasło
5. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Token jest nieważny
- Komunikat błędu o wygasłym/nieprawidłowym linku
- Resetowanie nie powiodło się

---

#### Test Case 5.3.2: Reset Password with SQL Injection Attempt
**Priorytet:** High (Security)  
**Cel:** Weryfikacja zabezpieczeń przed SQL Injection

**Prerekwizyty:** Prawidłowy link resetujący

**Kroki:**
1. Przejdź do linku resetującego
2. Wprowadź w pole hasła: `' OR '1'='1' --`
3. Potwierdź hasło
4. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Hasło jest traktowane jako zwykły string
- Może wystąpić błąd walidacji (zbyt krótkie lub brak wymaganych znaków)
- Brak wykonania złośliwego kodu
- System bezpieczny przed atakiem

---

#### Test Case 5.3.3: Reset Password Navigation - Back to Login
**Priorytet:** Low  
**Cel:** Weryfikacja opcji rezygnacji z resetowania

**Kroki:**
1. Przejdź do prawidłowego linku resetującego
2. Kliknij link "Back to Login"
3. Zweryfikuj przekierowanie

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Formularz logowania widoczny
- Użytkownik może zrezygnować z resetowania hasła

---

#### Test Case 5.3.4: Reset Password - Very Long Password
**Priorytet:** Low  
**Cel:** Weryfikacja obsługi bardzo długich haseł

**Prerekwizyty:** Prawidłowy token resetujący

**Kroki:**
1. Przejdź do prawidłowego linku resetującego
2. Wprowadź bardzo długie hasło (100+ znaków) spełniające wymagania
3. Potwierdź hasło
4. Kliknij "Reset Password"

**Oczekiwany rezultat:**
- Resetowanie pomyślne (jeśli brak górnego limitu) ALBO
- Błąd walidacji z informacją o maksymalnej długości

---

## 6. User Settings Flow

### 6.1 User Settings - Positive Tests

#### Test Case 6.1.1: View User Settings Page
**Priorytet:** High  
**Cel:** Weryfikacja dostępu do ustawień użytkownika

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Przejdź do `/auth/user-settings` (przez menu lub bezpośrednio)
3. Zweryfikuj wyświetlenie strony

**Oczekiwany rezultat:**
- Strona `/auth/user-settings` się ładuje
- Formularz ustawień jest widoczny
- Dropdown z strefami czasowymi jest widoczny i zawiera opcje
- Jedna z stref czasowych jest zaznaczona (aktualna)
- Przycisk "Save" jest widoczny

---

#### Test Case 6.1.2: Update Time Zone Setting
**Priorytet:** High  
**Cel:** Weryfikacja zmiany strefy czasowej

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Przejdź do `/auth/user-settings`
2. Sprawdź aktualnie zaznaczoną strefę czasową
3. Wybierz inną strefę czasową: `America/New_York`
4. Kliknij "Save"

**Oczekiwany rezultat:**
- Komunikat potwierdzenia wyświetlony: ustawienia zostały zaktualizowane
- Pozostanie na stronie `/auth/user-settings`
- Nowa strefa czasowa (`America/New_York`) jest teraz zaznaczona w dropdown

---

#### Test Case 6.1.3: Verify Time Zone Applies to Displayed Dates
**Priorytet:** High  
**Cel:** Weryfikacja zastosowania strefy czasowej do dat w aplikacji

**Prerekwizyty:** Użytkownik zmienił strefę czasową na `America/New_York`

**Kroki:**
1. Upewnij się że strefa czasowa jest ustawiona na `America/New_York`
2. Przejdź do `/job-applications` (jeśli istnieją aplikacje) lub utwórz nową
3. Sprawdź wyświetlane daty utworzenia/aktualizacji
4. Zmień strefę czasową na `Europe/Warsaw`
5. Ponownie sprawdź te same daty

**Oczekiwany rezultat:**
- Daty wyświetlane są w wybranej strefie czasowej
- Po zmianie strefy czasowej daty się zmieniają (różny offset)
- Format dat jest poprawny i czytelny

---

#### Test Case 6.1.4: Update Time Zone Multiple Times
**Priorytet:** Medium  
**Cel:** Weryfikacja wielokrotnej zmiany ustawień

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Przejdź do `/auth/user-settings`
2. Zmień strefę czasową na `Europe/London` → Kliknij "Save"
3. Sprawdź komunikat potwierdzenia
4. Zmień strefę czasową na `Asia/Tokyo` → Kliknij "Save"
5. Sprawdź komunikat potwierdzenia
6. Zmień strefę czasową na `Europe/Warsaw` → Kliknij "Save"
7. Sprawdź komunikat potwierdzenia

**Oczekiwany rezultat:**
- Każda zmiana jest zapisywana pomyślnie
- Komunikaty potwierdzenia po każdym zapisie
- Ostatnia wartość (`Europe/Warsaw`) jest zachowana i zaznaczona
- Brak błędów

---

#### Test Case 6.1.5: User Settings Persist Across Sessions
**Priorytet:** Medium  
**Cel:** Weryfikacja trwałości ustawień po wylogowaniu

**Prerekwizyty:** Użytkownik ustawił strefę czasową na `Europe/Warsaw`

**Kroki:**
1. Ustaw strefę czasową na `Europe/Warsaw` i zapisz
2. Wyloguj się
3. Zaloguj się ponownie tym samym użytkownikiem
4. Przejdź do `/auth/user-settings`
5. Sprawdź zaznaczoną strefę czasową

**Oczekiwany rezultat:**
- Strefa czasowa `Europe/Warsaw` nadal jest wybrana
- Ustawienia zostały zachowane między sesjami
- Aplikacja pamięta preferencje użytkownika

---

### 6.2 User Settings - Negative Tests

#### Test Case 6.2.1: Access User Settings Without Authentication
**Priorytet:** High  
**Cel:** Weryfikacja wymuszenia autoryzacji na stronie ustawień

**Prerekwizyty:** Użytkownik nie jest zalogowany

**Kroki:**
1. Upewnij się że nie jesteś zalogowany
2. Spróbuj przejść bezpośrednio do `/auth/user-settings`

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Możliwe ustawienie `returnUrl=/auth/user-settings`
- Brak dostępu do strony ustawień bez logowania

---

#### Test Case 6.2.2: Submit Invalid Time Zone ID
**Priorytet:** Medium  
**Cel:** Weryfikacja walidacji strefy czasowej

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Przejdź do `/auth/user-settings`
3. Za pomocą developer tools zmień wartość w dropdown na nieprawidłową: `Invalid/Timezone`
4. Kliknij "Save"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: nieprawidłowa strefa czasowa
- Komunikat o błędzie na stronie
- Pozostanie na stronie `/auth/user-settings`
- Poprzednia wartość pozostaje (zmiana nie została zapisana)

---

#### Test Case 6.2.3: Submit Empty Time Zone
**Priorytet:** Medium  
**Cel:** Weryfikacja wymagalności strefy czasowej

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Przejdź do `/auth/user-settings`
3. Za pomocą developer tools usuń wartość TimeZoneId (pusta wartość)
4. Kliknij "Save"

**Oczekiwany rezultat:**
- Błąd walidacji wyświetlony: strefa czasowa jest wymagana
- Formularz wyświetla błąd
- Zmiana nie została zapisana

---

### 6.3 User Settings - Edge Cases

#### Test Case 6.3.1: Verify All Available Time Zones
**Priorytet:** Low  
**Cel:** Weryfikacja kompletności listy stref czasowych

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Przejdź do `/auth/user-settings`
2. Otwórz dropdown ze strefami czasowymi
3. Zweryfikuj obecność kluczowych stref:
   - `UTC`
   - `America/New_York`
   - `Europe/London`
   - `Europe/Warsaw`
   - `Asia/Tokyo`
4. Sprawdź czy lista jest posortowana

**Oczekiwany rezultat:**
- Wszystkie główne strefy czasowe są dostępne
- Lista jest alfabetycznie posortowana (według nazwy)
- Każda strefa ma czytelną nazwę

---

#### Test Case 6.3.2: User Settings with Special Time Zones (Half-hour offsets)
**Priorytet:** Low  
**Cel:** Weryfikacja obsługi nietypowych offsetów

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Przejdź do `/auth/user-settings`
2. Sprawdź czy w liście są strefy z half-hour offset (np. `Asia/Kolkata` UTC+5:30)
3. Jeśli dostępne, wybierz taką strefę
4. Kliknij "Save"
5. Przejdź do strony z datami i sprawdź ich wyświetlanie

**Oczekiwany rezultat:**
- Strefa z nietypowym offsetem jest obsługiwana (jeśli w liście)
- Zapis przebiegł pomyślnie
- Daty wyświetlane są poprawnie z uwzględnieniem nietypowego offsetu

---

## 7. Access Control & Authorization

### 7.1 Access Control - Positive Tests

#### Test Case 7.1.1: Authenticated User Access to Protected Resources
**Priorytet:** High  
**Cel:** Weryfikacja dostępu zalogowanego użytkownika do chronionych zasobów

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Przejdź do `/job-applications`
3. Przejdź do `/job-applications/entry-job-details` (lub innej chronionej strony)
4. Przejdź do `/auth/user-settings`
5. Zweryfikuj że wszystkie strony się ładują

**Oczekiwany rezultat:**
- Wszystkie chronione strony są dostępne
- Brak przekierowań do `/auth/login`
- Treść stron jest wyświetlana poprawnie
- Menu użytkownika widoczne na każdej stronie

---

### 7.2 Access Control - Negative Tests

#### Test Case 7.2.1: Unauthenticated Access to Protected Pages
**Priorytet:** High  
**Cel:** Weryfikacja blokowania dostępu niezalogowanym użytkownikom

**Prerekwizyty:** Użytkownik nie jest zalogowany

**Kroki:**
1. Upewnij się że nie jesteś zalogowany
2. Spróbuj przejść do `/job-applications`
3. Zweryfikuj przekierowanie
4. Spróbuj przejść do `/job-applications/entry-job-details`
5. Zweryfikuj przekierowanie
6. Spróbuj przejść do `/auth/user-settings`
7. Zweryfikuj przekierowanie

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login` dla każdej chronionej strony
- Query parameter `returnUrl` może zawierać żądaną stronę
- Brak wyświetlenia treści chronionych stron
- Menu użytkownika nie jest widoczne

---

#### Test Case 7.2.2: Access Denied Page Display
**Priorytet:** Medium  
**Cel:** Weryfikacja strony Access Denied

**Kroki:**
1. Przejdź bezpośrednio do `/auth/access-denied`
2. Zweryfikuj wyświetlenie strony

**Oczekiwany rezultat:**
- Strona `/auth/access-denied` się wyświetla
- Widoczny komunikat o braku uprawnień
- Linki nawigacyjne są widoczne:
  - "Return to Home Page" → `/`
  - "Go to Sign In" → `/auth/login`
- Kliknięcie linków przekierowuje poprawnie

---

### 7.3 Access Control - Edge Cases

#### Test Case 7.3.1: Access Protected Resource After Session Timeout
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi wygasłej sesji

**Prerekwizyty:** Użytkownik był zalogowany ale sesja wygasła (lub symulacja przez usunięcie cookies)

**Kroki:**
1. Zaloguj się
2. Ręcznie usuń cookie sesyjne (przez developer tools) LUB poczekaj na wygaśnięcie sesji
3. Spróbuj przejść do `/job-applications`

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Możliwe ustawienie `returnUrl`
- Menu użytkownika nie jest widoczne
- Użytkownik musi zalogować się ponownie

---

#### Test Case 7.3.2: Access Public Pages While Authenticated
**Priorytet:** Low  
**Cel:** Weryfikacja dostępu do publicznych stron będąc zalogowanym

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Przejdź do `/` (home page)
3. Przejdź do `/auth/login`
4. Przejdź do `/auth/register`
5. Przejdź do `/auth/access-denied`

**Oczekiwany rezultat:**
- Publiczne strony są dostępne
- Możliwe przekierowania (np. z `/auth/login` do dashboard jeśli już zalogowany - w zależności od implementacji)
- Brak błędów
- Zachowanie jest spójne

---

## 8. Navigation & UI Flow

### 8.1 Navigation - Positive Tests

#### Test Case 8.1.1: Complete Registration to Job Applications Flow
**Priorytet:** High  
**Cel:** Weryfikacja pełnego przepływu nowego użytkownika

**Kroki:**
1. Przejdź do `/` (home page)
2. Kliknij "Register" lub przejdź do `/auth/register`
3. Zarejestruj nowe konto z poprawnymi danymi: `flowtest@example.com` / `Test1234!@#`
4. Zweryfikuj automatyczne logowanie (menu użytkownika widoczne)
5. Zweryfikuj przekierowanie do `/job-applications`
6. Sprawdź dostępność menu użytkownika (Settings, Logout)

**Oczekiwany rezultat:**
- Płynny przepływ od strony głównej przez rejestrację do dashboard
- Użytkownik jest automatycznie zalogowany
- Dashboard (`/job-applications`) jest dostępny
- Menu zawiera opcje użytkownika (Settings, Logout)
- Wszystkie przekierowania działają poprawnie

---

#### Test Case 8.1.2: Navigation Between Auth Pages
**Priorytet:** Medium  
**Cel:** Weryfikacja linków nawigacyjnych między stronami auth

**Kroki:**
1. Login → kliknij "Sign up" → zweryfikuj przekierowanie do Register
2. Register → kliknij "Log in" → zweryfikuj przekierowanie do Login
3. Login → kliknij "Forgot password?" → zweryfikuj przekierowanie do Forgot Password
4. Forgot Password → kliknij "Back to Login" → zweryfikuj przekierowanie do Login
5. Wyślij forgot password request → kliknij "Return to Login" na confirmation page

**Oczekiwany rezultat:**
- Wszystkie linki działają poprawnie
- Przekierowania są prawidłowe (odpowiednie URLe)
- Nawigacja jest intuicyjna
- Brak błędów 404

---

#### Test Case 8.1.3: User Menu Navigation
**Priorytet:** Medium  
**Cel:** Weryfikacja menu użytkownika

**Prerekwizyty:** Użytkownik jest zalogowany

**Kroki:**
1. Zaloguj się
2. Zweryfikuj widoczność menu użytkownika (dropdown lub navigation bar)
3. Znajdź i kliknij "Settings" lub "User Settings"
4. Zweryfikuj przekierowanie do `/auth/user-settings`
5. Wróć do dashboard
6. Znajdź i kliknij "Logout"
7. Zweryfikuj wylogowanie

**Oczekiwany rezultat:**
- Menu użytkownika jest widoczne po zalogowaniu (w header/navbar)
- Opcje w menu są funkcjonalne
- "Settings" prowadzi do `/auth/user-settings`
- "Logout" prowadzi do `/auth/logout` (confirmation page)
- Wszystkie linki działają poprawnie

---

### 8.2 Navigation - Edge Cases

#### Test Case 8.2.1: Browser Back Button After Login
**Priorytet:** Medium  
**Cel:** Weryfikacja obsługi przycisku wstecz przeglądarki po logowaniu

**Prerekwizyty:** Użytkownik się zalogował

**Kroki:**
1. Przejdź do `/auth/login`
2. Zaloguj się (przekierowanie do `/job-applications`)
3. Kliknij przycisk wstecz przeglądarki

**Oczekiwany rezultat:**
- Możliwe scenariusze (w zależności od implementacji):
  - Przekierowanie z powrotem do `/job-applications` (użytkownik już zalogowany)
  - Wyświetlenie strony logowania (ale użytkownik jest zalogowany, menu widoczne)
- Brak błędów
- Zachowanie jest graceful i bezpieczne

---

#### Test Case 8.2.2: Browser Back Button After Logout
**Priorytet:** Medium (Security)  
**Cel:** Weryfikacja bezpieczeństwa po wylogowaniu

**Prerekwizyty:** Użytkownik był zalogowany i się wylogował

**Kroki:**
1. Zaloguj się
2. Przejdź do `/job-applications`
3. Wyloguj się
4. Kliknij przycisk wstecz przeglądarki (próba powrotu do `/job-applications`)

**Oczekiwany rezultat:**
- Przekierowanie do `/auth/login`
- Brak dostępu do chronionej strony
- Cache przeglądarki nie wyświetla wrażliwych danych
- Menu użytkownika nie jest widoczne

---

#### Test Case 8.2.3: Direct URL Access to Auth Pages
**Priorytet:** Low  
**Cel:** Weryfikacja bezpośredniego dostępu przez URL

**Kroki:**
1. Wpisz bezpośrednio w pasku adresu następujące URLe i sprawdź czy się ładują:
   - `/auth/login`
   - `/auth/register`
   - `/auth/forgot-password`
   - `/auth/forgot-password-confirmation`
   - `/auth/access-denied`
2. Dla każdego URL zweryfikuj czy strona się poprawnie wyświetla

**Oczekiwany rezultat:**
- Publiczne strony (login, register, forgot-password) są dostępne
- Strony confirmation można odwiedzić bezpośrednio (nie wywołują błędów)
- Wszystkie strony ładują się poprawnie
- Brak błędów 404 lub 500

---

## 9. Security & Data Integrity

### 9.1 Security Tests

#### Test Case 9.1.1: CSRF Protection on Forms
**Priorytet:** High (Security)  
**Cel:** Weryfikacja ochrony przed CSRF

**Kroki:**
1. Otwórz formularz (np. Login) w przeglądarce
2. Otwórz developer tools → Elements/Inspector
3. Sprawdź czy formularz zawiera ukryte pole z CSRF token (np. `__RequestVerificationToken`)
4. Za pomocą developer tools usuń pole z tokenem
5. Spróbuj wysłać formularz

**Oczekiwany rezultat:**
- Wszystkie formularze zawierają CSRF token (hidden input)
- Próba wysłania bez tokenu jest odrzucona
- Błąd 400 (Bad Request) lub odpowiedni komunikat o błędzie
- System chroniony przed atakami CSRF

---

#### Test Case 9.1.2: Password Not Visible in Network Traffic
**Priorytet:** High (Security)  
**Cel:** Weryfikacja że hasła nie są widoczne w URL

**Kroki:**
1. Otwórz DevTools → Network tab
2. Wypełnij formularz logowania
3. Kliknij "Login"
4. W Network tab znajdź request do `/auth/login`
5. Sprawdź:
   - Request URL nie zawiera hasła
   - Method to POST
   - Hasło jest w body requesta (nie w query string)

**Oczekiwany rezultat:**
- Hasło jest wysyłane w body POSTa, nie w URL
- Request method to POST (nie GET)
- Hasło nie jest widoczne w query string ani w headers
- W środowisku produkcyjnym połączenie używa HTTPS

---

#### Test Case 9.1.3: Session Cookie Security Attributes
**Priorytet:** High (Security)  
**Cel:** Weryfikacja atrybutów bezpieczeństwa cookie sesyjnego

**Prerekwizyty:** Użytkownik zalogowany

**Kroki:**
1. Zaloguj się
2. Otwórz DevTools → Application (Chrome) / Storage (Firefox) → Cookies
3. Znajdź cookie sesyjne (np. `.AspNetCore.Identity.Application`)
4. Zweryfikuj atrybuty:
   - `HttpOnly`: powinno być zaznaczone/true
   - `Secure`: powinno być zaznaczone/true (w produkcji z HTTPS)
   - `SameSite`: powinno być `Lax` lub `Strict`

**Oczekiwany rezultat:**
- Cookie ma flagę `HttpOnly` (zapobiega dostępowi JavaScript)
- Cookie ma flagę `Secure` w środowisku produkcyjnym z HTTPS
- Cookie ma odpowiednie ustawienie `SameSite` (`Lax` lub `Strict`)
- Atrybuty bezpieczeństwa są prawidłowo skonfigurowane

---

#### Test Case 9.1.4: No Sensitive Data in Browser History/URL
**Priorytet:** Medium (Security)  
**Cel:** Weryfikacja że wrażliwe dane nie są w historii przeglądarki

**Kroki:**
1. Wykonaj różne operacje auth (login, register, reset password)
2. Otwórz historię przeglądarki (Ctrl+H)
3. Sprawdź zapisane URLe

**Oczekiwany rezultat:**
- URLe nie zawierają haseł
- Tokeny resetowania są w query string (dopuszczalne dla jednorazowych linków email)
- Historia nie ujawnia innych wrażliwych informacji (emaile są OK w history)
- Hasła nigdy nie są w URL

---

### 9.2 Data Integrity Tests

#### Test Case 9.2.1: User Can Perform Actions After Registration
**Priorytet:** High  
**Cel:** Weryfikacja że nowo utworzone konto jest w pełni funkcjonalne

**Kroki:**
1. Zarejestruj nowego użytkownika
2. Spróbuj wykonać różne akcje:
   - Zmień ustawienia strefy czasowej
   - Przejdź do tworzenia job application (jeśli dostępne)
   - Wyloguj się
   - Zaloguj ponownie

**Oczekiwany rezultat:**
- Wszystkie akcje są możliwe do wykonania
- Użytkownik może w pełni korzystać z aplikacji
- Ustawienia są zachowywane
- Logowanie działa poprawnie

---

#### Test Case 9.2.2: Email Uniqueness Enforcement in UI
**Priorytet:** High  
**Cel:** Weryfikacja że UI prawidłowo informuje o duplikacie emaila

**Prerekwizyty:** Użytkownik `unique@example.com` już istnieje

**Kroki:**
1. Spróbuj zarejestrować nowego użytkownika z emailem `unique@example.com`
2. Wypełnij formularz i kliknij "Register"
3. Sprawdź wyświetlony komunikat

**Oczekiwany rezultat:**
- Rejestracja odrzucona
- Komunikat: "An account with this email already exists. Please log in or reset your password."
- Pozostanie na stronie `/auth/register`
- Możliwość wprowadzenia innego emaila

---

## 10. End-to-End Scenarios

### 10.1 Complete User Journeys

#### Test Case 10.1.1: New User Complete Journey
**Priorytet:** High  
**Cel:** Weryfikacja pełnego przepływu nowego użytkownika od rejestracji do korzystania z aplikacji

**Kroki:**
1. Przejdź do home page `/`
2. Kliknij "Register"
3. Zarejestruj nowe konto: `journey@example.com` / `Journey123!`
4. Zweryfikuj automatyczne logowanie i przekierowanie do dashboard
5. Przejdź do User Settings
6. Zmień strefę czasową na `Europe/Warsaw`
7. Przejdź do job applications
8. Wyloguj się
9. Zaloguj się ponownie z tymi samymi danymi
10. Zweryfikuj że ustawienia zostały zachowane

**Oczekiwany rezultat:**
- Cały przepływ działa płynnie bez błędów
- Użytkownik może się zarejestrować, zmienić ustawienia, wylogować i zalogować
- Wszystkie dane są zachowywane
- Doświadczenie użytkownika jest spójne

---

#### Test Case 10.1.2: Password Reset Complete Journey
**Priorytet:** High  
**Cel:** Weryfikacja pełnego przepływu resetowania hasła

**Prerekwizyty:** Użytkownik `resetjounrey@example.com` z hasłem `OldPass123!` istnieje

**Kroki:**
1. Przejdź do `/auth/login`
2. Kliknij "Forgot password?"
3. Wprowadź email: `resetjourney@example.com`
4. Kliknij "Submit"
5. Zweryfikuj wyświetlenie confirmation page
6. Pobierz link resetujący z logów/test email service
7. Przejdź do linku resetującego
8. Wprowadź nowe hasło: `NewPass123!`
9. Potwierdź i zapisz
10. Zweryfikuj confirmation page
11. Kliknij "Continue to Login"
12. Zaloguj się nowym hasłem
13. Zweryfikuj pomyślne logowanie
14. Wyloguj się
15. Spróbuj zalogować się starym hasłem
16. Zweryfikuj że stare hasło nie działa

**Oczekiwany rezultat:**
- Cały proces resetowania hasła działa poprawnie
- Nowe hasło działa
- Stare hasło nie działa
- Wszystkie przekierowania i komunikaty są prawidłowe
- Link resetujący jest jednorazowy (nie można użyć ponownie)

---

---

## Summary

**Łączna liczba test case'ów: 120+**

### Podział według kategorii:
1. **Registration Flow**: 19 testów (6 pozytywnych, 9 negatywnych, 4 edge cases)
2. **Login Flow**: 20 testów (5 pozytywnych, 7 negatywnych, 8 edge cases)
3. **Logout Flow**: 6 testów (3 pozytywne, 3 edge cases)
4. **Forgot Password Flow**: 7 testów (2 pozytywne, 3 negatywne, 2 edge cases)
5. **Reset Password Flow**: 15 testów (3 pozytywne, 8 negatywne, 4 edge cases)
6. **User Settings Flow**: 11 testów (5 pozytywnych, 3 negatywne, 3 edge cases)
7. **Access Control & Authorization**: 5 testów (1 pozytywny, 2 negatywne, 2 edge cases)
8. **Navigation & UI Flow**: 6 testów (3 pozytywne, 3 edge cases)
9. **Security & Data Integrity**: 6 testów (4 security, 2 data integrity)
10. **End-to-End Scenarios**: 2 testy (complete user journeys)

### Priorytety:
- **High Priority**: ~55 testów (core functionality, security, critical paths)
- **Medium Priority**: ~40 testów (important validation, edge cases)
- **Low Priority**: ~15 testów (nice-to-have, UI/UX, minor edge cases)

### Kluczowe zasady testów E2E:
✅ Weryfikacja przez obserwowalne zachowanie aplikacji  
✅ Brak bezpośredniego dostępu do bazy danych  
✅ Sprawdzanie przez UI: komunikaty, przekierowania, widoczność elementów  
✅ Weryfikacja funkcjonalności przez możliwość wykonania kolejnych akcji  
✅ Scenariusze pozytywne (happy path)  
✅ Scenariusze negatywne (błędne dane, walidacje)  
✅ Edge cases (granice wartości, nietypowe sytuacje)  
✅ Security tests (CSRF, injection, session security)  
❌ Email confirmation scenarios (pominięte - feature flag wyłączony)  
❌ Bezpośrednie sprawdzanie bazy danych  
❌ Weryfikacja rekordów w tabelach  

### Uwagi implementacyjne:
- Testy zakładają że feature flag `ConfirmRegistration` jest wyłączony (automatyczne logowanie po rejestracji)
- Wymagane środowisko testowe z Docker + PostgreSQL
- Test fixtures dla setup/cleanup danych testowych (tworzenie użytkowników przed testami)
- Użycie Playwright dla automatyzacji E2E
- Page Object Pattern dla lepszej maintainability testów