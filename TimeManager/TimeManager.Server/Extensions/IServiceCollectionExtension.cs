using TimeManager.WebUI.Services.ManagementService;
using TimeManager.WebUI.Services.SnackbarService;

namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddApiServices(this IServiceCollection service)
    {
        service.AddScoped<IManagementService, ManagementService>();
        service.AddScoped<ISnackbarService, SnackbarService>();
    }
}