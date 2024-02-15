using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        ConfigurationHelper.Initialize(builder.Configuration);
        /*
         * W aplikacji Blazor, w której jest renderowane wiele stron korzystających z żądań HTTP chcemy,
         * aby w ramach tych żądań były tworzone osobne instancje kontekstu bazy danych oraz istniały jak najkrócej.
         * Metoda AddDbContextFactory<DbContext>() tworzy fabrykę typu DbContext co umożliwia szybkie stworzenie nowego kontekstu dla żądania
         * oraz zarządza jego cyklem życia.
         */
        builder.Services.AddContextFactory();
        builder.Services.AddBusinessLogics();
    }
}