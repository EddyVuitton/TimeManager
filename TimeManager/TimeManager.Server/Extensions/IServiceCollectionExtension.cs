using MudBlazor.Services;

namespace TimeManager.Server.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection service)
    {
        service
            .AddRazorComponents()
            .AddInteractiveServerComponents();
        service.AddMudServices();
    }
}