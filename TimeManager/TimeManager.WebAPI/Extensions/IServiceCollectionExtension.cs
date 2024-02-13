using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.Context;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection service, WebApplicationBuilder builder)
    {
        service.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        service.AddEndpointsApiExplorer();
        service.AddSwaggerGen();

        ConfigurationHelper.Initialize(builder.Configuration);
        /*
         * W aplikacji Blazor, w której jest renderowane wiele stron korzystających z żądań HTTP chcemy,
         * aby w ramach tych żądań były tworzone osobne instancje kontekstu bazy danych oraz istniały jak najkrócej.
         * Metoda AddDbContextFactory<DbContext>() tworzy fabrykę typu DbContext co umożliwia szybkie stworzenie nowego kontekstu dla żądania
         * oraz zarządza jego cyklem życia.
         */
        service.AddDbContextFactory<DBContext>(options =>
        {
            options.UseSqlServer(ConfigurationHelper.DatabaseConnectionString);
        });
    }
}