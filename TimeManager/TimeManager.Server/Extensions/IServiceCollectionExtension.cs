using TimeManager.WebUI.Services.ManagementService;

namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection service)
    {
        service.AddScoped<IManagementService, ManagementService>();
    }
}