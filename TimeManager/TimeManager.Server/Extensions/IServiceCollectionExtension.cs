using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TimeManager.Domain.Context;

namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection service, WebApplicationBuilder builder)
    {
        service
            .AddRazorComponents()
            .AddInteractiveServerComponents();
        service.AddMudServices();

        /*
         * W aplikacji Blazor, w której są renderowane wiele stron korzystające z żądań HTTP
         * chcemy, aby w ramach tych żądań były tworzone osobne instancje kontekstu bazy danych oraz
         * istniały jak najkrócej.
         * Metoda AddDbContextFactory<DbContext>() tworzy fabrykę DbContext co umożliwia szybkie stworzenie nowego kontekstu dla żądania
         * i zarządanie jego cyklem życia.
         */
        service.AddDbContextFactory<DBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("TempDatabase"));
        });
    }
}