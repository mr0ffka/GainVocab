GainVocab - aplikacja to nauki obcojęzycznego słownictwa

Wymagania:
1. Runtime .NET 6 -  https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-6.0.13-windows-hosting-bundle-installer
2. Node.js 18.11.2 - https://nodejs.org/download/release/v18.11.0/
3. PostgreSQL 14 - https://www.enterprisedb.com/postgresql-tutorial-resources-training?uuid=db55e32d-e9f0-4d7c-9aef-b17d01210704&campaignId=7012J000001NhszQAC

Instrukcja uruchomienia:
1. Zainstalowanie wymaganego oprogramowania
2. Utworzenie bazy danych o nazwie gainvocab_db, z użytkownikiem: postgres z hasłem postgres
3. Wykoananie skryptu gainvocab_db.sql zawierającego przykładowe dane testowe
4. Włączenie aplikacji serwera:
  	-> przejść do katalogu GainVocab.API.App\bin\Release\net6.0\publish
	-> włączyć GainVocab.API.App.exe jako administrator
	powinna się włączyć konsola cmd. Aplikacja powinna słuchać na http://localhost:5000
5. Włączenie aplikacji klienckiej:
	-> Przejść do katalogu GainVocab.Web
	-> jako administrator wykonać w folderze polecenie npx vite --port 4000
	-> w przeglądarce przejść na adres http://localhost:4000

Ważne żeby porty i adres interfejsu była taka sama jak w instrukcji. Inne wartości mogą spowodować nieoczekiwane błędy - m.in. problemy z CORS.

Konta użytkowników:
Administrator:
l. admin@example.com
h. P@ssword1

Użytkownik:
l. test@example.com
h. P@ssword1

Do przechwytywania wysyłanych maili skorzystano z darmowego serwisu do testowania maili - https://ethereal.email
Konto: 
l. melvin74@ethereal.email,
h. 2J5txMXExMJtKe4YP7

Konto jest czasowe, jeżeli wygaśnie trzeba zrobić nowe za pośrednictwem przycisku "Create Ethereal Account" oraz podmienić dane logowania w pliku appsettings.json znajdującego się w folderze GainVocab.API.App\bin\Release\net6.0\publish 