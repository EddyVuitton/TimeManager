using Microsoft.EntityFrameworkCore;
using TimeManager.Domain.Context;
using TimeManager.WebAPI.APIs.Management;
using TimeManager.WebAPI.Helpers;

namespace TimeManager.WebAPI.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddContextFactory(this IServiceCollection services)
    {
        services.AddDbContextFactory<DBContext>(options =>
        {
            options.UseSqlServer(ConfigurationHelper.DatabaseConnectionString);
        });

        return services;
    }

    public static IServiceCollection AddBusinessLogics(this IServiceCollection services)
    {
        services.AddScoped<IManagementContext, Management>();

        return services;
    }
}