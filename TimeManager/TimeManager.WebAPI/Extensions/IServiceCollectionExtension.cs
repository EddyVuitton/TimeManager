﻿using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.Context;
using TimeManager.WebAPI.Helpers;
using TimeManager.WebAPI.Repositories.Account;
using TimeManager.WebAPI.Repositories.Management;

namespace TimeManager.WebAPI.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddContextFactory(this IServiceCollection services)
    {
        /*
         * W aplikacji Blazor, w której jest renderowane wiele stron korzystających z żądań HTTP chcemy,
         * aby w ramach tych żądań były tworzone osobne instancje kontekstu bazy danych oraz istniały jak najkrócej.
         * Metoda AddDbContextFactory<DbContext>() tworzy fabrykę typu DbContext co umożliwia szybkie stworzenie nowego kontekstu dla żądania
         * oraz zarządza jego cyklem życia.
         */
        services.AddDbContextFactory<DBContext>(options =>
        {
            options.UseSqlServer(ConfigurationHelper.TempDatabaseConnectionString);
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IManagement, Management>();
        services.AddScoped<IAccount, Account>();

        return services;
    }
}