<p>
  Aplikacja jest stworzona w architekturze .NET 8.0 przy użyciu technologii Blazor, RestFul Web API ASP.NET Core 8.0 do obsługi żądań wysyłanych przez front oraz SQL Servera 2022 do przechowywania i przetwarzania danych. Frontend został stworzony z wykorzystaniem biblioteki MudBlazor, która udostępnia prosty i przejrzysty layout oraz opcje okienek dialogowych, menu, overlayu czy snackbarów.
</p>
<p>
  Aby móc uruchomić lokalnie aplikację trzeba ustawić dwa projekty jako startowe, tj. TimeManager.Server oraz TimeManager.WebAPI, gdzie oba projekty muszą być uruchomione z ustawioną opcją IIS Express. W projekcie TimeManager.Domain znajduje się jedna migracja obiektów do bazy danych (Code first + Entity framework), która jest migrowana na nowo z każdym uruchomieniem aplikacji w trybie developerskim. Zostało to obsłużone w <i><b>TimeManager.WebAPI.Extensions.WebApplicationExtension.ReMigrateDatabase(this WebApplication app)</b></i>
</p>
<p>
  Po uruchomieniu aplikacji, domyślnie powinna zostać utworzona baza danych w katalogu "C:\Users\[UserName]\TimeManagerData.mdf". Jeżeli z tego powodu są jakieś problemy, można ewentualnie zmienić w inicjalizacji DbContext, aby ConnectionString nie używał tempowej bazy danych, a stałej. Można to zrobić w <i><b>TimeManager.WebAPI.Extensions.IServiceCollectionExtension.AddContextFactory(this IServiceCollection services)</b></i> w następujący sposób:
 </p>
 
```csharp
  services.AddDbContextFactory<DBContext>(options =>
  {
      options.UseSqlServer(ConfigurationHelper.DatabaseConnectionString);
  });
```      
<p>Aplikacja jest mocno wzorowana na wyglądzie i funkcjonalnościach aplikacji Google Calendar. Starałem się zrobić budżetową kopię, poniżej screeny efektu końcowego.</p>
<h2>Strona główna</h2>

![strona_glowna_nie_zal](https://github.com/EddyVuitton/TimeManager/assets/76602435/27aaf865-43a1-4cca-b768-ab360687cf9d)
<h2>Logowanie</h2>

![logowanie](https://github.com/EddyVuitton/TimeManager/assets/76602435/79638654-e683-4170-9bcf-beb5dffe31c8)
<h2>Rejestracja</h2>

![rejestracja](https://github.com/EddyVuitton/TimeManager/assets/76602435/46b42f4f-69cf-49a5-8bb7-9716e5f9090a)
<h2>Po zalogowaniu się</h2>

![menu_glowne_po_zal](https://github.com/EddyVuitton/TimeManager/assets/76602435/5746b147-9df8-4ece-8622-4863019f3654)
<h2>Okienko z informacjami zadania oraz możliwa edycja</h2>

![activity_popover](https://github.com/EddyVuitton/TimeManager/assets/76602435/23c2fcc5-db61-4527-82d2-1160ac93b3ca)

![aktualizacja_zadania](https://github.com/EddyVuitton/TimeManager/assets/76602435/73ce2009-0935-45a1-8043-5583b4af3e96)
<p>Po kliknięciu prawym przyciskiem myszy na zadaniu pojawi się małe menu z opcją szybkiego usunięcia zadania</p>

![mini_menu_zadania](https://github.com/EddyVuitton/TimeManager/assets/76602435/c40b3c77-0d95-4ff8-a227-1eefd600c858)
<h2>Tworzenie nowego zadania</h2>

![tworzenie_zadania](https://github.com/EddyVuitton/TimeManager/assets/76602435/5ac06acc-849c-4506-b528-f9e40776fcd0)
<p>Po wybraniu innej opcji niż "Nie powtarza się" zostanie dodane więcej niż jedno zadanie w zależności od wybranego przedziału czasowej, np. co tydzień:</p>

![zadania_powtarzajace_sie_co_tydzien](https://github.com/EddyVuitton/TimeManager/assets/76602435/ea0f2426-5b3d-45fc-9d1a-e0107ae6c3e9)
<h2>Strona z listami zadań</h2>

![strona_z_listami](https://github.com/EddyVuitton/TimeManager/assets/76602435/cc7f43f2-03f7-4228-b088-88376cd60a71)
<p>Dialog z tworzeniem nowej listy czy chowanie już istniejących:</p>

![tworzenie_listy](https://github.com/EddyVuitton/TimeManager/assets/76602435/48202d3a-7cec-4c75-adce-fad6f4bbec85)
![chowanie_listy](https://github.com/EddyVuitton/TimeManager/assets/76602435/546a2e20-e84b-452a-9c04-4567f3c4f455)

<h2>Menu listy</h2>

![menu_listy](https://github.com/EddyVuitton/TimeManager/assets/76602435/01ced257-9bb6-4c74-b0b3-1d7e5e836724)

![zmiana_nazwy_listy](https://github.com/EddyVuitton/TimeManager/assets/76602435/2d4cbf9c-33ac-49e0-b3b9-90f57917e6ea)

![tworzenie_zadania_ze_strony_z_listami](https://github.com/EddyVuitton/TimeManager/assets/76602435/4726fece-1b06-45cd-a2a6-16d8da21a390)

<p>W przypadku kiedy na liście są jakieś zadania to próba usunięcia listy pokaże dialog z zapytaniem:</p>

![usuwanie_listy_z_zadaniami](https://github.com/EddyVuitton/TimeManager/assets/76602435/4506ae4e-2cdb-452b-89f0-40b70e717f68)
<p>Kliknięcie prawym przyciskiej myszy na zadaniu z listy otworzy menu z opcjami, tj. usunięcie zadania, przeniesienie zadania czy utworzenie nowej listy i przeniesienie zadania do nowej listy:</p>

![menu_zadania](https://github.com/EddyVuitton/TimeManager/assets/76602435/837fcdac-ae70-4600-831e-7c134ca72266)
