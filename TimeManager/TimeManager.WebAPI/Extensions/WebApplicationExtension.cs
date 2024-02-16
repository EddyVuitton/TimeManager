using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.Context;

namespace TimeManager.WebAPI.Extensions;

public static class WebApplicationExtension
{
    public static void ReMigrateDatabase(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<DBContext>()!;

        context.Database.EnsureDeleted(); //zdropuj bazę danych, jeżeli istnieje...
        context.Database.Migrate(); //i stwórz ją razem z jej obiektami

        //Przygotuj wstępne dane do testowania aplikacji
        SeedDataService.Initialize(context);
    }
}