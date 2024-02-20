using TimeManager.WebAPI.APIs.Management;
using TimeManager.WebAPI.APIs.Management.Interfaces;

namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection service)
    {
        service.AddScoped<IManagementService, ManagementService>();
    }
}