# Aplikacja do Zarządzania Książkami

Aplikacja umożliwia użytkownikom rejestrację, przeglądanie dostępnych książek oraz rezerwowanie wolnych książek za pomocą API. Dodatkowo, do aplikacji dołączony jest skrypt bash do łatwego deployowania aplikacji na serwerze.

## Funkcjonalności

- **Zakładanie kont użytkowników** – Użytkownicy mogą tworzyć swoje konto i logować się do aplikacji.
- **Przeglądanie dostępnych książek** – Możliwość przeglądania dostępnych książek w bazie.
- **Rezerwacja wolnych książek** – Użytkownicy mogą rezerwować dostępne książki na określony czas.
- **Zarządzanie księgozbiorem** - Użytkownik z rolą SuperUser może edytować i usuwać istniejące książki, dodawać nowe.

## Instalacja

1. Sklonuj repozytorium:

   ```bash
   git clone https://github.com/mpienk102/PAWLibrary
   cd PAWLibrary
2. Uruchom kontener z pliku docker-compose.yaml:
   ```bash
   docker-compose up -d
3. Wykonaj migrację do bazy danych. W folderze głównym projektu:
   ```
   dotnet ef migrations add InitialCreate
4. Zaaktualizuj bazę danych:
   ```
   dotnet ef database update

## Uruchomienie
1. Upewnij się, że masz odpowiednie uprawnienia do uruchomienia skryptu:
   ```
   chmod +x deploy.sh
2. Uruchom skrypt deployu:
   ```
   ./deploy.sh

Plik deploy.sh uruchamia kontener *library_db*, uruchamia serwer http na porcie 4000 i uruchamia API na porcie 5288
